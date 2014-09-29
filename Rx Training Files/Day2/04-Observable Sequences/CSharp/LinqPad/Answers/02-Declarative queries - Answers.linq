<Query Kind="Program">
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

void Main()
{
	var integers = Observable.Range(0,5);
	
	// Exercise 1 - Add 5
	integers.Select(i=>i+5).Dump("Add 5 to each element");
	
	//Exercise 2 - To ['A', 'B', 'C', 'D', 'E']
	integers.Select(i=>((char)(i+65))).Dump("Integers to Chars");
	
	//Exercise 3 - [0,2,4]
	integers.Where(i=>i%2==0).Dump("Even Integers");
	
	//Exercise 4 - To 10
	integers.Sum().Dump("Sum");
	integers.Aggregate((acc, cur)=>acc+cur).Dump("Sum - with aggregate");
	
	//Exercise 5
	integers.Scan(0, (acc, cur)=>acc+cur).Dump("Running Sum");
}

// Define other methods and classes here