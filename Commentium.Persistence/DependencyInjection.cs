﻿using Commentium.Application.Abstractions.Caching;
using Commentium.Application.Abstractions.EventBus;
using Commentium.Domain.Comments;
using Commentium.Domain.Users;
using Commentium.Persistence.Caching;
using Commentium.Persistence.MessageBroker;
using Commentium.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Commentium.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration["DefaultConnection"]));

            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<ICacheService, CacheService>();

            services.AddTransient<IEventBus, EventBus>();

            return services;
        }
    }
}
