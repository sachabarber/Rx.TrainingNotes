using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace Asynchrony
{
    //Very good examples on how to consume the APM pattern here http://www.codeproject.com/Articles/37244/Understanding-the-Asynchronous-Programming-Model
    //Also Jeffrey Richter's CLR via C# explains it very well.
    [TestFixture]
    public class LegacyConstructsAPM
    {
        [Test]
        public void Reading_a_file_Syncronously()
        {
            var binaryContents = File.ReadAllBytes(Program.LargeTextFilePath);
            var textContents = File.ReadAllText(Program.LargeTextFilePath);
            var allLines = File.ReadAllLines(Program.LargeTextFilePath);
        }

        //So much to remember to get his right:
        // Ensure you open the Stream specifying the FileOptions.Async
        //  Check the bytes read, it wont always be the fill amount you asked for
        //  Need to provide my own synchronization and callback system to know when things are done
        [Test]
        public void Read_file_with_APM()
        {
            // Show the ID of the thread executing Main
            Console.WriteLine("Main thread ID={0}", Thread.CurrentThread.ManagedThreadId);

            var bufferSize = 65536; //4096 = 4k, 65536 = 64k. Variations in read times 4k~25s 64k~7s 256k~7s
            var filePath = @"C:\Users\Lee\Documents\GitHub\RxTraining\02-Asynchrony\CSharp\Visual Studio\Asynchrony\bin\Debug\LargeTextFile.txt";
            var resetEvent = new AutoResetEvent(false);
            var lineCount = 0;
            var reader = new AsyncFileReader(bufferSize, filePath, line => {/*Do something with the line here...*/
                                                                               lineCount++;
            }, ()=>resetEvent.Set());
            reader.Read();

            resetEvent.WaitOne(TimeSpan.FromSeconds(5));
            reader.Dispose();
            Console.WriteLine(lineCount);

        }
        public class AsyncFileReader : IDisposable
        {
            private static readonly string[] LineSplit = { Environment.NewLine };
            private readonly byte[] _buffer;
            private readonly FileStream _fileStream;
            private readonly Action<string> _onLineRead;
            private readonly Action _onTermination;
            private string _currentText;
            private readonly Stopwatch _timer;

            public AsyncFileReader(int bufferSize, string filePath, Action<string> onLineRead, Action onTermination)
            {
                _buffer = new byte[bufferSize];
                _fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous);
                _onLineRead = onLineRead;
                _onTermination = onTermination;
                _timer = new Stopwatch();
            }

            public void Read()
            {
                _timer.Start();
                _fileStream.BeginRead(_buffer, 0, _buffer.Length, OnBufferRead, null);
            }

            private void OnBufferRead(IAsyncResult asyncResult)
            {
                // Show the ID of the thread executing ReadIsDone
                var currentThread = Thread.CurrentThread.ManagedThreadId;

                // Get the result
                Int32 bytesRead;
                try
                {
                    //Catch errors here. Do I expose them to the end user? As an event, a callback, a Status property?
                    bytesRead = _fileStream.EndRead(asyncResult);
                }
                catch (ObjectDisposedException ode)
                {
                    bytesRead = -1;
                    OnTermination(false);
                }

                if (bytesRead > 0)
                {
                    ProcessBuffer(bytesRead);
                    //Recall it
                    Read();

                }
                else
                {
                    //Can close the fileStream now.
                    _fileStream.Dispose();
                    OnTermination(true);
                }
            }

            private void ProcessBuffer(int bytesRead)
            {
                //Read through bytes into string. Return lines via _onLineRead callback.
                var actualBuffer = new byte[bytesRead];
                Array.Copy(_buffer, actualBuffer, bytesRead);
                var text = Encoding.UTF8.GetString(actualBuffer);
                _currentText += text;

                var lines = _currentText.Split(LineSplit, StringSplitOptions.None);
                if (lines.Length > 1)
                {
                    var linesToReturn = lines.Take(lines.Length - 1).ToArray();
                    foreach (var line in linesToReturn)
                    {
                        _onLineRead(line);
                    }
                    var endIdx = linesToReturn.Sum(line => line.Length) + (Environment.NewLine.Length * linesToReturn.Length);
                    _currentText = _currentText.Remove(0, endIdx);
                }
                Array.Clear(actualBuffer, 0, actualBuffer.Length);
                actualBuffer = null;
            }

            private void OnTermination(bool isSuccessful)
            {
                Console.WriteLine((isSuccessful ? "Done" : "Error"));
                _timer.Stop();
                Console.WriteLine("Elapsed time : {0}", _timer.Elapsed);

                //How do I tell the user that the process is complete? An event, callback, status Property?
                _onTermination();
            }
            public void Dispose()
            {
                if (_fileStream != null)
                {
                    _fileStream.Dispose();
                }
            }
        }

    }
}
