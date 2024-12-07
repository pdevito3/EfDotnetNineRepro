namespace SharedKernel.Messages
{
    using System;
    using System.Text;

    public interface IImportRecipe
    {
        public Guid RecipeId { get; set; }
    }

    public class ImportRecipe : IImportRecipe
    {
        public Guid RecipeId { get; set; }
    }
}