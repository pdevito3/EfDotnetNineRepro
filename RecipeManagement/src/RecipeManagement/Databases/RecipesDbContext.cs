namespace RecipeManagement.Databases;

using RecipeManagement.Domain;
using RecipeManagement.Databases.EntityConfigurations;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Resources;
using MediatR;
using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Ingredients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

public sealed class RecipesDbContext(DbContextOptions<RecipesDbContext> options, 
    ICurrentUserService currentUserService, 
    IMediator mediator, 
    TimeProvider dateTimeProvider)
    : DbContext(options)
{
    #region DbSet Region - Do Not Delete
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    #endregion DbSet Region - Do Not Delete

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.FilterSoftDeletedRecords();
        /* any query filters added after this will override soft delete 
                https://docs.microsoft.com/en-us/ef/core/querying/filters
                https://github.com/dotnet/efcore/issues/10275
        */

        #region Entity Database Config Region - Only delete if you don't want to automatically add configurations
        modelBuilder.ApplyConfiguration(new RecipeConfiguration());
        modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        modelBuilder.ApplyConfiguration(new IngredientConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
        #endregion Entity Database Config Region - Only delete if you don't want to automatically add configurations
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        var result = base.SaveChanges();
        _dispatchDomainEvents().GetAwaiter().GetResult();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateAuditFields();
        var result = await base.SaveChangesAsync(cancellationToken);
        await _dispatchDomainEvents();
        return result;
    }
    
    private async Task _dispatchDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<BaseEntity>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var entityDomainEvent in events)
                await mediator.Publish(entityDomainEvent);
        }
    }
        
    private void UpdateAuditFields()
    {
        var now = dateTimeProvider.GetUtcNow();
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.UpdateCreationProperties(now, currentUserService?.UserId);
                    entry.Entity.UpdateModifiedProperties(now, currentUserService?.UserId);
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateModifiedProperties(now, currentUserService?.UserId);
                    break;
                
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.UpdateModifiedProperties(now, currentUserService?.UserId);
                    entry.Entity.UpdateIsDeleted(true);
                    break;
            }
        }
    }
}

public static class Extensions
{
public static void FilterSoftDeletedRecords(this ModelBuilder modelBuilder)
{
    Expression<Func<BaseEntity, bool>> filterExpr = e => !e.IsDeleted;
    foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes()
        .Where(m => m.ClrType.IsAssignableTo(typeof(BaseEntity))))
    {
        // modify expression to handle correct child type
        var parameter = Expression.Parameter(mutableEntityType.ClrType);
        var body = ReplacingExpressionVisitor
            .Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
        var lambdaExpression = Expression.Lambda(body, parameter);

        // set filter
        mutableEntityType.SetQueryFilter(lambdaExpression);
    }
}

    public static async Task<TEntity> GetByIdOrDefault<TEntity>(this DbSet<TEntity> dbSet, 
        Guid id, 
        CancellationToken cancellationToken = default) 
            where TEntity : BaseEntity
    {
        return await dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    
    public static async Task<TEntity> GetByIdOrDefault<TEntity>(this IQueryable<TEntity> query, 
        Guid id, 
        CancellationToken cancellationToken = default) 
            where TEntity : BaseEntity
    {
        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    } 
    
    public static async Task<TEntity> GetById<TEntity>(this DbSet<TEntity> dbSet, 
        Guid id, 
        CancellationToken cancellationToken = default) 
            where TEntity : BaseEntity
    {
        var result = await dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    
        return result.MustBeFoundOrThrow();
    }
    
    public static async Task<TEntity> GetById<TEntity>(this IQueryable<TEntity> query, 
        Guid id, 
        CancellationToken cancellationToken = default) 
            where TEntity : BaseEntity
    {
        var result = await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    
        return result.MustBeFoundOrThrow();
    }

    public static TEntity MustBeFoundOrThrow<TEntity>(this TEntity entity)
        where TEntity : BaseEntity
    {
         return entity ?? throw new NotFoundException($"{typeof(TEntity).Name} was not found.");
    }public static IQueryable<User> GetUserAggregate(this RecipesDbContext dbContext)
{
    return dbContext.Users
        .Include(u => u.Roles);
}
}
