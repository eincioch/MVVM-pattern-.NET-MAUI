using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Recipes.Client.Core.ViewModels;

public class PickLanguageViewModel : ObservableObject
{
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

    public PickLanguageViewModel()
    {
        OkCommand = new AsyncRelayCommand(LanguagePicked);
    }

    private Task LanguagePicked()
    {
        return Task.CompletedTask;
    }

    public Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        SelectedLanguage = parameters["language"] as string;
        return Task.CompletedTask;
    }
}
