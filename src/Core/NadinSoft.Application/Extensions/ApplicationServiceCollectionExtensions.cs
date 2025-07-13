using System.Reflection;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Application.Common.MappingConfiguration;
using NadinSoft.Application.Common.Validation;
using NadinSoft.Application.Features.Common;

namespace NadinSoft.Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationValidator(this IServiceCollection services)
    {
        var validationTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(c =>
            c.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidatableModel<>)));

        foreach (var validationType in validationTypes)
        {
            var biggestConstructorLength = validationType.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length).First().GetParameters().Length;

            var requestModel = Activator.CreateInstance(validationType, new object[biggestConstructorLength]);
            if (requestModel is null)
                continue;

            var requestModelInfo = validationType.GetMethod(nameof(IValidatableModel<object>.Validate));
            var validationModelBase =
                Activator.CreateInstance(typeof(ValidationModelBase<>).MakeGenericType(validationType));

            if (validationModelBase is null)
                continue;

            var validator = requestModelInfo?.Invoke(requestModel, [validationModelBase]);
            if (validator is null)
                continue;

            var validatorInterfaces = validator.GetType().GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

            if (validatorInterfaces is null)
                continue;

            services.AddTransient(validatorInterfaces, _ => validator);
        }

        return services;
    }
 
    public static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(RegisterApplicationMappers));
        return services;
    }

    public static IServiceCollection AddApplicationMediatorServices(this IServiceCollection services)
    {
        services.AddMediator(opt =>
        {
            opt.ServiceLifetime = ServiceLifetime.Transient;
            opt.Namespace = "NadinSoft.Application.GeneratedMediatorServices";
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidateRequestBehavior<,>));
        return services;
    }
}