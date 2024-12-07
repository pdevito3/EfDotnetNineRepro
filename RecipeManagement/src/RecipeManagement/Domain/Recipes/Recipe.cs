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

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Recipe Create(RecipeForCreation recipeForCreation)
    {
        var newRecipe = new Recipe();

        newRecipe.Title = recipeForCreation.Title;
        newRecipe.Directions = recipeForCreation.Directions;
        newRecipe.DateOfOrigin = recipeForCreation.DateOfOrigin;
        newRecipe.HaveMadeItMyself = recipeForCreation.HaveMadeItMyself;

        
        return newRecipe;
    }

    public Recipe Update(RecipeForUpdate recipeForUpdate)
    {
        Title = recipeForUpdate.Title;
        Directions = recipeForUpdate.Directions;
        DateOfOrigin = recipeForUpdate.DateOfOrigin;
        HaveMadeItMyself = recipeForUpdate.HaveMadeItMyself;


        return this;
    }
    
    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Recipe() { } // For EF + Mocking
}
