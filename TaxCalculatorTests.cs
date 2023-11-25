using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaxCalculator.Data;
using TaxCalculator.Models;
using TaxCalculator.Service.Contract;
using TaxCalculator.Service.Implementation;
//TaxCalculatorUnitTests
namespace TaxCalculatorUnitTests
{
    [TestFixture]
    [TestFixture]
    public class TaxCalculatorTests
    {
        private TaxCalculator.Service.Implementation.TaxCalculator _taxCalculator;
        private ILogger<TaxCalculator.Service.Implementation.TaxCalculator> _logger;
        private TaxCalculationContext _taxContext;

        [SetUp]
        public void Setup()
        {
            _logger = new Logger<TaxCalculator.Service.Implementation.TaxCalculator>(new LoggerFactory());
            var optionsBuilder = new DbContextOptionsBuilder<TaxCalculationContext>();

            DbContextOptions options = optionsBuilder.Options;
            _taxCalculator = new TaxCalculator.Service.Implementation.TaxCalculator(_logger, _taxContext);
        }

        [Test]
        public void CalculateTax_WithFlatRate_ReturnsCorrectTax()
        {
            var postalCode = "7000";
            var annualIncome = 100000m;
            var postalCodeEntity = new PostalCodes { Code = postalCode, TaxCalculationType = "Flat rate" };
            _taxContext.PostalCodes.Add(postalCodeEntity);

            var result = _taxCalculator.CalculateTax(postalCode, annualIncome);

            Assert.AreEqual(annualIncome * 0.175m, result);
        }

        [Test]
        public void CalculateTax_WithFlatValueAndIncomeLessThan200000_ReturnsCorrectTax()
        {
            var postalCode = "A100";
            var annualIncome = 150000m;
            var postalCodeEntity = new PostalCodes { Code = postalCode, TaxCalculationType = "Flat Value" };
            _taxContext.PostalCodes.Add(postalCodeEntity);

            var result = _taxCalculator.CalculateTax(postalCode, annualIncome);
            Assert.AreEqual(annualIncome * 0.05m, result);
        }

        [Test]
        public void CalculateTax_WithFlatValueAndIncomeGreaterThan200000_ReturnsCorrectTax()
        {
            var postalCode = "A100";
            var annualIncome = 250000m;
            var postalCodeEntity = new PostalCodes { Code = postalCode, TaxCalculationType = "Flat Value" };
            _taxContext.PostalCodes.Add(postalCodeEntity);

            var result = _taxCalculator.CalculateTax(postalCode, annualIncome);
            Assert.AreEqual(10000, result);
        }

        [Test]
        public void CalculateTax_WithProgressiveAndIncomeLessThan8350_ReturnsCorrectTax()
        {
            var postalCode = "7441";
            var annualIncome = 8000m;
            var postalCodeEntity = new PostalCodes { Code = postalCode, TaxCalculationType = "Progressive" };
            _taxContext.PostalCodes.Add(postalCodeEntity);

            var result = _taxCalculator.CalculateTax(postalCode, annualIncome);
            Assert.AreEqual(annualIncome * 0.10m, result);
        }


    }


}