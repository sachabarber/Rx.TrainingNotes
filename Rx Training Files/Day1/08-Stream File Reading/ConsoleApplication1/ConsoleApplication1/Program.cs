using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = Path.Combine(Directory.GetCurrentDirectory(), "TextFile1.txt");
            Program p = new Program();
            
            
            //get lines simple
            //var actual = p.GetLinesWithCancellation(fileName).Take(5);
            //var sub = actual.Subscribe(Console.WriteLine);

            //get lines progress
            var actual = p.GetLinesWithByteProgress(fileName);
            var sub = actual.Subscribe(Console.WriteLine);

            Console.ReadLine();
        }



        public IObservable<string> GetLinesWithCancellation(string fileName)
        {
            return Observable.Create<string>(
                async (observer, ct) =>
                {
                    string line;
                    using (StreamReader file = new StreamReader(fileName))
                    {
                        try
                        {
                            while ((line = await file.ReadLineAsync()) != null && !ct.IsCancellationRequested)
                            {
                                observer.OnNext(line);
                            }
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                        }
                    }
                    observer.OnCompleted();
                });
        }

        public IObservable<LineWithByteProgress> GetLinesWithByteProgress(string fileName)
        {

        



            return Observable.Create<LineWithByteProgress>(
                async (IObserver<LineWithByteProgress> observer) =>
                {

                    string line;

                    // Read the file and display it line by line.
                    System.IO.StreamReader file = new System.IO.StreamReader(fileName);
                    var length = file.BaseStream.Length;
                    long readSoFar = 0;
                    try
                    {
                        while ((line = await file.ReadLineAsync()) != null)
                        {
                            var lineBytesLength = Encoding.ASCII.GetBytes(line).Length;
                            readSoFar += lineBytesLength;
                            var progress = new LineWithByteProgress();
                            progress.Line = line;
                            progress.LineBytes = lineBytesLength;
                            progress.TotalBytes = length;
                            progress.BytesReadSoFar = readSoFar;


                            observer.OnNext(progress);
                        }
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                    }
                    observer.OnCompleted();

                    file.Close();


                    return Disposable.Create(() =>
                    {
                        file.Close();
                        file.Dispose();
                    });

                });
        }

    
    
    
    
    }

    public class LineWithByteProgress
    {
        public string Line { get; set; }
        public long LineBytes { get; set; }
        public long TotalBytes { get; set; }
        public long BytesReadSoFar { get; set; }
        public long Perc { get; set; }


        public override string ToString()
        {
            return string.Format("Line: {0}, LineBytes: {1}, TotalBytes: {2}, BytesReadSoFar: {3}, Perc: {4}", 
                Line, LineBytes, TotalBytes, BytesReadSoFar, Perc);
        }
    }




}
