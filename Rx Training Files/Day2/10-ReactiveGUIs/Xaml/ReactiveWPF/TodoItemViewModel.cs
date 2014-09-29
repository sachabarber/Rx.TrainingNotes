using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveWPF.Annotations;
using ReactiveWPF.Common;

namespace ReactiveWPF
{
    public class TodoItemViewModel : INotifyPropertyChanged
    {
        private readonly Guid _id;
        private bool _isCompleted;
        private string _title;

        public TodoItemViewModel(Guid id)
        {
            _id = id;
        }

        public Guid Id { get { return _id; } }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                if (value.Equals(_isCompleted)) return;
                _isCompleted = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand DeleteCommand { get; set; }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
