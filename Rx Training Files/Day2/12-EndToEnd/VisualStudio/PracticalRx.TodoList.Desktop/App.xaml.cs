using System.Windows;
using PracticalRx.TodoList.Contracts;
using PracticalRx.TodoList.SignalRClient;

namespace PracticalRx.TodoList.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ITaskListServiceClient serviceClient = new TaskListSignalRServiceClient();
            var repo = new TodoRepository(serviceClient);
            var vm = new TodoListViewModel(repo);
            using (vm.Load())
            {
                var mainWindow = new MainWindow { DataContext = vm };
                MainWindow = mainWindow;
                mainWindow.ShowDialog();
            }
        }
    }
}
