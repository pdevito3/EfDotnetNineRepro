namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Percentages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    /// <summary>
    /// The database configuration for Authors. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        // Relationship Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding

        // Property Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding

        builder.Property(x => x.Ownership)
            .HasConversion(x => x.Value, x => new Percent(x));
        
        // example for a more complex value object
        // builder.OwnsOne(x => x.PhysicalAddress, opts =>
        // {
        //     opts.Property(x => x.Line1).HasColumnName("physical_address_line1");
        //     opts.Property(x => x.Line2).HasColumnName("physical_address_line2");
        //     opts.Property(x => x.City).HasColumnName("physical_address_city");
        //     opts.Property(x => x.State).HasColumnName("physical_address_state");
        //     opts.OwnsOne(x => x.PostalCode, o =>
        //         {
        //             o.Property(x => x.Value).HasColumnName("physical_address_postal_code");
        //         }).Navigation(x => x.PostalCode)
        //         .IsRequired();
        //     opts.Property(x => x.Country).HasColumnName("physical_address_country");
        // }).Navigation(x => x.PhysicalAddress)
        //     .IsRequired();
    }
}
