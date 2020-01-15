using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zynapse;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        private static readonly Type[] HandlerTypes =
        {
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>),
        };

        public static IServiceCollection AddZynapse<T>(this IServiceCollection services)
        {
            services.TryAddScoped<ICommandDispatcher, CommandDispatcher>();
            services.TryAddScoped<IQueryExecutor, QueryExecutor>();
            services.AddHandlers(typeof(T).Assembly.DefinedTypes);
            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                foreach (var handlerType in HandlerTypes)
                {
                    if (type.IsAssignableTo(handlerType, out var arguments))
                    {
                        var serviceType = handlerType.MakeGenericType(arguments);

                        var descriptor = new ServiceDescriptor(serviceType, type, ServiceLifetime.Scoped);

                        services.TryAdd(descriptor);
                    }
                }
            }

            return services;
        }

        private static bool IsAssignableTo(this Type type, Type genericTypeDefinition, out Type[] genericArguments)
        {
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType)
                {
                    if (genericTypeDefinition == @interface.GetGenericTypeDefinition())
                    {
                        genericArguments = @interface.GenericTypeArguments;
                        return true;
                    }
                }
            }

            if (type.IsGenericType)
            {
                if (genericTypeDefinition == type.GetGenericTypeDefinition())
                {
                    genericArguments = type.GenericTypeArguments;
                    return true;
                }
            }

            var baseType = type.BaseType;

            if (baseType is null)
            {
                genericArguments = Type.EmptyTypes;
                return false;
            }

            return baseType.IsAssignableTo(genericTypeDefinition, out genericArguments);
        }
    }
}
