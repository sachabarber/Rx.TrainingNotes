using System.ServiceModel;

namespace DuplexWcfChat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatService : IChatService
    {
        private IChatServiceCallback _callback;

        public void Subscribe()
        {
            Send("Connected");
        }

        public void Send(string message)
        {
            if (_callback == null && OperationContext.Current == null)
                return;
            if(_callback==null)
                _callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
            if (_callback != null)
                _callback.MessageReceived(message);
        }
    }

    [ServiceContract(CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void Subscribe();
    }

    public interface IChatServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void MessageReceived(string message);
    }
}
