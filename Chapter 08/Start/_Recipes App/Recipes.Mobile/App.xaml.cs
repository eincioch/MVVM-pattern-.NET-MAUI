using Recipes.Mobile.Navigation;

namespace Recipes.Mobile;

public partial class App : Application
{
    public App()
    {
        Application.Current.UserAppTheme = AppTheme.Light;
        InitializeComponent();

        //Using Shell
        MainPage = new AppShell(ServiceProvider
            .GetService<INavigationInterceptor>());

        //Not using Shell
        //MainPage = 
        //new NavigationPage();
        //ServiceProvider.Current.GetService<INavigationService>().GoToOverview();
    }
}
public class Label
{
    public string Text { get; set; }
}