namespace RecipeManagement.Domain.Recipes.Features;

using Hangfire;
using HeimGuard;
using RecipeManagement.Resources.HangfireUtilities;
using RecipeManagement.Services;
using RecipeManagement.Databases;

public class PerformFakeBookMigration(RecipesDbContext dbContext)
{    
    public sealed class Command : IJobWithUserContext
    {
        public string User { get; set; }
    }

    [JobDisplayName("Perform Fake Book Migration")]
    [AutomaticRetry(Attempts = 1)]
    // [Queue(Consts.HangfireQueues.PerformFakeBookMigration)]
    [CurrentUserFilter]
    public async Task Handle(Command command, CancellationToken cancellationToken)
    {
        // TODO some work here
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}