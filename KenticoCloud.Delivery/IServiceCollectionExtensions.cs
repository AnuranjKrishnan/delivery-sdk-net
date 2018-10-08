using System;
using System.Collections;
using System.Collections.Generic;
using KenticoCloud.Delivery.InlineContentItems;
using KenticoCloud.Delivery.ResiliencePolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KenticoCloud.Delivery
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDeliveryClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.TryAddTransient(serviceProvider => (IContentLinkUrlResolver)null); //rozdelit do metod
            services.TryAddTransient(serviceProvider => (ICodeFirstTypeProvider)null);
            services.TryAddTransient<IInlineContentItemsResolver<object>, ReplaceWithWarningAboutRegistrationResolver>();
            services.TryAddTransient<IInlineContentItemsResolver<UnretrievedContentItem>, ReplaceWithWarningAboutUnretrievedItemResolver>();
            services.TryAddTransient<IInlineContentItemsProcessor, InlineContentItemsProcessor>();
            services.TryAddTransient<ICodeFirstModelProvider, CodeFirstModelProvider>();
            services.TryAddTransient<ICodeFirstPropertyMapper, CodeFirstPropertyMapper>();

            services.TryAddTransient<IResiliencePolicyProvider>(serviceProvider =>
            {
                var deliveryOptions = serviceProvider.GetService<IOptions<DeliveryOptions>>();

                return new DefaultResiliencePolicyProvider(deliveryOptions.Value.MaxRetryAttempts);
            });

            services.TryAddTransient<IDeliveryClient>(serviceProvider =>
            {
                var deliveryOptions = serviceProvider.GetService<IOptions<DeliveryOptions>>();
                var contentLinkUrlResolver = serviceProvider.GetService<IContentLinkUrlResolver>();
                var inlineContentItemsProcessor = serviceProvider.GetService<IInlineContentItemsProcessor>();
                var codeFirstModelProvider = serviceProvider.GetService<ICodeFirstModelProvider>();
                var resiliencePolicyProvider = serviceProvider.GetService<IResiliencePolicyProvider>();
                var codeFirstTypeProvider = serviceProvider.GetService<ICodeFirstTypeProvider>();
                var codeFirstPropertyMapper = serviceProvider.GetService<ICodeFirstPropertyMapper>();

                return new DeliveryClient(
                    deliveryOptions,
                    contentLinkUrlResolver,
                    inlineContentItemsProcessor,
                    codeFirstModelProvider,
                    resiliencePolicyProvider,
                    codeFirstTypeProvider,
                    codeFirstPropertyMapper
                );
            });

            return services;
        }
    }
}