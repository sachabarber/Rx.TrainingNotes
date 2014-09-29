using System.Windows;

namespace ReactiveWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var repo = new StubTodoRepository())
            {
                var vm = new TodoViewModel(repo);
                vm.Load();


                var mainWindow = new MainWindow { DataContext = vm };
                MainWindow = mainWindow;
                mainWindow.ShowDialog();
            }
        }
    }
}
