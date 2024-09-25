﻿using System.Diagnostics.CodeAnalysis;
using Application;
using Application.Interfaces;
using Application.Services;
using Domain.Repositories;
using Domain.SeedWork.Notification;
using Infra.Data;
using Infra.Data.Repository;
using Infra.Http.AFMail;
using Infra.Security;
using Infra.Utils.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC
{
    [ExcludeFromCodeCoverage]
    public static class NativeInjector
    {
        public static void AddLocalHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var urlMail = configuration["Settings:AFMailUrl"];

            services.AddHttpClient<IMailService, MailService>(client => {
                client.BaseAddress = new Uri(urlMail!);
                client.Timeout = TimeSpan.FromSeconds(10);
            });
        }

        public static void AddLocalServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services
            services.AddScoped<INotification, Notification>();
            services.AddScoped<IPeopleService, PeopleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IContainer, ServiceProviderProxy>();
            services.AddScoped<IJwtService, JwtService>();
            #endregion

            #region Repository
            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion
        }

        public static void AddLocalUnitOfWork(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = Builders.BuildConnectionString(configuration);
            services.AddDbContext<Context>(options => options.UseLazyLoadingProxies().UseSqlServer(connString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddLocalHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = Builders.BuildConnectionString(configuration);
            var cacheConnString = configuration["App:Settings:Cache"]!;
            services.AddHealthChecks()
                //.AddSeqPublisher()
                .AddSqlServer(connString)
                .AddRedis(cacheConnString);
        }
    }
}
