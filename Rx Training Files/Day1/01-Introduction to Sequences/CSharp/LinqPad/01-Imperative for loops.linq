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
	var integers = new int[5]{0,1,2,3,4};
	// Exercise 1
	ArrayAdd5(integers).Dump("Add 5 to each element");
	
	//Exercise 2
	//ArrayIntToArrayChar(integers).Dump("Integers to Chars");
	
	//Exercise 3
	//ArrayIntToArrayEvenInt(integers).Dump("ArrayIntToArrayEvenInt");
	
	//Exercise 4
	//ArrayTakeFirst3(integers).Dump("Take first 3 values");
	
	//Exercise 5
	//ArrayIntToSum(integers).Dump("ArrayIntToSum");
	
	//Exercise 6
	//ArrayIntToRunningSum(integers).Dump("ArrayIntToRunningSum");
}

// Define other methods and classes here
public int[] ArrayAdd5(int[] source)
{
	//Take the sequence [0,1,2,3,4]
	// and create the sequence [5,6,7,8,9]
	var output = new int[source.Length];
	for (int i = 0; i < source.Length; i++)
	{
		output[i] = source[i] + 5;
	}
	return output;
}

public char[] ArrayIntToArrayChar(int[] source)
{
	//Take the sequence [0,1,2,3,4]
	// and create the sequence [A,B,C,D,E]
	var output = new char[source.Length];
	for (int i = 0; i < source.Length; i++)
	{
		output[i] = default(char); //TODO: Correct this line
	}
	return output;
}

public int[] ArrayIntToArrayEvenInt(int[] source)
{
	//Take the sequence [0,1,2,3,4]
	// and create the sequence [0,2,4]
	
	for (int i = 0; i < source.Length; i++)
	{
	
	}
	
	return default(int[]);
}

public int[] ArrayTakeFirst3(int[] source)
{
	//Take the sequence [0,1,2,3,4]
	// and create the sequence [0,1,2]
	return default(int[]);
}

public int ArrayIntToSum(int[] source)
{
	//Take the sequence [0,1,2,3,4]
	// and return the value 10

	for (int i = 0; i < source.Length; i++)
	{
		
	}
	return default(int);
}

public int[] ArrayIntToRunningSum(int[] source)
{
	//Take the sequence [0,1,2,3,4]
	// and return the value [0,1,3,6,10]
	
	for (int i = 1; i < source.Length; i++)
	{
		
	}
	return default(int[]);
}