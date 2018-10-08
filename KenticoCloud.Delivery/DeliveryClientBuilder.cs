using System;
using System.Net.Http;
using KenticoCloud.Delivery.Configuration;
using KenticoCloud.Delivery.InlineContentItems;
using KenticoCloud.Delivery.ResiliencePolicy;
using Microsoft.Extensions.Options;

namespace KenticoCloud.Delivery
{
    public interface IBuildStep
    {
        IDeliveryClient Build();
    }

    public interface IMandatoryStep
    {
        IOptionalStep WithProjectId(string projectId);
        IOptionalStep WithDeliveryOptions(DeliveryOptions deliveryOptions);
    }

    public interface IOptionalStep : IBuildStep
    {
        IOptionalStep WithPreviewApiKey(string previewApiKey);
        IOptionalStep WithHttpClient(HttpClient httpClient);
        IOptionalStep WithContentLinkUrlResolver(IContentLinkUrlResolver contentLinkUrlResolver);
        IOptionalStep WithInlineContentItemsProcessor(IInlineContentItemsProcessor inlineContentItemsProcessor);
        IOptionalStep WithCodeFirstModelProvider(ICodeFirstModelProvider codeFirstModelProvider);
        IOptionalStep WithCodeFirstTypeProvider(ICodeFirstTypeProvider codeFirstTypeProvider);
        IOptionalStep WithResiliencePolicyProvider(IResiliencePolicyProvider resiliencePolicyProvider);
        IOptionalStep WithCodeFirstPropertyMapper(ICodeFirstPropertyMapper propertyMapper);
    }
    public class Ahoj
    {
        public Ahoj()
        {
            var c = DeliveryClientBuilder
                .GetDeliveryClientBuilder()
                .WithProjectId("123")
                .Build();
        }
    }

    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj) => obj == null;
    }
    public class DeliveryClientBuilder
    {
        internal DeliveryOptions DeliveryOptions;

        public static IMandatoryStep GetDeliveryClientBuilder()
        {
            return new Steps();
        }

        private DeliveryClientBuilder() {}

        public DeliveryClientBuilder WithDeliveryOptions(Func<DeliveryOptionsBuilder, DeliveryOptionsBuilder> buildDeliveryOptions)
        {
            DeliveryOptions = buildDeliveryOptions(new DeliveryOptionsBuilder()).Build();

            //WithDeliveryOptions(options =>
            //{
            //    options.ProjectId = "";
            //    options.PreviewApiKey = "";
            //    return options;
            //});
            WithDeliveryOptions(optionsBuilder => optionsBuilder.WithProjectId("123").UsePreviewApi());

            return this;
        }

        private class Steps : IMandatoryStep, IOptionalStep
        {
            private IContentLinkUrlResolver _contentLinkUrlResolver;
            private IInlineContentItemsProcessor _inlineContentItemsProcessor;
            private ICodeFirstModelProvider _codeFirstModelProvider;
            private IResiliencePolicyProvider _resiliencePolicyProvider;
            private ICodeFirstTypeProvider _codeFirstTypeProvider;
            private ICodeFirstPropertyMapper _codeFirstPropertyMapper;
            private HttpClient _httpClient;
            private DeliveryOptions _deliveryOptions;
            public IOptionalStep WithProjectId(string projectId)
            {
                ValidateProjectId(projectId);
                _deliveryOptions = new DeliveryOptions {ProjectId = projectId};

                return this;
            }

            public IOptionalStep WithDeliveryOptions(DeliveryOptions deliveryOptions)
            {
                ValidateDeliveryOptions(deliveryOptions);

                _deliveryOptions = deliveryOptions;

                return this;
            }

            public IOptionalStep WithPreviewApiKey(string previewApiKey)
            {
                ValidatePreviewApiKey(previewApiKey);

                _deliveryOptions.PreviewApiKey = previewApiKey;
                _deliveryOptions.UsePreviewApi = true;

                return this;
            }

            public IOptionalStep WithHttpClient(HttpClient httpClient)
            {
                _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient), "Http client is not specified");

                return this;
            }

            public IOptionalStep WithContentLinkUrlResolver(IContentLinkUrlResolver contentLinkUrlResolver)
            {
                _contentLinkUrlResolver = contentLinkUrlResolver;

                return this;
            }

            public IOptionalStep WithInlineContentItemsProcessor(IInlineContentItemsProcessor inlineContentItemsProcessor)
            {
                _inlineContentItemsProcessor = inlineContentItemsProcessor;

                return this;
            }

            public IOptionalStep WithCodeFirstModelProvider(ICodeFirstModelProvider codeFirstModelProvider)
            {
                _codeFirstModelProvider = codeFirstModelProvider;

                return this;
            }

            public IOptionalStep WithCodeFirstTypeProvider(ICodeFirstTypeProvider codeFirstTypeProvider)
            {
                _codeFirstTypeProvider = codeFirstTypeProvider;

                return this;
            }

            public IOptionalStep WithResiliencePolicyProvider(IResiliencePolicyProvider resiliencePolicyProvider)
            {
                _resiliencePolicyProvider = resiliencePolicyProvider;

                return this;
            }

            public IOptionalStep WithCodeFirstPropertyMapper(ICodeFirstPropertyMapper propertyMapper)
            {
                _codeFirstPropertyMapper = propertyMapper;

                return this;
            }

            public IDeliveryClient Build()
            {
                var deliveryOptionsWrapper = new OptionsWrapper<DeliveryOptions>(_deliveryOptions);

                var inlineContentItemsProcessor =
                    _inlineContentItemsProcessor ??
                    new InlineContentItemsProcessor(
                        new ReplaceWithWarningAboutRegistrationResolver(),
                        new ReplaceWithWarningAboutUnretrievedItemResolver()
                    );
                var codeFirstModelProvider =
                    _codeFirstModelProvider ??
                    new CodeFirstModelProvider(
                        _contentLinkUrlResolver,
                        inlineContentItemsProcessor,
                        _codeFirstTypeProvider,
                        _codeFirstPropertyMapper
                    );

                return new DeliveryClient(
                    deliveryOptionsWrapper,
                    _contentLinkUrlResolver,
                    inlineContentItemsProcessor,
                    codeFirstModelProvider,
                    _resiliencePolicyProvider
                )
                {
                    HttpClient = _httpClient
                };
            }

            private void ValidateDeliveryOptions(DeliveryOptions deliveryOptions)
            {
                _deliveryOptions = deliveryOptions ?? throw new ArgumentNullException(nameof(deliveryOptions), "The Delivery options object is not specified.");
                ValidateProjectId(deliveryOptions.ProjectId);

                if (_deliveryOptions.UsePreviewApi)
                {
                    ValidatePreviewApiKey(_deliveryOptions.PreviewApiKey);
                }
            }

            private void ValidatePreviewApiKey(string previewApiKey)
            {
                if (previewApiKey == null)
                {
                    throw new ArgumentNullException(nameof(previewApiKey), "The Preview API key is not specified.");
                }

                if (previewApiKey == string.Empty)
                {
                    throw new ArgumentException("The Preview API key is not specified.", nameof(previewApiKey));
                }
            }

            private void ValidateProjectId(string projectId)
            {
                if (projectId == null)
                {
                    throw new ArgumentNullException(nameof(projectId), "Kentico Cloud project identifier is not specified.");
                }

                if (projectId == string.Empty)
                {
                    throw new ArgumentException("Kentico Cloud project identifier is not specified.", nameof(projectId));
                }

                if (!Guid.TryParse(projectId, out Guid projectIdGuid))
                {
                    throw new ArgumentException("Provided string is not a valid project identifier ({ProjectId}). Haven't you accidentally passed the Preview API key instead of the project identifier?", nameof(_deliveryOptions.ProjectId));
                }
            }
        }
    }
}
