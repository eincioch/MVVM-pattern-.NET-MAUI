using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Recipes.Client.Core.Messages;
using Recipes.Client.Core.Navigation;

namespace Recipes.Client.Core.ViewModels;

public class SettingsViewModel : ObservableObject, INavigatedTo, INavigatedFrom
{
    INavigationService _navigationService;

    private string currentLanguage = "Dutch";

    public string CurrentLanguage
    {
        get => currentLanguage;
        set => SetProperty(ref currentLanguage, value);
    }

    public AsyncRelayCommand SelectLanguageCommand { get; }

    public SettingsViewModel(INavigationService service)
    {
        _navigationService = service;
        SelectLanguageCommand = new AsyncRelayCommand(ChooseLanguage);
    }

    private async Task ChooseLanguage()
    {
        WeakReferenceMessenger.Default
          .Register<LanguageChangedMessage>(this,
          (r, m) => ((SettingsViewModel)r)
          .CurrentLanguage = m.Value);

        await _navigationService.GoToChooseLanguage(CurrentLanguage);
    }

    public Task OnNavigatedFrom(NavigationType navigationType)
    {
        return Task.CompletedTask;
    }

    public Task OnNavigatedTo(NavigationType navigationType)
    {
        WeakReferenceMessenger.Default
          .Unregister<LanguageChangedMessage>(this);
        return Task.CompletedTask;
    }
}
