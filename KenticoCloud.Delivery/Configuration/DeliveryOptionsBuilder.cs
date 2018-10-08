namespace KenticoCloud.Delivery.Configuration
{
    public class DeliveryOptionsBuilder
    {
        private string _productionEndpoint;
        private string _previewEndpoint;
        private string _projectId;
        private string _previewApiKey;
        private bool _usePreviewApi;
        private bool _waitForLoadingNewContent;
        private bool _useSecuredProductionApi;
        private string _securedProductionApiKey;
        private bool _resilienceLogicEnabled;
        private int _maxRetryAttempts;
        public DeliveryOptionsBuilder WithProductionEndpoint(string productionEndpoint)
        {
            _productionEndpoint = productionEndpoint;

            return this;
        }

        public DeliveryOptionsBuilder WithPreviewEndpoint(string previewEndpoint)
        {
            _previewEndpoint = previewEndpoint;

            return this;
        }

        public DeliveryOptionsBuilder WithProjectId(string projectId)
        {
            _projectId = projectId;

            return this;
        }

        public DeliveryOptionsBuilder WithPreviewApiKey(string previewApiKey)
        {
            _previewApiKey = previewApiKey;

            return this;
        }

        public DeliveryOptionsBuilder UsePreviewApi()
        {
            _usePreviewApi = true;

            return this;
        }

        public DeliveryOptionsBuilder WaitForLoadingNewContent()
        {
            _waitForLoadingNewContent = true;

            return this;
        }

        public DeliveryOptionsBuilder UseSecuredProductionApi()
        {
            _useSecuredProductionApi = true;

            return this;
        }

        public DeliveryOptionsBuilder WithSecuredProductionApiKey(string securedProductionApiKey)
        {
            _securedProductionApiKey = securedProductionApiKey;

            return this;
        }

        public DeliveryOptionsBuilder EnableResilienceLogic()
        {
            _resilienceLogicEnabled = true;

            return this;
        }

        public DeliveryOptionsBuilder WithMaxRetryAttempts(int attempts)
        {
            _maxRetryAttempts = attempts;

            return this;
        }

        public DeliveryOptions Build()
        {
            return new DeliveryOptions
            {
                ProjectId = _projectId,
                EnableResilienceLogic = _resilienceLogicEnabled,
                MaxRetryAttempts = _maxRetryAttempts,
                PreviewApiKey = _previewApiKey,
                PreviewEndpoint = _previewEndpoint,
                ProductionEndpoint = _productionEndpoint,
                SecuredProductionApiKey = _securedProductionApiKey,
                UsePreviewApi = _usePreviewApi,
                UseSecuredProductionApi = _useSecuredProductionApi,
                WaitForLoadingNewContent = _waitForLoadingNewContent
            };
        }
    }
}
