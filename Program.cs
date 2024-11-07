
using EcommercePetsFoodBackend.customMiddleware;
using EcommercePetsFoodBackend.Db_Context;
using EcommercePetsFoodBackend.Mapper;
using EcommercePetsFoodBackend.Services.CustomerServices;
using EcommercePetsFoodBackend.Services.CustomerServices.serviceProduct;
using EcommercePetsFoodBackend.Services.ServiceCart;
using EcommercePetsFoodBackend.Services.WishlistServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EcommercePetsFoodBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var key = jwtSettings["Key"];
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EcommercePetsFoodBackend", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });


            builder.Services.AddAutoMapper(typeof(MappPets));
            builder.Services.AddScoped<ICustomer, Customer>();
            builder.Services.AddScoped<IProductServices, ProductService>();
            builder.Services.AddScoped<IwishlistServices, WishlistServices>();
            builder.Services.AddScoped<ICartServices,CartServices>();
            builder.Services.AddDbContext<EcomContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("EcommerceConctn")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseDeveloperExceptionPage();


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserIdMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
