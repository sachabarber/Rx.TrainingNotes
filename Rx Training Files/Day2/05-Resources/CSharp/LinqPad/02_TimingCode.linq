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
	//Here we show a classic pattern timing how long a 
	//	block of code runs.
}

//This sample simply uses a Stopwatch instance and a try/finally block to time a block of code
public void Timing_Sample1()
{
	var timer = Stopwatch.StartNew();
	try
	{	        
		//Some work goes here	
	}
	finally
	{
		Console.WriteLine(timer.Elapsed);	
	}
}

//In this sample we want to time both the outer and inner blocks of code (e.g. an If or For block)
public void NestedTiming_Sample()
{
	var timer = Stopwatch.StartNew();
	try
	{	        
		//Some work goes here
		var timer2 = Stopwatch.StartNew();
		try
		{	        
			//Some inner work goes here
		}
		finally
		{
			Console.WriteLine(timer2.Elapsed);	
		}
	}
	finally
	{
		Console.WriteLine(timer.Elapsed);	
	}
}

//Here we leverage a custom Timer class (defined below) that implements IDiposable to time our code
public void Timing_Sample1()
{
	using(new Timer("Timing_Sample1"))
	{
		//Some work goes here
	}
}

//Again leveraging the using pattern, we see less noise (try/finally/variables)
public void NestedTiming_Sample2()
{
	using(new Timer("outer"))
	{
		//Some work goes here
		using(new Timer("inner"))
		{
			//Some inner work goes here
		}
	}
}

/// <summary>
/// A timer class that will 'log' its elapsed lifetime when disposed.
/// </summary>
public class Timer : IDisposable
{
	string _name;
	Stopwatch _stopwatch;
	
	public Timer(string name)
	{
		_name = name;
		_stopwatch = Stopwatch.StartNew();
	}
	
	public void Dispose()
	{
		Console.WriteLine ("{0} took {1}", _name, _stopwatch.Elapsed);
	}
}