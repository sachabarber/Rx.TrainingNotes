using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;
using PracticalRx.TodoList.Contracts;
using PracticalRx.TodoList.Desktop.Annotations;
using PracticalRx.TodoList.Desktop.Common;

namespace PracticalRx.TodoList.Desktop
{
    public class TodoListViewModel : INotifyPropertyChanged
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ObservableCollection<TodoViewModel> _items = new ObservableCollection<TodoViewModel>();
        private readonly ReadOnlyObservableCollection<TodoViewModel> _roitems;
        private readonly Dictionary<Guid, IDisposable> _itemUpdateSubscriptions = new Dictionary<Guid, IDisposable>();
        private string _newTitle;
        private ViewModelStatus _status;

        public TodoListViewModel(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
            _roitems = new ReadOnlyObservableCollection<TodoViewModel>(_items);
            Status = ViewModelStatus.Idle;
            this.PropertyChanges(vm => vm.NewTitle)
                .Where(val => !string.IsNullOrEmpty(val))
                .Subscribe(RequestAdd);
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

        public ViewModelStatus Status
        {
            get { return _status; }
            set
            {
                if (value == _status) return;
                _status = value;
                OnPropertyChanged();
            }
        }

        public ReadOnlyObservableCollection<TodoViewModel> Items { get { return _roitems; } }

        public IDisposable Load()
        {
            Status = ViewModelStatus.Processing;

            var query = from head in _todoRepository.HeadVersion().ToObservable().Trace("HeadVersion")
                        from update in _todoRepository.Updates.StartWith((TaskUpdate)null).Trace("Updates")
                        select new { head, update };

            return query.SubscribeOn(Scheduler.Default)
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(x =>
                           {
                               if (x.update == null)
                               {
                                   if (x.head == -1)
                                   {
                                       Status = ViewModelStatus.Idle;
                                   }
                                   return;
                               }
                               if (x.update.Version >= x.head)
                               {
                                   Status = ViewModelStatus.Idle;
                               }

                               if (x.update.AddedEvent != null)
                               {
                                   Insert(x.update.AddedEvent);
                               }
                               else if (x.update.UpdatedEvent != null)
                               {
                                   Update(x.update.UpdatedEvent);
                               }
                               else if (x.update.DeletedEvent != null)
                               {
                                   Remove(x.update.DeletedEvent.TaskId);
                               }
                           });
        }

        private void RequestAdd(string title)
        {
            Status = ViewModelStatus.Processing;
            _todoRepository.AddItem(new TodoViewModel(Guid.NewGuid()) { Title = title }).ToObservable()
                .SubscribeOn(Scheduler.Default)
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(
                    _ => { },
                    ex =>
                    {
                        Status = ViewModelStatus.Error(ex.Message);
                    },
                    () =>
                    {
                        NewTitle = string.Empty;
                        Status = ViewModelStatus.Idle;
                    });
        }

        private void RequestUpdate(TodoViewModel item)
        {
            Status = ViewModelStatus.Processing;
            _todoRepository.UpdateItem(item)
                .ToObservable()
                .SubscribeOn(Scheduler.Default)
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(
                    _ => { },
                    ex =>
                    {
                        Status = ViewModelStatus.Error(ex.Message);
                    },
                    () =>
                    {
                        Status = ViewModelStatus.Idle;
                    });
        }

        private void RequestDelete(TodoViewModel item)
        {
            Status = ViewModelStatus.Processing;
            _todoRepository.RemoveItem(item)
                .ToObservable()
                .SubscribeOn(Scheduler.Default)
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(
                    _ => { },
                    ex =>
                    {
                        Status = ViewModelStatus.Error(ex.Message);
                    },
                    () =>
                    {
                        NewTitle = string.Empty;
                        Status = ViewModelStatus.Idle;
                    });
        }

        private void Insert(TaskAddedEvent addition)
        {
            var newItem = new TodoViewModel(addition.TaskId) { Title = addition.Title };
            newItem.DeleteCommand = new DelegateCommand(() => RequestDelete(newItem));

            var updateSubscription = newItem
                .AnyPropertyChanges()
                
                .Where(item=>!item.IsUpdating)
                .Subscribe(RequestUpdate);
            _itemUpdateSubscriptions.Add(newItem.Id, updateSubscription);
            _items.Add(newItem);
        }

        private void Update(TaskUpdatedEvent update)
        {
            var item = _items.Single(i => i.Id == update.TaskId);
            item.Update(update.Title, update.IsCompleted);
        }

        private void Remove(Guid id)
        {
            var itemsToRemove = _items.Where(item => item.Id == id).ToArray();
            foreach (var itemToRemove in itemsToRemove)
            {
                _items.Remove(itemToRemove);
                _itemUpdateSubscriptions[id].Dispose();
                _itemUpdateSubscriptions.Remove(id);
            }
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