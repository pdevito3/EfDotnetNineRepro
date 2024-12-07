namespace RecipeManagement.Domain.Recipes;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Authors;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Recipes.Models;
using RecipeManagement.Domain.Recipes.DomainEvents;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Models;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Models;
using RecipeManagement.Domain.RecipeVisibilities;
using RecipeManagement.Domain.UserRatings;


public class Recipe : BaseEntity
{
    public string Title { get; private set; }

   public RecipeVisibility Visibility { get; private set; }

    public string Directions { get; private set; }

   public UserRating Rating { get; private set; }

    public DateOnly? DateOfOrigin { get; private set; }

    public bool HaveMadeItMyself { get; private set; }

    public string[] Tags { get; private set; } = Array.Empty<string>();

    public Author Author { get; private set; }

    private readonly List<Ingredient> _ingredients = new();
    public IReadOnlyCollection<Ingredient> Ingredients => _ingredients.AsReadOnly();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Recipe Create(RecipeForCreation recipeForCreation)
    {
        var newRecipe = new Recipe();

        newRecipe.Title = recipeForCreation.Title;
        newRecipe.Visibility = RecipeVisibility.Of(recipeForCreation.Visibility);
        newRecipe.Directions = recipeForCreation.Directions;
        newRecipe.Rating = UserRating.Of(recipeForCreation.Rating);
        newRecipe.DateOfOrigin = recipeForCreation.DateOfOrigin;
        newRecipe.HaveMadeItMyself = recipeForCreation.HaveMadeItMyself;
        newRecipe.SetTags(recipeForCreation.Tags);

        newRecipe.QueueDomainEvent(new RecipeCreated(){ Recipe = newRecipe });
        
        return newRecipe;
    }

    public Recipe Update(RecipeForUpdate recipeForUpdate)
    {
        Title = recipeForUpdate.Title;
        Visibility = RecipeVisibility.Of(recipeForUpdate.Visibility);
        Directions = recipeForUpdate.Directions;
        Rating = UserRating.Of(recipeForUpdate.Rating);
        DateOfOrigin = recipeForUpdate.DateOfOrigin;
        HaveMadeItMyself = recipeForUpdate.HaveMadeItMyself;
        SetTags(recipeForUpdate.Tags);

        QueueDomainEvent(new RecipeUpdated(){ Id = Id });
        return this;
    }

    public Recipe SetAuthor(Author author)
    {
        Author = author;
        return this;
    }

    public Recipe AddIngredient(Ingredient ingredient)
    {
        _ingredients.Add(ingredient);
        return this;
    }
    
    public Recipe RemoveIngredient(Ingredient ingredient)
    {
        _ingredients.RemoveAll(x => x.Id == ingredient.Id);
        return this;
    }

    public Recipe AddTag(string tag)
    {
        Tags ??= Array.Empty<string>();
        Tags = Tags.Append(tag).ToArray();
        return this;
    }

    public Recipe RemoveTag(string tag)
    {
        Tags ??= Array.Empty<string>();
        Tags = Tags.Where(x => x != tag).ToArray();
        return this;
    }

    public Recipe SetTags(string[] tags)
    {
        Tags = tags;
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Recipe() { } // For EF + Mocking
}
