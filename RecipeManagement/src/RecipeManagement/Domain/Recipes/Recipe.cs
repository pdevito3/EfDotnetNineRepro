namespace RecipeManagement.Domain.Recipes;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Recipes.Models;


public class Recipe : BaseEntity
{
    public string Title { get; private set; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Recipe Create(RecipeForCreation recipeForCreation)
    {
        var newRecipe = new Recipe();

        newRecipe.Title = recipeForCreation.Title;

        
        return newRecipe;
    }
    
    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Recipe() { } // For EF + Mocking
}
