<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	//This is only a sample set of declarations. 
	// No running code here.


	//1)Explicit null with Option/Maybe monad
	//2)Explicit failures with Try monad
	//3)Dealing with Option<Option<T>> by using FlatMap/SelectMany
}

class Maybe<T>
{
    public readonly static Maybe<T> Nothing = new Maybe<T>();
    public T Value { get; private set; }
    public bool HasValue { get; private set; }
    Maybe()
    {
        HasValue = false;
    }
    public Maybe(T value)
    {
        Value = value;
        HasValue = true;
    }
}
//Or an alternative implementation
public abstract class Option<T>
{	
	public abstract bool HasValue { get; }
	public abstract T Value { get; }
}
public sealed class Some<T> : Option<T>
{
	private readonly T _value;
	public Some(T value)
	{
		_value = value;
	}
	public override bool HasValue { get { return true; } }
	public override T Value { get{ return _value;} }
}
public sealed class None<T> : Option<T>
{
	public override bool HasValue { get { return false; } }
	public override T Value { get { throw new InvalidOperationException();} }
}


public abstract class Try<T>
{
	public abstract bool IsSuccess { get; }
	public abstract Exception Error { get; }
	public abstract T Value { get;  }
}
public sealed class Success<T> : Try<T>
{
	private readonly T _value;
	public Success(T value)
	{
		_value = value;
	}
	public override bool IsSuccess { get { return true; } }
	public override Exception Error { get { throw new InvalidOperationException();} }
	public override T Value { get{ return _value;} }
}
public sealed class Failure<T> : Try<T>
{
	private readonly Exception _error;
	public Failure(Exception error)
	{
		_error = error;
	}
	public override bool IsSuccess { get { return false; } }
	public override Exception Error { get { return _error;} }
	public override T Value { get { throw new InvalidOperationException();} }
}



//Example usage
public interface IEmployee
{
	Guid Id { get; }
	string Name { get; }
	string PhoneNumber {get;}
	Option<string> Spouse{ get; }				//May not have a spouse
	Option<IEmployee> Manager { get; }			//May not have a manager (e.g the Boss)
	Task<decimal> OutstandingPay { get;}		//Heavy calculation or lookup to get this value.
	
	Try<IEmployee> AddLineReport(Guid employeeId);	//May not be valid e.g. Circular references or invalid Id.
}