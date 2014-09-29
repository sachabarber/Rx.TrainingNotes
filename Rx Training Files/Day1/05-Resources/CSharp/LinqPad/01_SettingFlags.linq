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
	//Here we show a classic pattern of setting a "Processing" flag, doing 
	//	some work and then resetting the flag.
}

// This is out field that we will set to indicate we are processing
bool _isProcessing = false;

//This sample uses the classic try/finally to ensure the flag is reset
public void SettingFlags_Sample1()
{
	_isProcessing = true;
	try
	{	        
		//Some work goes here
	}
	finally
	{
		_isProcessing = false;
	}
}

//This sample leverages both the C# using language feature and Rx's Disposable tools.
public void SettingFlags_Sample2()
{
	_isProcessing = true;
	using(Disposable.Create(()=>_isProcessing=false))
	{	        
		//Some work goes here
	}	
}


//This sample is useful if you have a concern over reentrancy.
int _processingRefCount = 0;
public bool IsProcessing()
{
	return _processingRefCount > 0;
}
public void SettingFlags_Sample3()
{
	_processingRefCount++;
	using(Disposable.Create(()=>_processingRefCount--))
	{	        
		//Some work goes here
	}	
}