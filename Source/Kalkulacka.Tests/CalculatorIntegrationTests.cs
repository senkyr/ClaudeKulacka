using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCalculator.ViewModel;
using System;
using System.Threading;

namespace SimpleCalculator.Tests.Integration
{
    [TestClass]
    public class CalculatorIntegrationTests
    {
        private CalculatorViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new CalculatorViewModel();
        }

        [TestMethod]
        public void SimpleAddition_ExecutesThroughViewModelToModel()
        {
            // Act - provádí integraci mezi ViewModelem a Modelem
            _viewModel.NumberCommand.Execute("5");
            _viewModel.OperationCommand.Execute("+");
            _viewModel.NumberCommand.Execute("3");
            _viewModel.EqualsCommand.Execute(null);

            // Assert
            Assert.AreEqual("8", _viewModel.DisplayText);
        }

        [TestMethod]
        public void SequenceOfOperations_ExecutesCorrectlyThroughLayers()
        {
            // Testuje sekvenci operací procházející skrz ViewModel do Modelu

            // Act: 5 × 3 - 2 = 13
            _viewModel.NumberCommand.Execute("5");
            _viewModel.OperationCommand.Execute("×");
            _viewModel.NumberCommand.Execute("3");
            _viewModel.EqualsCommand.Execute(null); // Výsledek bude 15
            _viewModel.OperationCommand.Execute("−");
            _viewModel.NumberCommand.Execute("2");
            _viewModel.EqualsCommand.Execute(null); // Výsledek bude 13

            // Assert
            Assert.AreEqual("13", _viewModel.DisplayText);
        }

        [TestMethod]
        public void ComplexCalculation_IntegratesCorrectly()
        {
            // Testuje složitější výpočet procházející všemi vrstvami

            // (10 + 5²) ÷ 3 = 11,67
            _viewModel.NumberCommand.Execute("10");
            _viewModel.OperationCommand.Execute("+");
            _viewModel.NumberCommand.Execute("5");
            _viewModel.OperationCommand.Execute("x²"); // 5² = 25
            _viewModel.EqualsCommand.Execute(null); // 10 + 25 = 35
            _viewModel.OperationCommand.Execute("÷");
            _viewModel.NumberCommand.Execute("3");
            _viewModel.EqualsCommand.Execute(null); // 35 ÷ 3 = 11,67

            // Převádíme výsledek na číslo pro přesnější porovnání
            double result = double.Parse(_viewModel.DisplayText);
            Assert.IsTrue(Math.Abs(result - 11.67) < 0.01);
        }

        [TestMethod]
        public void ErrorHandling_PropagatesThroughLayers()
        {
            // Test ověřuje, že chybové stavy (např. dělení nulou) správně 
            // procházejí z Modelu do ViewModelu

            _viewModel.NumberCommand.Execute("5");
            _viewModel.OperationCommand.Execute("÷");
            _viewModel.NumberCommand.Execute("0");
            _viewModel.EqualsCommand.Execute(null);

            Assert.AreEqual("Nelze dělit nulou", _viewModel.DisplayText);
        }

        [TestMethod]
        public void SpecialOperations_IntegrateCorrectly()
        {
            // Test ověřuje integraci speciálních operací (%, √, 1/x)

            // Test procenta
            _viewModel.NumberCommand.Execute("200");
            _viewModel.OperationCommand.Execute("+");
            _viewModel.NumberCommand.Execute("10");
            _viewModel.OperationCommand.Execute("%"); // 10% z 200 = 20
            _viewModel.EqualsCommand.Execute(null); // 200 + 20 = 220
            Assert.AreEqual("220", _viewModel.DisplayText);

            // Vyčištění
            _viewModel.ClearCommand.Execute(null);

            // Test odmocniny
            _viewModel.NumberCommand.Execute("16");
            _viewModel.OperationCommand.Execute("²√x");
            Assert.AreEqual("4", _viewModel.DisplayText);

            // Test převrácené hodnoty
            _viewModel.ClearCommand.Execute(null);
            _viewModel.NumberCommand.Execute("5");
            _viewModel.OperationCommand.Execute("¹/ₓ");
            Assert.AreEqual("0,2", _viewModel.DisplayText);
        }

        [TestMethod]
        public void ClearAndBackspace_IntegrateCorrectly()
        {
            // Test ověřuje integraci mazacích funkcí

            // Testujeme Clear
            _viewModel.NumberCommand.Execute("123");
            _viewModel.ClearCommand.Execute(null);
            Assert.AreEqual("0", _viewModel.DisplayText);

            // Testujeme ClearEntry
            _viewModel.NumberCommand.Execute("5");
            _viewModel.OperationCommand.Execute("+");
            _viewModel.NumberCommand.Execute("3");
            _viewModel.ClearEntryCommand.Execute(null);
            _viewModel.NumberCommand.Execute("7");
            _viewModel.EqualsCommand.Execute(null);
            Assert.AreEqual("12", _viewModel.DisplayText);

            // Testujeme Backspace
            _viewModel.ClearCommand.Execute(null);
            _viewModel.NumberCommand.Execute("123");
            _viewModel.BackspaceCommand.Execute(null);
            Assert.AreEqual("12", _viewModel.DisplayText);
        }

        [TestMethod]
        public void SignChange_IntegratesCorrectly()
        {
            // Test ověřuje integraci změny znaménka

            _viewModel.NumberCommand.Execute("5");
            _viewModel.ToggleSignCommand.Execute(null);
            Assert.AreEqual("-5", _viewModel.DisplayText);

            _viewModel.ToggleSignCommand.Execute(null);
            Assert.AreEqual("5", _viewModel.DisplayText);
        }

        [TestMethod]
        public void DecimalPoint_IntegratesCorrectly()
        {
            // Test ověřuje integraci desetinné čárky

            _viewModel.NumberCommand.Execute("5");
            _viewModel.DecimalPointCommand.Execute(null);
            _viewModel.NumberCommand.Execute("25");
            Assert.AreEqual("5,25", _viewModel.DisplayText);

            // Ověření, že nelze přidat více desetinných čárek
            _viewModel.DecimalPointCommand.Execute(null);
            _viewModel.NumberCommand.Execute("3");
            Assert.AreEqual("5,253", _viewModel.DisplayText);
        }

        [TestMethod]
        public void ChainedCalculations_IntegrateCorrectly()
        {
            // Test ověřuje řetězení výpočtů

            // 5 + 3 = 8, pak + 2 = 10
            _viewModel.NumberCommand.Execute("5");
            _viewModel.OperationCommand.Execute("+");
            _viewModel.NumberCommand.Execute("3");
            _viewModel.EqualsCommand.Execute(null);
            Assert.AreEqual("8", _viewModel.DisplayText);

            _viewModel.OperationCommand.Execute("+");
            _viewModel.NumberCommand.Execute("2");
            _viewModel.EqualsCommand.Execute(null);
            Assert.AreEqual("10", _viewModel.DisplayText);
        }
    }
}
