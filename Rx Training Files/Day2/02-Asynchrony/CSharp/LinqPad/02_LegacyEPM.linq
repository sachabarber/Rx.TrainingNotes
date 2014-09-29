<Query Kind="Program">
  <Namespace>System.ComponentModel</Namespace>
</Query>

void Main()
{
	var filePath = @"C:\Users\Lee\Documents\GitHub\RxTraining\02-Asynchrony\CSharp\Visual Studio\Asynchrony\bin\Debug\LargeTextFile.txt";
	
	Thread.CurrentThread.ManagedThreadId.Dump("Initial Thread");
	
	var bgWorker = new BackgroundWorker();
	bgWorker.DoWork+=Work;
	bgWorker.RunWorkerCompleted+=Completed;
	bgWorker.RunWorkerAsync(filePath);
}

// Define other methods and classes here
private void Work(object sender, DoWorkEventArgs e)
{
	var caller = (BackgroundWorker)sender;
	var filePath = (string)e.Argument;
	
	Thread.CurrentThread.ManagedThreadId.Dump("WorkerThread");
	e.Result = File.ReadAllBytes(filePath).Length;	//Yeah silly. Could just use FileInfo. -LC
}

private void Completed(object sender, RunWorkerCompletedEventArgs e)
{
	Thread.CurrentThread.ManagedThreadId.Dump("Completion Thread");
	e.Dump();
}