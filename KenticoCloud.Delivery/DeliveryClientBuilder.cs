using System.Net.Http;
using KenticoCloud.Delivery.InlineContentItems;
using KenticoCloud.Delivery.ResiliencePolicy;
using Microsoft.Extensions.Options;

namespace KenticoCloud.Delivery
{
    public class DeliveryClientBuilder
    {
        private IContentLinkUrlResolver _contentLinkUrlResolver;
        private IInlineContentItemsProcessor _inlineContentItemsProcessor;
        private ICodeFirstModelProvider _codeFirstModelProvider;
        private IResiliencePolicyProvider _resiliencePolicyProvider;
        private ICodeFirstTypeProvider _codeFirstTypeProvider;
        private ICodeFirstPropertyMapper _codeFirstPropertyMapper;
        private HttpClient _httpClient;
        private DeliveryOptions _deliveryOptions;

        public DeliveryClientBuilder()
        {
            _deliveryOptions = new DeliveryOptions();
        }

        public DeliveryClientBuilder WithProjectId(string projectId)
        {
            _deliveryOptions.ProjectId = projectId;

            return this;
        }

        public DeliveryClientBuilder WithPreviewApiKey(string previewApiKey)
        {
            _deliveryOptions.PreviewApiKey = previewApiKey;
            _deliveryOptions.UsePreviewApi = true;

            return this;
        }

        public DeliveryClientBuilder WithDeliveryOptions(DeliveryOptions deliveryOptions)
        {
            _deliveryOptions = deliveryOptions;

            return this;
        }

        public DeliveryClientBuilder WithHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            return this;
        }

        public DeliveryClientBuilder WithContentLinkUrlResolver(IContentLinkUrlResolver contentLinkUrlResolver)
        {
            _contentLinkUrlResolver = contentLinkUrlResolver;

            return this;
        }

        public DeliveryClientBuilder WithInlineContentItemsProcessor(IInlineContentItemsProcessor inlineContentItemsProcessor)
        {
            _inlineContentItemsProcessor = inlineContentItemsProcessor;

            return this;
        }

        public DeliveryClientBuilder WithCodeFirstModelProvider(ICodeFirstModelProvider codeFirstModelProvider)
        {
            _codeFirstModelProvider = codeFirstModelProvider;

            return this;
        }

        public DeliveryClientBuilder WithCodeFirstTypeProvider(ICodeFirstTypeProvider codeFirstTypeProvider)
        {
            _codeFirstTypeProvider = codeFirstTypeProvider;

            return this;
        }

        public DeliveryClientBuilder WithResiliencePolicyProvider(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;

            return this;
        }

        public DeliveryClientBuilder WithCodeFirstPropertyMapper(ICodeFirstPropertyMapper propertyMapper)
        {
            _codeFirstPropertyMapper = propertyMapper;

            return this;
        }

        public DeliveryClient Build()
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
    }
}