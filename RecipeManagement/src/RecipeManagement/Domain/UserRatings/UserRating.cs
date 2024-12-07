namespace RecipeManagement.Domain.UserRatings;

using FluentValidation;

public sealed class UserRating : ValueObject
{
    public int? Value { get; set; }
    
    public UserRating(int? value)
    {
        Value = value;
    }
    
    public static UserRating Of(int? value) => new UserRating(value);
    public static implicit operator string(UserRating value) => value.Value.ToString();

    private UserRating() { } // EF Core
    
    private sealed class UserRatingValidator : AbstractValidator<int?> 
    {
        public UserRatingValidator()
        {
        }
    }
}