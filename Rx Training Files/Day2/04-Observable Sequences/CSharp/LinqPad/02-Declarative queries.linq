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
	var integers = Observable.Range(0,5);
	
	// Exercise 1 - Add 5
	integers.Select(i=>i+5).Dump("Add 5 to each element");
	
	//Exercise 2 - To ['A', 'B', 'C', 'D', 'E']
	integers.Dump("Integers to Chars");
	
	//Exercise 3 - [0,2,4]
	integers.Dump("Even Integers");
	
	//Exercise 4 - To 10
	integers.Dump("Sum");
	
	//Exercise 5
	integers.Dump("Running Sum");
}

// Define other methods and classes here