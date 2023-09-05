namespace Recipes.Client.Repositories;

public class RepositorySettings
{
    public string ApiAddress { get; }

    public RepositorySettings(string apiAddress)
    {
        ApiAddress = apiAddress;
    }
}
