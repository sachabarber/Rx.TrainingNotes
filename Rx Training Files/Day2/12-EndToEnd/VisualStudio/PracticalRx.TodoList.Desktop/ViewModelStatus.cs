using System;

namespace PracticalRx.TodoList.Desktop
{
    public abstract class ViewModelStatus
    {
        public static readonly ViewModelStatus Idle = new IdleViewModelStatus();
        public static readonly ViewModelStatus Processing = new ProcessingViewModelStatus();
        public static ViewModelStatus Error(string message)
        {
            return new ErrorViewModelStatus(message);
        }

        public virtual bool IsProcessing { get { return false; } }
        public virtual bool HasError { get { return false; } }
        public virtual string ErrorMessage { get { throw new InvalidOperationException(); } }

        private ViewModelStatus()
        { }

        private sealed class IdleViewModelStatus : ViewModelStatus
        { }

        private sealed class ProcessingViewModelStatus : ViewModelStatus
        {
            public override bool IsProcessing { get { return true; } }
        }

        private sealed class ErrorViewModelStatus : ViewModelStatus
        {
            private readonly string _errorMessage;

            public ErrorViewModelStatus(string message)
            {
                _errorMessage = message;
            }

            public override bool HasError { get { return true; } }
            public override string ErrorMessage { get { return _errorMessage; } }
        }
    }
}