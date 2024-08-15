using JobPortalApplication.Models;
using JobPortalApplication.Models.CompanyModel;
using JobPortalApplication.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace JobPortalApplication.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<UserModel> usermodel { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<State> state { get; set; }
        public DbSet<City> city { get; set; }
        public DbSet<Course> course { get; set; }
        public DbSet<Qualification> qualification { get; set; }
        public DbSet<Industry> industry { get; set; }
        public DbSet<Userdata> userdata { get; set; }
        public DbSet<Company> company { get; set; }
        public DbSet<Job> job { get; set; }
        public DbSet<JobApplication> jobapplication { get; set; }
        public DbSet<Interview> interview { get; set; }




    }
}
