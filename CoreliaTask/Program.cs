using BL;
using BL.IRepository;
using BL.Mapper;
using CoreliaTask.Data;
using CoreliaTask.SignalR;
using DAL;
using DAL.Repository;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace CoreliaTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //HangFire
            //builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString(name: "Con1")));
            //builder.Services.AddHangfireServer();

            builder.Services.AddSignalR();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration
                .GetConnectionString(name: "Con1"), b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
            builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            }));
            builder.Services.AddTransient<IUnitOfWork,UnitOfWork>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("corsapp");

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            //app.UseHangfireDashboard(pathMatch:"/dashboard") ;
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotifyHub>("/notificationHub");
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapControllers();

            app.Run();
        }
    }
}