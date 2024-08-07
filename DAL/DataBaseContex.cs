using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcBuggetoEx.Models;

namespace MvcBuggetoEx.DAL
{
    public class DataBaseContex : IdentityDbContext<User,Role ,string>
    {
        public DataBaseContex(DbContextOptions<DataBaseContex> options):base(options) { }
        public DbSet<Blog> Blogs { get; set; }

    }
}
