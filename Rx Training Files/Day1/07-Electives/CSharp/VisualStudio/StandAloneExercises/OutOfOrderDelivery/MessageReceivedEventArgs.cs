using System;

namespace StandAloneExercises.OutOfOrderDelivery
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(string command, string payload)
        {
            Command = command;
            Payload = payload;
        }

        public string Command { get; private set; }
        public string Payload { get; private set; }
    }
}