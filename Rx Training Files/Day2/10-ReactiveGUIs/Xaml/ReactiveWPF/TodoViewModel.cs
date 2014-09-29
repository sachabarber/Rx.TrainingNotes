using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using ReactiveWPF.Annotations;
using ReactiveWPF.Common;

namespace ReactiveWPF
{
    public class TodoViewModel : INotifyPropertyChanged
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ObservableCollection<TodoItemViewModel> _items = new ObservableCollection<TodoItemViewModel>(); 
        private readonly ReadOnlyObservableCollection<TodoItemViewModel> _roitems; 
        private string _newTitle;

        public TodoViewModel(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
            _roitems = new ReadOnlyObservableCollection<TodoItemViewModel>(_items);
            this.PropertyChanges(vm => vm.NewTitle)
                .Where(val=>!string.IsNullOrEmpty(val))
                .Subscribe(CreateNew);

        }

        public string NewTitle
        {
            get { return _newTitle; }
            set
            {
                if (value == _newTitle) return;
                _newTitle = value;
                OnPropertyChanged();
            }
        }


        public ReadOnlyObservableCollection<TodoItemViewModel> Items { get { return _roitems; } }

        public void Load()
        {
            _todoRepository.Updates
                .Subscribe(update =>
                           {
                               if (update.IsDeleted)
                               {
                                   Remove(update.Id);
                               }
                               else
                               {
                                   UpdateOrInsert(update);
                               }
                           });
        }


        private void CreateNew(string val)
        {
            Insert(Guid.NewGuid(), val, false);
            NewTitle = string.Empty;
        }

        private void Remove(Guid id)
        {
            var itemsToRemove = _items.Where(item => item.Id == id).ToArray();
            foreach (var itemToRemove in itemsToRemove)
            {
                _items.Remove(itemToRemove);
            }
        }

        private void UpdateOrInsert(TodoItemUpdate update)
        {
            var item = _items.FirstOrDefault(i => i.Id == update.Id);
            if (item != null)
            {
                item.Title = update.Title;
                item.IsCompleted = update.IsCompleted;
            }
            else
            {
                Insert(update.Id, update.Title, update.IsCompleted);
            }
        }

        private void Insert(Guid id, string val, bool isCompleted)
        {
            var newItem = new TodoItemViewModel(id) { Title = val, IsCompleted = isCompleted };
            newItem.DeleteCommand = new DelegateCommand(() => _todoRepository.RemoveItem(newItem));
            _items.Add(newItem);
        }

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