using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Recipes.Client.Core.Messages;
using Recipes.Client.Core.Navigation;

namespace Recipes.Client.Core.ViewModels;

public class PickLanguageViewModel : ObservableObject,
    INavigationParameterReceiver
{
    readonly INavigationService _navigationService;
    public AsyncRelayCommand OkCommand { get; }


    private string _selectedLanguage = "English";

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set => SetProperty(ref _selectedLanguage, value);
    }

    public List<string> Languages { get; set; } = new List<string>()
    {
        "Dutch",
        "French",
        "English",
        "Italian",
        "Spanish"
    };

    public PickLanguageViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        OkCommand = new AsyncRelayCommand(LanguagePicked);
    }

    private Task LanguagePicked()
    {
        WeakReferenceMessenger.Default
            .Send(new LanguageSelectedMessage(SelectedLanguage));
        return _navigationService.GoBack();
    }

    public Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        SelectedLanguage = parameters["language"] as string;
        return Task.CompletedTask;
    }
}
