namespace RecipeManagement.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    public DateTimeOffset CreatedOn { get; private set; }
    public bool IsDeleted { get; private set; }
    
    [NotMapped]
    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

    public void UpdateIsDeleted(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }
    
    public void QueueDomainEvent(DomainEvent @event)
    {
        if(!DomainEvents.Contains(@event))
            DomainEvents.Add(@event);
    }
}