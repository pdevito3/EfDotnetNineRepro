namespace RecipeManagement.Domain.Recipes;

using System.ComponentModel.DataAnnotations;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Recipes.Models;


public class Recipe : BaseEntity
{
    public string Title { get; private set; }

    public string Directions { get; private set; }

    public DateOnly? DateOfOrigin { get; private set; }

    public bool HaveMadeItMyself { get; private set; }

    public string[] Tags { get; private set; } = Array.Empty<string>();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Recipe Create(RecipeForCreation recipeForCreation)
    {
        var newRecipe = new Recipe();

        newRecipe.Title = recipeForCreation.Title;
        newRecipe.Directions = recipeForCreation.Directions;
        newRecipe.DateOfOrigin = recipeForCreation.DateOfOrigin;
        newRecipe.HaveMadeItMyself = recipeForCreation.HaveMadeItMyself;
        newRecipe.SetTags(recipeForCreation.Tags);
        
        return newRecipe;
    }

    public Recipe Update(RecipeForUpdate recipeForUpdate)
    {
        Title = recipeForUpdate.Title;
        Directions = recipeForUpdate.Directions;
        DateOfOrigin = recipeForUpdate.DateOfOrigin;
        HaveMadeItMyself = recipeForUpdate.HaveMadeItMyself;
        SetTags(recipeForUpdate.Tags);

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
