<Query Kind="Program" />

void Main()
{
	Generate(4, i=>i-1).Select(i=>100/i).Take(4).Dump("Expected [25, 33, 50, 100]");
	Generate(4, i=>i-1).Select(i=>100/i).Take(5).Dump("Expected DivideByZero exception");
}

// Define other methods and classes here
public IObservable<T> Generate<T>(T seed, Func<T, T> increment)
{
	//TODO:
}