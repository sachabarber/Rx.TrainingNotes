using System;
using System.Reactive.Concurrency;

namespace StandAloneExercises.OutOfOrderDelivery
{
    public class Network
    {
        private readonly IScheduler _scheduler;
        public Network(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void Request(string command)
        {
            switch (command)
            {
                case "A":
                    _scheduler.Schedule(TimeSpan.FromSeconds(5), () => RaiseMessageReceived(new MessageReceivedEventArgs(command, "Message A")));
                    break;
                case "B":
                    _scheduler.Schedule(TimeSpan.FromSeconds(2), () => RaiseMessageReceived(new MessageReceivedEventArgs(command, "Message B")));
                    break;
                case "C":
                    _scheduler.Schedule(TimeSpan.FromSeconds(3), () => RaiseMessageReceived(new MessageReceivedEventArgs(command, "Message C")));
                    break;
                case "D":
                    RaiseMessageReceived(new MessageReceivedEventArgs(command, "Message D"));
                    break;
            }
        }


        /*
         * The Observable.FromEventPattern<MessageReceivedEventArgs> factory may be useful here if you decide to use it.
         */
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        protected virtual void RaiseMessageReceived(MessageReceivedEventArgs e)
        {
            var handler = MessageReceived;
            if (handler != null) handler(this, e);
        }
    }
}