using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Recipes.Client.Core.ViewModels;

public class SettingsViewModel : ObservableObject//, INavigatedTo, INavigatedFrom
{
    //INavigationService _navigationService;

    private string currentLanguage = "Dutch";

    public string CurrentLanguage
    {
        get => currentLanguage;
        set => SetProperty(ref currentLanguage, value);
    }

    public AsyncRelayCommand SelectLanguageCommand { get; }

    //public SettingsViewModel(INavigationService service)
    public SettingsViewModel()
    {
        //_navigationService = service;
        SelectLanguageCommand = new AsyncRelayCommand(ChooseLanguage);
    }

    private async Task ChooseLanguage()
    {
        
    }

    //public Task OnNavigatedFrom(NavigationType navigationType)
    //{
    //    return Task.CompletedTask;
    //}

    //public Task OnNavigatedTo(NavigationType navigationType)
    //{
    //    return Task.CompletedTask;
    //}
}
