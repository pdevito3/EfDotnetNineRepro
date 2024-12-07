namespace RecipeManagement.Databases;

using RecipeManagement.Domain;
using RecipeManagement.Databases.EntityConfigurations;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Resources;
using MediatR;
using RecipeManagement.Domain.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

public sealed class RecipesDbContext(DbContextOptions<RecipesDbContext> options)
    : DbContext(options)
{
    #region DbSet Region - Do Not Delete
    public DbSet<Recipe> Recipes { get; set; }
    #endregion DbSet Region - Do Not Delete

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Entity Database Config Region - Only delete if you don't want to automatically add configurations
        modelBuilder.ApplyConfiguration(new RecipeConfiguration());
        #endregion Entity Database Config Region - Only delete if you don't want to automatically add configurations
    }

    public override int SaveChanges()
    {
        var result = base.SaveChanges();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}

public static class Extensions
{

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
    }
}
