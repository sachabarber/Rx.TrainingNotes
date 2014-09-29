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
            _network.Request(command);
            //TODO:
            throw new NotImplementedException();
        }
    }
}