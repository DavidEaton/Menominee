﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Menominee.Idp.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Menominee.Idp
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        private readonly string CorsPolicyProduction = "CorsPolicyProduction";
        private readonly string CorsPolicyDevelopment = "CorsPolicyDevelopment";
        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var builder = services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)

                // Configure IdentityServer to use AspNetCore Identity membership
                .AddAspNetIdentity<ApplicationUser>();

            if (Environment.IsDevelopment()) // not recommended for production - you need to store your key material somewhere secure
            {
                builder.AddDeveloperSigningCredential();
                services.AddTransient<IEmailSender, DummyEmailSender>();
            }

            ConfigureCors(services);
        }

        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyProduction,
                builder =>
                {
                    builder.WithOrigins(
                           "https://menominee.net",
                           "https://www.menominee.net",
                           "https://menominee.azurewebsites.net",
                           "https://menominee-testing.net",
                           "https://menominee-testing.azurewebsites.net")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });

                if (Environment.IsDevelopment())
                {
                    options.AddPolicy(CorsPolicyDevelopment,
                                      builder =>
                                      {
                                          builder.WithOrigins()
                                                 .AllowAnyOrigin()
                                                 .AllowAnyMethod()
                                                 .AllowAnyHeader();
                                      });
                }
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(CorsPolicyDevelopment);
            }
            else
            {
                app.UseCors(CorsPolicyProduction);
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}