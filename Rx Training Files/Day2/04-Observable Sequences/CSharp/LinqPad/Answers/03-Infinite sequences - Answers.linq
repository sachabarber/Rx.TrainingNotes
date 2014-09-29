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
	Generate(4, i=>i-1).Select(i=>100/i).Take(4).Dump("Expected [25, 33, 50, 100]");
	Generate(4, i=>i-1).Select(i=>100/i).Take(5).Dump("Expected DivideByZero exception");
}

// Define other methods and classes here
public IObservable<T> Generate<T>(T seed, Func<T, T> increment)
{
	return Observable.Generate(seed, _=>true, increment, x=>x);
}