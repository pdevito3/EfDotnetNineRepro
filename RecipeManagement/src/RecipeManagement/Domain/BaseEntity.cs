namespace RecipeManagement.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();
}