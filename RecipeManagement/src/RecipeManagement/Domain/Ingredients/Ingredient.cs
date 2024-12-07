namespace RecipeManagement.Domain.Ingredients;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Recipes;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Ingredients.Models;
using RecipeManagement.Domain.Ingredients.DomainEvents;
using RecipeManagement.Domain.MonetaryAmounts;


public class Ingredient : BaseEntity
{
    public string Name { get; private set; }

    public string Quantity { get; private set; }

    public DateTime? ExpiresOn { get; private set; }

    public DateTimeOffset? BestTimeOfDay { get; private set; }

    public string Measure { get; private set; }

   public MonetaryAmount AverageCost { get; private set; }

    public Recipe Recipe { get; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Ingredient Create(IngredientForCreation ingredientForCreation)
    {
        var newIngredient = new Ingredient();

        newIngredient.Name = ingredientForCreation.Name;
        newIngredient.Quantity = ingredientForCreation.Quantity;
        newIngredient.ExpiresOn = ingredientForCreation.ExpiresOn;
        newIngredient.BestTimeOfDay = ingredientForCreation.BestTimeOfDay;
        newIngredient.Measure = ingredientForCreation.Measure;
        newIngredient.AverageCost = MonetaryAmount.Of(ingredientForCreation.AverageCost);

        newIngredient.QueueDomainEvent(new IngredientCreated(){ Ingredient = newIngredient });
        
        return newIngredient;
    }

    public Ingredient Update(IngredientForUpdate ingredientForUpdate)
    {
        Name = ingredientForUpdate.Name;
        Quantity = ingredientForUpdate.Quantity;
        ExpiresOn = ingredientForUpdate.ExpiresOn;
        BestTimeOfDay = ingredientForUpdate.BestTimeOfDay;
        Measure = ingredientForUpdate.Measure;
        AverageCost = MonetaryAmount.Of(ingredientForUpdate.AverageCost);

        QueueDomainEvent(new IngredientUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Ingredient() { } // For EF + Mocking
}
