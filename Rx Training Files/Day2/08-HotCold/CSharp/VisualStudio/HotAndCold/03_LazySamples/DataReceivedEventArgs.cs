using System;

namespace HotAndCold._03_LazySamples
{
    public class DataReceivedEventArgs : EventArgs
    {
        private readonly string _data;

        public string Data
        {
            get { return _data; }
        }

        public DataReceivedEventArgs(string data)
        {
            _data = data;
        }
    }
}