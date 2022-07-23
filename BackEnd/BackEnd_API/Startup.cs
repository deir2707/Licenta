using BackEnd.Hubs;
using BackEnd.Middleware;
using Domain;
using Infrastructure;
using Infrastructure.Mongo;
using Infrastructure.Notifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Repository;
using Service;

namespace BackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));

            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddControllers();
            services.AddSignalR();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "BackEnd_API", Version = "v1"}); 
                c.OperationFilter<CustomHeaderSwaggerAttribute>();
            });
            
            services.AddDbContext<AuctionContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuctionService, AuctionService>();
            services.AddTransient<IRepository<User>,Repository<User>>();
            services.AddTransient<IRepository<Auction>,Repository<Auction>>();

            services.AddSingleton<INotificationPublisher, AuctionHub>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

            services.AddHostedService<AuctionFinishService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackEnd_API v1"));
            }
            
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseMiddleware<UserValidationMiddleware>();
            
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(builder =>
                builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
                endpoints.MapHub<AuctionHub>("/hub/auctionHub");
            });
        }
    }
}