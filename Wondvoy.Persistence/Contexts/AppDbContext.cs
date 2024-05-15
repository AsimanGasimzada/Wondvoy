using Microsoft.EntityFrameworkCore;

namespace Wondvoy.Persistence.Contexts;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)   
    {
        
    }


}
