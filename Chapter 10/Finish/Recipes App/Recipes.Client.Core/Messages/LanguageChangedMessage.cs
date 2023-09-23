using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Recipes.Client.Core.Messages;

internal class LanguageChangedMessage : ValueChangedMessage<string>
{
    public LanguageChangedMessage(string value) : base(value)
    {
    }
}
