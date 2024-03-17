
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using S1ASPNETBase.Abstraction;
using S1ASPNETBase.Models;
using S1ASPNETBase.Repo;

namespace S1ASPNETBase
{
    public class Program
    {
        public static WebApplication AppBuilding(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //.AddJsonOptions(x =>
            //x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            //Реализация Autofac. Не нужна т.к. есть встроенный функционал Singleton
            //builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            //builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
            //cb.RegisterTypes([typeof(ProductRepository), typeof(CategoryRepository)])
            //.As([typeof(IProductRepository), typeof(ICategoryRepository)])
            //);

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddMemoryCache(o => o.TrackStatistics = true);

            string? connectionString = builder.Configuration.GetConnectionString("db");
            builder.Services.AddDbContext<MarketModelsDtContext>(options => options.UseNpgsql(connectionString));

            return builder.Build();
        }
        public static void Main(string[] args)
        {
            var app = AppBuilding(args);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //для теста создаем путь по директории где будем хранить файлы
            var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
            Directory.CreateDirectory(staticFilesPath);

            app.UseStaticFiles(
                new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(staticFilesPath),
                    RequestPath = "/static"
                });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }
    }
}
