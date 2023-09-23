using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Recipes.Client.Core.Messages;

internal class LanguageSelectedMessage 
    : ValueChangedMessage<string>
{
    public LanguageSelectedMessage(string value) 
        : base(value)
    {
    }
}
