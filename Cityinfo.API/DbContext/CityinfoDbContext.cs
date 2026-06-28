using Microsoft.EntityFrameworkCore;
namespace Cityinfo.API.Entities
{
    public class CityinfoDbContex : DbContext
    {
        public CityinfoDbContex
            (DbContextOptions<CityinfoDbContex> options)
            : base(options)
        {

        }
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterests { get; set; } = null!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite();
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<City>()
                .HasData(
                new City("Tehran")
                {
                    Id = 1,
                    Description = "This is Trhran"
                },
                   new City("Shiraz")
                   {
                       Id = 2,
                       Description = "This is Shiraz"
                   },
                      new City("Tabriz")
                      {
                          Id = 3,
                          Description = "This is Tabriz"
                      }
                );
            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                new PointOfInterest("Academy Barnamenevisan")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "Tell 02188454816"
                },
                 new PointOfInterest("Shemiran")
                 {
                     Id = 2,
                     CityId = 1,
                     Description = "This is Shemiran"
                 },
                  new PointOfInterest("Meydan toopkhoone")
                  {
                      Id = 3,
                      CityId = 1,
                      Description = "This is toopkhoone"
                  }
                );

            base.OnModelCreating(modelBuilder);
        }


    }
}
