using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Configurations
{
    public class DrinkModificationRequestConfiguration : IEntityTypeConfiguration<DrinkModificationRequest>
    {
        public void Configure(EntityTypeBuilder<DrinkModificationRequest> builder)
        {
            builder.HasKey(x => x.DrinkModificationRequestId);

            builder.HasOne(x => x.OldDrink)
                   .WithMany()
                   .HasForeignKey(x => x.OldDrinkId)
                   .OnDelete(DeleteBehavior.Restrict); // avoid cascade deletes if not desired

            builder.HasOne(x => x.NewDrink)
                   .WithMany()
                   .HasForeignKey(x => x.NewDrinkId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RequestingUser)
                   .WithMany()
                   .HasForeignKey(x => x.RequestingUserId)
                   .OnDelete(DeleteBehavior.Cascade); // depends on your data model
        }
    }

}
