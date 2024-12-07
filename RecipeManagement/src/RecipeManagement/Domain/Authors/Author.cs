namespace RecipeManagement.Domain.Authors;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Recipes;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Authors.Models;
using RecipeManagement.Domain.Authors.DomainEvents;
using RecipeManagement.Domain.Percentages;


public class Author : BaseEntity
{
    [LogMasked]
    public string Name { get; private set; }

   public Percent Ownership { get; private set; }

    public string PrimaryEmail { get; private set; }

    public IReadOnlyCollection<Recipe> Recipes { get; } = new List<Recipe>();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Author Create(AuthorForCreation authorForCreation)
    {
        var newAuthor = new Author();

        newAuthor.Name = authorForCreation.Name;
        newAuthor.Ownership = Percent.Of(authorForCreation.Ownership);
        newAuthor.PrimaryEmail = authorForCreation.PrimaryEmail;

        newAuthor.QueueDomainEvent(new AuthorCreated(){ Author = newAuthor });
        
        return newAuthor;
    }

    public Author Update(AuthorForUpdate authorForUpdate)
    {
        Name = authorForUpdate.Name;
        Ownership = Percent.Of(authorForUpdate.Ownership);
        PrimaryEmail = authorForUpdate.PrimaryEmail;

        QueueDomainEvent(new AuthorUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Author() { } // For EF + Mocking
}
