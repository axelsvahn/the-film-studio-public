using Filmstudion.Data;
using Filmstudion.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using System.Reflection;

namespace Filmstudion
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

            services.AddIdentity<User, IdentityRole>(cfg => //IdentityRole för säkerhets skull
            {
                cfg.User.RequireUniqueEmail = true;
            })
       .AddEntityFrameworkStores<AppDbContext>(); //vår identity data lagras i appdbcontext

            services.AddAuthentication()
              .AddCookie() // tar med för säkerhets skull
              .AddJwtBearer(cfg =>
              {
                  cfg.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidIssuer = Configuration["Tokens:Issuer"],
                      ValidAudience = Configuration["Tokens:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                  };
              });


            //inMemory database
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("filmstudion"));

            //classes
            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddScoped<IStudioRepository, StudioRepository>();
            services.AddScoped<IRentedFilmRepository, RentedFilmRepository>();
            services.AddScoped<IApiUserRepository, ApiUserRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly()); 

            services.AddControllers();

            services.AddMvc();

            //swagger
            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles(); //makes it go to index.html automatically
            app.UseStaticFiles(); //for the client interface in wwwroot


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
