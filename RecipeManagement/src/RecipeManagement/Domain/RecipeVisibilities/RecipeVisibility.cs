namespace RecipeManagement.Domain.RecipeVisibilities;

using Ardalis.SmartEnum;
using RecipeManagement.Exceptions;

public sealed class RecipeVisibility : ValueObject
{
    private RecipeVisibilityEnum _recipeVisibility;
    public string Value
    {
        get => _recipeVisibility.Name;
        private set
        {
            if (!RecipeVisibilityEnum.TryFromName(value, true, out var parsed))
                throw new ValidationException($"Invalid Recipe visibility. Please use one of the following: {string.Join(", ", ListNames())}");

            _recipeVisibility = parsed;
        }
    }
    
    public RecipeVisibility(string value)
    {
        Value = value;
    }

    public static RecipeVisibility Of(string value) => new RecipeVisibility(value);
    public static implicit operator string(RecipeVisibility value) => value.Value;
    public static List<string> ListNames() => RecipeVisibilityEnum.List.Select(x => x.Name).ToList();

   public static RecipeVisibility Public() => new RecipeVisibility(RecipeVisibilityEnum.Public.Name);
   public static RecipeVisibility FriendsOnly() => new RecipeVisibility(RecipeVisibilityEnum.FriendsOnly.Name);
   public static RecipeVisibility Private() => new RecipeVisibility(RecipeVisibilityEnum.Private.Name);

    private RecipeVisibility() { } // EF Core

    private abstract class RecipeVisibilityEnum(string name, int value)
        : SmartEnum<RecipeVisibilityEnum>(name, value)
    {
        public static readonly RecipeVisibilityEnum Public = new PublicType();
        public static readonly RecipeVisibilityEnum FriendsOnly = new FriendsOnlyType();
        public static readonly RecipeVisibilityEnum Private = new PrivateType();

        private class PublicType() : RecipeVisibilityEnum("Public", 0);

        private class FriendsOnlyType() : RecipeVisibilityEnum("Friends Only", 1);

        private class PrivateType() : RecipeVisibilityEnum("Private", 2);
    }
}