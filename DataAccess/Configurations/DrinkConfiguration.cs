﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class DrinkConfiguration : IEntityTypeConfiguration<Drink>
    {
        public void Configure(EntityTypeBuilder<Drink> builder)
        {
            builder.HasKey(drink => drink.DrinkId);

            builder.Property(drink => drink.DrinkId)
                   .ValueGeneratedOnAdd();

            builder.Property(drink => drink.DrinkName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(drink => drink.IsRequestingApproval)
                   .HasDefaultValue(false);

            builder.HasOne(drink => drink.Brand)
                   .WithMany()
                   .HasForeignKey(drink => drink.BrandId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
