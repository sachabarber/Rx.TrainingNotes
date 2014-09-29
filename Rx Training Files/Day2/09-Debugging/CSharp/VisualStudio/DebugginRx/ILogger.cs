namespace DebugginRx
{
    /// <summary>
    /// A simple logging interface. 
    /// The point is to remove focus from the logging library/framework and focus on the Rx code.
    /// </summary>
    public interface ILogger
    {
        void Log(string input);
    }
}