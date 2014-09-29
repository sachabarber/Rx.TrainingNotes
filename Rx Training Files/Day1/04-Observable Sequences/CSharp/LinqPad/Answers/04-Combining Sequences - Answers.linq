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
	Test_Concatenating_two_sequences();
	Test_Concatenating_three_sequences();
	Test_Zipping_two_size_equal_sequences();
	Test_CartesianProduct();
	Test_CartesianProduct_Query_Comprehension_syntax();
	Test_Sequence_Join_QCS();
	Test_FastBySlow_join();
	Test_SlowByFast_join();
}

// Define other methods and classes here

public void Test_Concatenating_two_sequences()
{
  var s1 = Observable.Range(0, 3);
  var s2 = Observable.Range(7, 2);

  var actual = s1.Concat(s2);

  var testResult = Enumerable.SequenceEqual(new[] { 0, 1, 2, 7, 8 },  actual.ToArray().Single()) ? "Success" : "Fail";
  actual.Dump("Concat - " + testResult);
  
}

public void Test_Concatenating_three_sequences()
{
  var s1 = Observable.Range(0, 3);
  var s2 = Observable.Range(10, 3);
  var s3 = Observable.Range(20, 3);

  var actual = s1.Concat(s2).Concat(s3);

  var testResult = Enumerable.SequenceEqual(new[] { 0, 1, 2, 10, 11, 12, 20, 21, 22 },  actual.ToArray().Single()) ? "Success" : "Fail";
  actual.Dump("Concat3 - " + testResult);
}


public void Test_Zipping_two_size_equal_sequences()
{
	var s1 = Observable.Range(0, 5);
	var s2 = Observable.Range(0, 5).Select(i=>(char)(i+65));
	
	var actual = s1.Zip(s2, Tuple.Create);
	
	var testResult = Enumerable.SequenceEqual(new[] {
			Tuple.Create(0,'A'),
			Tuple.Create(1,'B'),
			Tuple.Create(2,'C'),
			Tuple.Create(3,'D'),
			Tuple.Create(4,'E')
		},  actual.ToArray().Single()) ? "Success" : "Fail";
	actual.Dump("Zip - " + testResult);
}

public void Test_CartesianProduct()
{
	var numbers = Observable.Interval(TimeSpan.FromMilliseconds(50)).Take(3).Select(i=>(int)(i+1));//Observable.Range(1, 3);
	var letters = Observable.Range(1, 3).Select(i=>(char)(i+119));//new[]{'x','y','z'}.ToObservable();

	var actual = numbers.SelectMany (num => letters, Tuple.Create);
	
	var testResult = Enumerable.SequenceEqual(new[] {
			Tuple.Create(1,'x'),
			Tuple.Create(1,'y'),
			Tuple.Create(1,'z'),
			Tuple.Create(2,'x'),
			Tuple.Create(2,'y'),
			Tuple.Create(2,'z'),
			Tuple.Create(3,'x'),
			Tuple.Create(3,'y'),
			Tuple.Create(3,'z')
		},  actual.ToArray().Single()) ? "Success" : "Fail";
	actual.Dump("CartesianProduct - " + testResult);
}

public void Test_CartesianProduct_Query_Comprehension_syntax()
{
	var numbers = Observable.Interval(TimeSpan.FromMilliseconds(50)).Take(3).Select(i=>(int)(i+1));//Observable.Range(1, 3);
	var letters = Observable.Range(1, 3).Select(i=>(char)(i+119));//new[]{'x','y','z'}.ToObservable();

	var actual = from num in numbers
				 from letter in letters
				 select Tuple.Create(num,letter);
	
	var testResult = Enumerable.SequenceEqual(new[] {
			Tuple.Create(1,'x'),
			Tuple.Create(1,'y'),
			Tuple.Create(1,'z'),
			Tuple.Create(2,'x'),
			Tuple.Create(2,'y'),
			Tuple.Create(2,'z'),
			Tuple.Create(3,'x'),
			Tuple.Create(3,'y'),
			Tuple.Create(3,'z')
		},  actual.ToArray().Single()) ? "Success" : "Fail";
	actual.Dump("CartesianProduct QCS - " + testResult);
}

public void Test_Sequence_Join_QCS()
{
	var s1 = new []{
		Tuple.Create(0,"zero"),
		Tuple.Create(1,"one"),
		Tuple.Create(2,"two"),
		Tuple.Create(3,"three")
	}.ToObservable();
	var s2 = new[] {
		
		Tuple.Create(1,"uno"),
		Tuple.Create(2,"dos"),
		Tuple.Create(3,"tres"),
		Tuple.Create(4,"cuatro"),
	}.ToObservable();
	
	var actual = from eng in s1
				 from esp in s2
				 where eng.Item1 == esp.Item1
				 select Tuple.Create(eng.Item2, esp.Item2);
	
	var testResult = Enumerable.SequenceEqual(new[] {
			Tuple.Create("one","uno"),
			Tuple.Create("two","dos"),
			Tuple.Create("three","tres")
		},  actual.ToArray().Single()) ? "Success" : "Fail";
	actual.Dump("Join QCS - " + testResult);
}

public void Test_FastBySlow_join()
{
	
	var fast = Observable.Range(1,3);
	var slow = SlowIntegers().Take(3);
	
	var actual = from x in fast
				 from y in slow
				 select new { fast = x, slow = y };
	
	actual.Log("Fast by slow").Subscribe();
}

public void Test_SlowByFast_join()
{
	var fast = Observable.Range(1,3);
	var slow = SlowIntegers().Take(3);
	
	var actual = (from x in slow
				 from y in fast
				 //orderby y 	//Not supported.
				 select new { fast = y, slow = x })
				 ;
	
	
	actual.Log("Slow by fast").Subscribe();
}

public IObservable<int> SlowIntegers()
{
	return Observable.Interval(TimeSpan.FromMilliseconds(200)).Select(l=>(int)l);
}

public static class ObsEx
{
	public static IObservable<T> Log<T>(this IObservable<T> source, string name)
	{
		return Observable.Create<T>(
			o =>
			{
					"Subscribe()".Dump(name);
				
					var timer = Stopwatch.StartNew();
					var timerLog = Disposable.Create(()=> {
						timer.Stop();
						timer.Elapsed.Dump(name);
					});
				
				var disposal = Disposable.Create(() => "Dispose()".Dump(name));
					var subscription = source.Subscribe(o);
				return new CompositeDisposable(disposal, subscription, timerLog);
			});
	}
}