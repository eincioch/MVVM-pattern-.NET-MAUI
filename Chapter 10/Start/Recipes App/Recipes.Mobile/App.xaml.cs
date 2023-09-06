using Recipes.Mobile.Navigation;

namespace Recipes.Mobile;

public partial class App : Application
{
    public App()
    {
        Application.Current.UserAppTheme = AppTheme.Light;
        InitializeComponent();

        MainPage = new AppShell(ServiceProvider
            .GetService<INavigationInterceptor>());
    }
}
public class Label
{
    public string Text { get; set; }
}