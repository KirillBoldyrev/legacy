using ApprovalTests;
using ApprovalTests.Reporters;
using FakeItEasy;
using Newtonsoft.Json;
using NUnit.Framework;
using ProviderProcessing.ProcessReports;
using ProviderProcessing.ProviderDatas;
using ProviderProcessing.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderProcessor
{
    [TestFixture]
    public class ProviderProcessorTest
    {
        private ProviderProcessing.ProviderProcessor providerProcessor { get; set; }

        [SetUp]
        public void SetUp()
        {
            var productsReference = A.Fake<ProductsReference>();
            A.CallTo(() => productsReference.FindCodeByName("Product111")).Returns(111);
            A.CallTo(() => productsReference.FindCodeByName("Product222")).Returns(222);
            A.CallTo(() => productsReference.FindCodeByName("Product333")).Returns(null);

            var measureUnitsReference = A.Fake<MeasureUnitsReference>();
            A.CallTo(() => measureUnitsReference.FindByCode("measure01")).Returns(new MeasureUnit());
            A.CallTo(() => measureUnitsReference.FindByCode("measure02")).Returns(null);

            providerProcessor = A.Fake<ProviderProcessing.ProviderProcessor>();
            A.CallTo(() => providerProcessor.GetProductsReferenceInstance()).Returns(productsReference);
            A.CallTo(() => providerProcessor.GetMeasureUnitsReferenceInstance()).Returns(measureUnitsReference);
        }

        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void Validation_Test_PricesAndMeasures()
        {
            var prices = new decimal[] { -100, 0, 100 };
            var measureUnits = new string[] { "measure01", "measure02" };
            var products = new List<ProductData>();

            foreach (var p in prices)
            {
                foreach (var m in measureUnits)
                {
                    products.Add(new ProductData() { Price = p, MeasureUnitCode = m });
                }
            }

            var validationResult = new List<ProductValidationResult>();

            foreach (var p in products)
            {
                var vResult = providerProcessor.ValidatePricesAndMeasureUnitCodes(p);

                validationResult.AddRange(vResult);
            }

            Approvals.Verify(JsonConvert.SerializeObject(validationResult));
        }

        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void Validation_Test_Names()
        {
            var productNames = new string[] { "Product111", "Product222", "Product333" };
            var products = new List<ProductData>();

            foreach (var pName in productNames)
            {
                products.Add(new ProductData() { Name = pName });
            }

            var validationResult = new List<ProductValidationResult>();

            foreach (var p in products)
            {
                var vResult = providerProcessor.ValidatePricesAndMeasureUnitCodes(p);

                validationResult.AddRange(vResult);
            }

            Approvals.Verify(JsonConvert.SerializeObject(validationResult));
        }
    }
}
