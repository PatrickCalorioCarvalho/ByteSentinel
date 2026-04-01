using ByteSentinelServer.Data;
using ByteSentinelServer.Services;
using Microsoft.EntityFrameworkCore;

namespace ByteSentinelServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped<MetricsService>();
            builder.Services.AddSignalR();
            builder.Services.AddControllers();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }

            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<AgentHub>("/hub/agents");

            app.Run();
        }
    }
}
