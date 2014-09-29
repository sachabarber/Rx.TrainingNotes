using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.ServiceModel;
using DuplexWcfChat.ChatServiceReference;

namespace DuplexWcfChat
{
    class Program
    {
        private static readonly Uri PrimaryUri = new Uri("net.tcp://localhost:4505/ChatService");
        private static readonly Uri SecondaryAddress = new Uri("net.tcp://localhost:4506/ChatService");

        static void Main(string[] args)
        {
            var myAddress = PrimaryUri;
            var otherAddress = SecondaryAddress;
            if (args != null && args.Length > 0)
            {
                myAddress = SecondaryAddress;
                otherAddress = PrimaryUri;
            }

            var svc = new ChatService();
            using (var host = new ServiceHost(svc, myAddress))
            {
                StartHost(host, myAddress);
                Console.WriteLine("Welcome to Rx Chat.");
                using (IncomingMessages(otherAddress).Subscribe(RecieveMessage, ex=>OtherTerminated("Fault"), ()=>OtherTerminated("Exited")))
                {
                    ProcessChat(svc);
                    host.Close();
                }
                
            }
        }

        private static void OtherTerminated(string reason)
        {
            using (ConsoleColorScope(ConsoleColor.Gray))
            {
                Console.WriteLine("Other server unavailable : {0}", reason);
                Console.WriteLine("Closing.");
                Environment.Exit(0);
            }
            
        }

        private static void StartHost(ICommunicationObject host, Uri myAddress)
        {
            host.Open();
            using (ConsoleColorScope(ConsoleColor.DarkGray))
            {
                Console.WriteLine("Service is running...");
                Console.WriteLine("Service address: " + myAddress);
                Console.WriteLine("Type 'exit' to quit.");
            }
        }

        private static void ProcessChat(ChatService svc)
        {
            var shouldExit = false;

            while (!shouldExit)
            {
                var message = Console.ReadLine();
                if (string.Equals(message, "exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    shouldExit = true;
                }
                else
                {
                    svc.Send(message);
                }
            }
        }

        private static IObservable<string> IncomingMessages(Uri otherAddress)
        {
            return Observable.Create<string>(
                o =>
                {
                    var sink = new ChatCallBack();
                    var subscription = sink.IncomingMessages.Subscribe(o);
                    var binding = new NetTcpBinding(SecurityMode.None);
                    var endpoint = new EndpointAddress(otherAddress);
                    var channelFactory = new DuplexChannelFactory<IChatServiceChannel>(sink, binding, endpoint);
                    var channel = channelFactory.CreateChannel();

                    var terminationSubscription = channel.TerminationSequence()
                                                         .Subscribe(_ => { }, o.OnError, o.OnCompleted);
                    channel.Open();
                    channel.Subscribe();
                    return new CompositeDisposable(subscription, terminationSubscription, channel.CreateSafeDisposal());
                });

        }

        private static void RecieveMessage(string msg)
        {
            using (ConsoleColorScope(ConsoleColor.DarkCyan))
            {
                Console.WriteLine("> {0}", msg);
            }
        }

        private static IDisposable ConsoleColorScope(ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            return Disposable.Create(() => Console.ForegroundColor = previousColor);
        }
    }

    public class ChatCallBack : ChatServiceReference.IChatServiceCallback
    {
        private readonly ISubject<string> _subject = new Subject<string>();

        void ChatServiceReference.IChatServiceCallback.MessageReceived(string message)
        {
            _subject.OnNext(message);
        }

        public IObservable<string> IncomingMessages
        {
            get { return _subject.AsObservable(); }
        }
    }
}
