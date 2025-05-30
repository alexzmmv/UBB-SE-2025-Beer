using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasKey(vote => vote.VoteId);

            builder.HasOne(vote => vote.User)
                   .WithMany()
                   .HasForeignKey(vote => vote.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vote => vote.Drink)
                   .WithMany()
                   .HasForeignKey(vote => vote.DrinkId)
                   .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
