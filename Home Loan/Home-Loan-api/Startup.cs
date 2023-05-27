using AutoMapper;
using BusinessLogic.AppServices;
using BusinessLogic.Mapper;
using DataAccess.DBContext;
using DataAccess.Entities;
using Home_Loan_api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Models;
using System;
using System.Text;
using Presentation.Mapper;
using System.IO;
using System.Reflection;
using DataAccess.Contracts;
using DataAccess.Repository;

namespace Home_Loan_api
{
    public class Startup
    {

        private MapperConfiguration _mapperConfiguration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
                cfg.AddProfile(new WebMappingProfile());
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin",
                    builder => builder.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });
            services.AddControllers();

            //Registering Logging
            Logging.NLog.ILogger logger = new Logging.NLog.Logger();
            services.AddSingleton<Logging.NLog.ILogger, Logging.NLog.Logger>();

            //Register Databse
            services.AddDbContext<DatabaseContext>(
                x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Register Repositories
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            //add identities
            services.AddIdentityCore<User>().AddEntityFrameworkStores<DatabaseContext>(); ;
            services.AddIdentityCore<Advisor>().AddEntityFrameworkStores<DatabaseContext>();

            //register services
            services.AddScoped<IAuthAppServices, AuthAppServices>();
            services.AddScoped<ILoanAppServices, LoanAppServices>();
            services.AddScoped<IPromotionAppServices, PromotionAppServices>();
            services.AddScoped<ICollateralAppService, CollateralAppService>();
            services.AddScoped<ILoanStateAppService, LoanStateAppService>();
            services.AddScoped<IStaticAppService, StaticAppService>();

            //adding mapper configurations
            services.AddSingleton<IMapper>(sp => _mapperConfiguration.CreateMapper());

            //adding roles
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();


            //
            services.AddAuthorization().AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            //configuring swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Home Loan Api",
                        Description = "Home Loan Api",
                        Version = "v1"
                    });
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "security token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type =ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IServiceProvider serviceProvider, IApplicationBuilder app, IWebHostEnvironment env, Logging.NLog.ILogger logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            RoleInitializer.InitializeAsync(serviceProvider).Wait();

            app.ConfigureExceptionHandler(logger);
            app.UseCors("AllowAllOrigin");
            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Home Loan Api");
            });
        }
    }
}
