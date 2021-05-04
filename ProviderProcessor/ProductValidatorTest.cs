using ApprovalTests;
using ApprovalTests.Reporters;
using FakeItEasy;
using NUnit.Framework;
using ProviderProcessing.ProviderDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderProcessor
{
    [TestFixture]
    public class ProductValidatorTest
    {
        private ProductValidator productValidator { get; set; }
        
        [SetUp]
        [UseReporter(typeof(DiffReporter))]
        public void SetUp()
        {
            productValidator = A.Fake<ProductValidator>();
        }

        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void Validation_Test_InitialState()
        {
            var emptyProduct = new ProductData();
            Approvals.Verify(emptyProduct);
        }

        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void Validation_Test_Prices([Values(-100, 0, 100)] decimal productPrice)
        {
            var pricedProduct = new ProductData() { Price = productPrice };
            var validationResult = productValidator.ValidateProduct(pricedProduct);
        }

        ////[Test]
        ////[UseReporter(typeof(DiffReporter))]
        ////public void Validation_Test_Names([Values("Name01", "Name02", "Name03")] string productName)
        ////{
        ////    var namedProduct = new ProductData() { Name = productName };
        ////    productValidator.ValidateProduct(namedProduct);
        ////}


    }
}
