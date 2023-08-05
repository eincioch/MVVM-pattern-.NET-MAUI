using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Recipes.Client.Core.Navigation;
using Recipes.Client.Core.Recipes;
using Recipes.Client.Core.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Recipes.Client.Core.ViewModels;




public class ValidationErrorExposer : INotifyPropertyChanged, IDisposable
{
    readonly ObservableValidator _validator;

    public event PropertyChangedEventHandler? PropertyChanged;

    private Dictionary<string, ObservableCollection<ValidationResult>> _validationResults;

    public ValidationErrorExposer(ObservableValidator observableValidator)
    {
        _validationResults = new();
        _validator = observableValidator;
        _validator.ErrorsChanged += ObservableValidator_ErrorsChanged;
    }

    private void ObservableValidator_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        var property = e.PropertyName;
        if (_validationResults.ContainsKey(property))
        {
            _validationResults[property].Clear();
            _validator.GetErrors(property).ToList()
                .ForEach(_validationResults[property].Add);
        }
        else
        {
            _validationResults.Add(property,
                new(_validator.GetErrors(property)));
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
    }

    public void Dispose()
    {
        _validator.ErrorsChanged += ObservableValidator_ErrorsChanged;
    }

    public ObservableCollection<ValidationResult> this[string property] => _validationResults[property];
}

public class AddRatingViewModel : ObservableValidator, INavigationParameterReceiver, INavigatedFrom
{

    readonly INavigationService _navigationService;
    public const string EmailValidationRegex = @"^[a-z0-9]+@[a-z]+\.[a-z]{2,3}$";
    public const string RangeDecimalRegex = @"^\d+(\.\d{1,1})?$";
    public const int DisplayNameMinLength = 5;
    public const int DisplayNameMaxLength = 25;
    public const double RatingMinVal = 0d;
    public const double RatingMaxVal = 4d;

    string _recipeTitle = string.Empty;
    public string RecipeTitle
    {
        get => _recipeTitle;
        private set => SetProperty(ref _recipeTitle, value);
    }

    string _emailAddress;
    [Required]
    [RegularExpression(EmailValidationRegex)]
    public string EmailAddress
    {
        get => _emailAddress;
        set => SetProperty(ref _emailAddress, value, true);
    }

    string _displayName;

    [Required]
    [MinLength(DisplayNameMinLength)]
    [MaxLength(DisplayNameMaxLength)]
    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value, true);
    }

    string _ratingInput;

    [Required]
    [RegularExpression(RangeDecimalRegex)]
    [Range(RatingMinVal, RatingMaxVal)]
    public string RatingInput
    {
        get => _ratingInput;
        set
        {
            SetProperty(ref _ratingInput, value, true);
         //   ValidateProperty(Review, nameof(Review));
        }
    }

    string _review;

    //[CustomValidation(typeof(AddRatingViewModel), nameof(ValidateReview))]
    [EmptyOrHavingMinMaxLength(
        MinLength = 2, MaxLength = 10)]
    public string Review
    {
        get => _review;
        set => SetProperty(ref _review, value, true);
    }
    public ValidationErrorExposer ErrorExposer { get; }


    public RelayCommand GoBackCommand { get; }

    public RelayCommand SubmitCommand { get; }

    public AddRatingViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        GoBackCommand = new RelayCommand(() => _navigationService.GoBack());
        SubmitCommand = new RelayCommand(OnSubmit, () => !HasErrors);
        Errors = new ();
        ErrorExposer = new (this);
        ErrorsChanged += AddRatingViewModel_ErrorsChanged;
        ResetInputFields();
    }

    private void AddRatingViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        Errors.Clear();
        GetErrors().ToList().ForEach(Errors.Add);
        Errors.ToList().ForEach(e => Console.WriteLine(e.ErrorMessage));
        SubmitCommand.NotifyCanExecuteChanged();
    }

    //private bool IsValid()
    //    => !HasErrors;

    private void ResetInputFields()
    {
        EmailAddress = "";
        DisplayName = "";
        RatingInput = "";
        Review = "";
    }

    public ObservableCollection<ValidationResult> Errors { get; }

    private void OnSubmit()
    {
        //ValidateAllProperties();
        ValidateProperty(EmailAddress, nameof(EmailAddress));
        ValidateProperty(DisplayName, nameof(DisplayName));
        ValidateProperty(DisplayName, nameof(DisplayName));

        if(HasErrors)
        {
            var errors = GetErrors();
            Debug.WriteLine(
                string.Join("\r\n",
                errors.Select(e => e.ErrorMessage)));
        }
        else
        {
            Debug.WriteLine("All OK");
        }

        //var x = this.HasErrors;
        //var y = this.GetErrors(nameof(EmailAddress));
        ////throw new NotImplementedException();
    }

    private async Task LoadData(RecipeDetailDto recipe)
    {
        RecipeTitle = recipe.Name;
    }

    public Task OnNavigatedTo(Dictionary<string, object> parameters)
    => LoadData(parameters["recipe"]
        as RecipeDetailDto);

    public Task OnNavigatedFrom(NavigationType navigationType)
    {
        if (navigationType == NavigationType.Back)
        {
            ErrorsChanged -= AddRatingViewModel_ErrorsChanged;
            ErrorExposer.Dispose();
            ResetInputFields();
        }
        return Task.CompletedTask;
    }

    public static ValidationResult ValidateReview(string review, ValidationContext context)
    {
        AddRatingViewModel instance = (AddRatingViewModel)context.ObjectInstance;

        if (double.TryParse(instance.RatingInput, out var rating))
        {
            if (rating < 2 && string.IsNullOrEmpty(review))
            {
                return new("A review as mandatory when rating the recipe 2 or less.");
            }
        }

        return ValidationResult.Success;
    }
}



//<Entry Text = "{Binding EmailAddress, Mode=TwoWay}" />
//    < Label Text="Display name:" />
//    <Entry Text = "{Binding DisplayName, Mode=TwoWay}" />
//    < Label Text="Rating (1-5):" />
//    <Entry Text = "{Binding RatingInput, Mode=TwoWay}" />
//    < Label Text="Review (optional):" />
//    <Entry Text = "{Binding Review, Mode=TwoWay}" />
