//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace KenticoCloud.Delivery.Tests
//{
//    public class DeliveryClientBuilderTests
//    {
//        private const string Guid = "e9403c90-4eaf-4b53-9b84-aba3c77af2f0";
//        private readonly DeliveryClientBuilder _deliveryClientBuilder;

//        public DeliveryClientBuilderTests()
//        {
//            _deliveryClientBuilder = new DeliveryClientBuilder();
//        }

//        [Fact]
//        public void WithProjectId_ProjectId_SetsProjectId()
//        {
//            _deliveryClientBuilder.WithProjectId(Guid);

//            Assert.NotNull(_deliveryClientBuilder.DeliveryOptions.ProjectId);
//        }

//        [Fact]
//        public void WithPreviewApiKey_PreviewApiKey_SetsPreviewApiKey()
//        {
//            _deliveryClientBuilder.WithPreviewApiKey(Guid);

//            Assert.NotNull(_deliveryClientBuilder.DeliveryOptions.ProjectId);
//        }
//    }
//}
