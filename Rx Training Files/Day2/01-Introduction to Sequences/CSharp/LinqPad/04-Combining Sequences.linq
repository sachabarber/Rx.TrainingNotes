<Query Kind="Program" />

void Main()
{
	Test_Concatenating_two_sequences();
	Test_Concatenating_three_sequences();
	Test_Zipping_two_size_equal_sequences();
	Test_CartesianProduct();
	Test_CartesianProduct_Query_Comprehension_syntax();
	Test_Sequence_Join();
	Test_Sequence_Join_QCS();
	
//	Test_FastBySlow_join();
//	Test_SlowByFast_join();
}

// Define other methods and classes here

public void Test_Concatenating_two_sequences()
{
  var s1 = Enumerable.Range(0, 3);
  var s2 = Enumerable.Range(7, 2);

  var actual = Enumerable.Empty<int>();   //TODO Implement the concatenation of s1 & s2 here.

  var testResult = Enumerable.SequenceEqual(new[] { 0, 1, 2, 7, 8 },  actual.ToArray()) ? "Success" : "Fail";
  actual.Dump("Concat - " + testResult);
  
}

public void Test_Concatenating_three_sequences()
{
  var s1 = Enumerable.Range(0, 3);
  var s2 = Enumerable.Range(10, 3);
  var s3 = Enumerable.Range(20, 3);

  var actual = Enumerable.Empty<int>();   //TODO Implement the concatenation of s1, s2 & s3 here.

  var testResult = Enumerable.SequenceEqual(new[] { 0, 1, 2, 10, 11, 12, 20, 21, 22 },  actual.ToArray()) ? "Success" : "Fail";
  actual.Dump("Concat3 - " + testResult);
}

public void Test_Zipping_two_size_equal_sequences()
{
	var s1 = Enumerable.Range(0, 5);
	var s2 = Enumerable.Range(0, 5).Select(i=>(char)(i+65));
	
	var actual = Enumerable.Empty<Tuple<int,char>>();   //TODO pair the values by index from each sequence.
	
	var testResult = Enumerable.SequenceEqual(new[] {
			Tuple.Create(0,'A'),
			Tuple.Create(1,'B'),
			Tuple.Create(2,'C'),
			Tuple.Create(3,'D'),
			Tuple.Create(4,'E')
		},  actual.ToArray()) ? "Success" : "Fail";
	actual.Dump("Zip - " + testResult);
}

public void Test_CartesianProduct()
{
	var numbers = Enumerable.Range(1, 3);
	var letters = new[]{'x','y','z'};

	var actual = Enumerable.Empty<Tuple<int,char>>();   //TODO Create the Cartesian Product of numbers and letters
	
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
		},  actual.ToArray()) ? "Success" : "Fail";
	actual.Dump("CartesianProduct - " + testResult);
}

public void Test_CartesianProduct_Query_Comprehension_syntax()
{
	var numbers = Enumerable.Range(1, 3);
	var letters = new[]{'x','y','z'};

	//TODO Create the Cartesian Product of numbers and letters, this time using Query Comprehension Syntax
	var actual = from x in Enumerable.Empty<Tuple<int,char>>()
				 select x;
	
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
		},  actual.ToArray()) ? "Success" : "Fail";
	actual.Dump("CartesianProduct QCS - " + testResult);
}

public void Test_Sequence_Join()
{
	var s1 = new []{
		Tuple.Create(0,"zero"),
		Tuple.Create(1,"one"),
		Tuple.Create(2,"two"),
		Tuple.Create(3,"three"),
	};
	var s2 = new[] {
		
		Tuple.Create(1,"uno"),
		Tuple.Create(2,"dos"),
		Tuple.Create(3,"tres"),
		Tuple.Create(4,"cuatro"),
	};
	
	var actual = Enumerable.Empty<Tuple<string,string>>();   //TODO pair the values by index from each sequence.
		
	var testResult = Enumerable.SequenceEqual(new[] {
			Tuple.Create("one","uno"),
			Tuple.Create("two","dos"),
			Tuple.Create("three","tres")
		},  actual.ToArray()) ? "Success" : "Fail";
	actual.Dump("Join - " + testResult);
}
public void Test_Sequence_Join_QCS()
{
	var s1 = new []{
		Tuple.Create(0,"zero"),
		Tuple.Create(1,"one"),
		Tuple.Create(2,"two"),
		Tuple.Create(3,"three")
	};
	var s2 = new[] {
		
		Tuple.Create(1,"uno"),
		Tuple.Create(2,"dos"),
		Tuple.Create(3,"tres"),
		Tuple.Create(4,"cuatro")
	};
	
	var actual = from x in Enumerable.Empty<Tuple<string,string>>()   //TODO pair the values by index from each sequence.
				 select x;
	
	var testResult = Enumerable.SequenceEqual(new[] {
			Tuple.Create("one","uno"),
			Tuple.Create("two","dos"),
			Tuple.Create("three","tres")
		},  actual.ToArray()) ? "Success" : "Fail";
	actual.Dump("Join QCS - " + testResult);
}


public void Test_FastBySlow_join()
{
	var timer = Stopwatch.StartNew();
	var fast = Enumerable.Range(1,3);
	var slow = SlowIntegers().Take(3);
	
	var actual = from x in fast
				 from y in slow
				 select new { fast = x, slow = y };
	
	actual.Dump("Fast by slow");
	timer.Stop();
	timer.Dump("Fast by slow - time");
	
}

public void Test_SlowByFast_join()
{
	var timer = Stopwatch.StartNew();
	var fast = Enumerable.Range(1,3);
	var slow = SlowIntegers().Take(3);
	
	var actual = from x in slow
				 from y in fast
				 orderby y
				 select new { fast = y, slow = x };
	
	actual.Dump("Slow by fast");
	timer.Stop();
	timer.Dump("Slow by fast - time");
}

public IEnumerable<int> SlowIntegers()
{

	var i = 1;
	while(true)
	{
		Thread.Sleep(200);
		yield return i++;
	}
}