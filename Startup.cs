using EquipmentChecklistDataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChecklistAPI.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ChecklistAPI.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;

namespace ChecklistAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            Configuration = configuration;
            this.env = environment;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddDefaultPolicy(builder => {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers();


            //add db context service
            services.AddDbContext<EquipmentChecklistDBContext>(options => {
                var _conStr = "";
                if (env.IsDevelopment()) _conStr = "DevConnection";
                if (env.IsStaging()) _conStr = "StgConnection";
                if (env.IsProduction()) _conStr = "PrdConnection";

                var _builder = new SqlConnectionStringBuilder
                {
                    ConnectionString = Configuration.GetConnectionString(_conStr)
                };

                var _credentials = Configuration.GetSection("Credentials"); //turned into variable to avoid mistyping
                _builder.UserID = _credentials["DbUser"];
                _builder.Password = _credentials["DbPassword"];

                options.UseSqlServer(_builder.ConnectionString); } //change to "PrdConnection" when using efcore 'update-database' to prod
            );

            services.AddScoped<IUserService, UserService>();

            //configuration for settings appsettings as a strongly - typed object
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configuration to get values of appsettings
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
