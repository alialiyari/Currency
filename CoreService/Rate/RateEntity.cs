using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rate
{
    public class RateEntity
    {
        public long Id { get; set; }
        public string To { get; set; }
        public string From { get; set; }



        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }




        public double Rate { get; set; }
    }




    public class RateEntityConfiguration : IEntityTypeConfiguration<RateEntity>
    {
        public void Configure(EntityTypeBuilder<RateEntity> entity)
        {
            entity.HasKey(u => u.Id);
            entity.ToTable("Rates", "dbo");
        }
    }
}
partial class DatabaseContext
{
    public DbSet<Rate.RateEntity> Rates { get; set; }
}

