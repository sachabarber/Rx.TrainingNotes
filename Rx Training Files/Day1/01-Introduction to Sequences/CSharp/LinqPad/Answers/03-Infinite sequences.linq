<Query Kind="Program" />

void Main()
{
	AllPositiveIntegers().Take(10).Dump();
	Generate(4, i=>i-1).Select(i=>100/i).Take(4).Dump("Expected [25, 33, 50, 100]");
	Generate(4, i=>i-1).Select(i=>100/i).Take(5).Dump("Expected DivideByZero exception");
}

// Define other methods and classes here
public IEnumerable<int> AllPositiveIntegers()
{
	var i = 1;
	while(true)
	{
		yield return i++;
	}
}

public IEnumerable<T> Generate<T>(T seed, Func<T, T> increment)
{
	var i = seed;
	while (true)
	{
		yield return i;
		i = increment(i);		
	}
}