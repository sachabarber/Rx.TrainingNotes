using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace StandAloneExercises.OutOfOrderDelivery
{
    public class Service
    {
        private readonly Network _network;

        public Service(Network network)
        {
            _network = network;
        }

        public Task<string> Request(string command)
        {
            //TODO: Implementation goes here
            var result  = Observable.FromEventPattern<MessageReceivedEventArgs>(_network, "MessageReceived")
                 .Where(x => x.EventArgs.Command == command).Select(x => x.EventArgs.Payload).Take(1).ToTask();

            _network.Request(command);
            return result;
        }
    }
}