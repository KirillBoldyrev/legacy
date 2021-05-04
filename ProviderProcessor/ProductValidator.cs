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
    public class ProductValidator
    {
        private ProductsReference productsReference { get; set; }
        private MeasureUnitsReference measureUnitsReference { get; set; }

        public ProductValidator(ProductsReference productsReference, MeasureUnitsReference measureUnitsReference)
        {
            this.productsReference = productsReference ?? ProductsReference.GetInstance();
            this.measureUnitsReference = measureUnitsReference ?? MeasureUnitsReference.GetInstance();
        }

        public IList<ProductValidationResult> ValidateProduct(ProductData product)
        {
            var namesValidationResults = ValidateNames(product).ToList();
            var pricesValidationResults = ValidatePrices(product);
            var measuresValidationResults = ValidateMeasureUnitCodes(product);

            var result = namesValidationResults.Concat(pricesValidationResults).Concat(measuresValidationResults).ToList();

            return result;
        }

        private IEnumerable<ProductValidationResult> ValidateNames(ProductData product)
        {
            var reference = GetProductsReferenceInstance();

            if (!reference.FindCodeByName(product.Name).HasValue)
                yield return new ProductValidationResult(product,
                    "Unknown product name", ProductValidationSeverity.Error);
        }

        private IEnumerable<ProductValidationResult> ValidatePrices(ProductData product)
        {
            if (product.Price <= 0)
                yield return new ProductValidationResult(product, "Bad price", ProductValidationSeverity.Warning);
        }

        private IEnumerable<ProductValidationResult> ValidateMeasureUnitCodes(ProductData product)
        {
            if (!IsValidMeasureUnitCode(product.MeasureUnitCode))
                yield return new ProductValidationResult(product,
                    "Bad units of measure", ProductValidationSeverity.Warning);
        }

        private bool IsValidMeasureUnitCode(string measureUnitCode)
        {
            var reference = GetMeasureUnitsReferenceInstance();
            return reference.FindByCode(measureUnitCode) != null;
        }

        protected virtual ProductsReference GetProductsReferenceInstance()
        {
            return productsReference;
        }

        protected virtual MeasureUnitsReference GetMeasureUnitsReferenceInstance()
        {
            return measureUnitsReference;
        }
    }
}
