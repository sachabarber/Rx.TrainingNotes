<Query Kind="Program">
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.PlatformServices</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var bufferSize = 65536; //4096 = 4k, 65536 = 64k. Variations in read times 4k~25s 64k~7s 256k~7s
	var filePath = @"C:\Users\Lee\Documents\GitHub\RxTraining\02-Asynchrony\CSharp\Visual Studio\Asynchrony\bin\Debug\LargeTextFile.txt";
	var reader = new AsyncFileReader(bufferSize, filePath, line=>{/*Do something with the line here...*/});
	reader.Read();
	
	//How do I know when it is done?
	
	//Console.ReadLine();
	//reader.Dispose();
}

// Define other methods and classes here
public class AsyncFileReader : IDisposable
{
	private static readonly string[] LineSplit = { Environment.NewLine };
	private readonly byte[] _buffer;
	private readonly FileStream _fileStream;
	private readonly Action<string> _onLineRead;
	private string _currentText;
	private readonly Stopwatch _timer;
	
	public AsyncFileReader(int bufferSize, string filePath, Action<string> onLineRead)
	{
		_buffer = new byte[bufferSize];
		_fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous);
		_onLineRead = onLineRead;
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
		//How do I tell the user that the process is complete? An event, callback, status Property?
		(isSuccessful ? "Done" : "Error").Dump("Termination");
		_timer.Stop();
		_timer.Elapsed.Dump("Elapsed time");
	}
	public void Dispose()
	{
		if (_fileStream != null)
		{
			_fileStream.Dispose();
		}
	}
}