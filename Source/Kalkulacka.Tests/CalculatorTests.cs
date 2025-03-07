using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCalculator.Model;

namespace SimpleCalculator.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        private Calculator _calculator;

        [TestInitialize]
        public void Setup()
        {
            _calculator = new Calculator();
        }

        [TestMethod]
        public void Calculator_InitialState_DisplaysZero()
        {
            // Ověření počátečního stavu
            Assert.AreEqual("0", _calculator.Result);
        }

        [TestMethod]
        public void AppendDigit_SingleDigit_DisplaysDigit()
        {
            // Arrange - již provedeno v Setup

            // Act
            _calculator.AppendDigit("5");

            // Assert
            Assert.AreEqual("5", _calculator.Result);
        }

        [TestMethod]
        public void AppendDigit_MultipleDigits_DisplaysAllDigits()
        {
            // Act
            _calculator.AppendDigit("1");
            _calculator.AppendDigit("2");
            _calculator.AppendDigit("3");

            // Assert
            Assert.AreEqual("123", _calculator.Result);
        }

        [TestMethod]
        public void AppendDecimalPoint_AfterDigits_AddsDecimalPoint()
        {
            // Act
            _calculator.AppendDigit("5");
            _calculator.AppendDecimalPoint();

            // Assert
            Assert.AreEqual("5,", _calculator.Result);
        }

        [TestMethod]
        public void AppendDecimalPoint_MultipleTimes_AddsOnlyOnce()
        {
            // Act
            _calculator.AppendDigit("5");
            _calculator.AppendDecimalPoint();
            _calculator.AppendDecimalPoint();
            _calculator.AppendDigit("2");

            // Assert
            Assert.AreEqual("5,2", _calculator.Result);
        }

        [TestMethod]
        public void ToggleSign_PositiveNumber_MakesNegative()
        {
            // Act
            _calculator.AppendDigit("5");
            _calculator.ToggleSign();

            // Assert
            Assert.AreEqual("-5", _calculator.Result);
        }

        [TestMethod]
        public void ToggleSign_NegativeNumber_MakesPositive()
        {
            // Act
            _calculator.AppendDigit("5");
            _calculator.ToggleSign();
            _calculator.ToggleSign();

            // Assert
            Assert.AreEqual("5", _calculator.Result);
        }

        [TestMethod]
        public void Addition_TwoPositiveNumbers_CalculatesCorrectSum()
        {
            // Act
            _calculator.AppendDigit("5");
            _calculator.SetOperation("+");
            _calculator.AppendDigit("3");
            _calculator.CalculateResult();

            // Assert
            Assert.AreEqual("8", _calculator.Result);
        }

        [TestMethod]
        public void Subtraction_LargerFromSmaller_CalculatesNegativeResult()
        {
            // Act
            _calculator.AppendDigit("3");
            _calculator.SetOperation("−");
            _calculator.AppendDigit("5");
            _calculator.CalculateResult();

            // Assert
            Assert.AreEqual("-2", _calculator.Result);
        }

        [TestMethod]
        public void Multiplication_TwoNumbers_CalculatesCorrectProduct()
        {
            // Act
            _calculator.AppendDigit("4");
            _calculator.SetOperation("×");
            _calculator.AppendDigit("5");
            _calculator.CalculateResult();

            // Assert
            Assert.AreEqual("20", _calculator.Result);
        }

        [TestMethod]
        public void Division_ByZero_DisplaysErrorMessage()
        {
            // Act
            _calculator.AppendDigit("5");
            _calculator.SetOperation("÷");
            _calculator.AppendDigit("0");
            _calculator.CalculateResult();

            // Assert
            Assert.AreEqual("Nelze dělit nulou", _calculator.Result);
        }

        [TestMethod]
        public void SquareOperation_PositiveNumber_CalculatesCorrectResult()
        {
            // Act
            _calculator.AppendDigit("3");
            _calculator.SquareOperation();

            // Assert
            Assert.AreEqual("9", _calculator.Result);
        }

        [TestMethod]
        public void SquareRootOperation_PositiveNumber_CalculatesCorrectResult()
        {
            // Act
            _calculator.AppendDigit("9");
            _calculator.SquareRootOperation();

            // Assert
            Assert.AreEqual("3", _calculator.Result);
        }

        [TestMethod]
        public void SquareRootOperation_NegativeNumber_DisplaysErrorMessage()
        {
            // Act
            _calculator.AppendDigit("9");
            _calculator.ToggleSign();
            _calculator.SquareRootOperation();

            // Assert
            Assert.AreEqual("Neplatná operace", _calculator.Result);
        }

        [TestMethod]
        public void ReciprocalOperation_NonZeroNumber_CalculatesCorrectResult()
        {
            // Act
            _calculator.AppendDigit("2");
            _calculator.ReciprocalOperation();

            // Assert
            Assert.AreEqual("0,5", _calculator.Result);
        }

        [TestMethod]
        public void Clear_AfterInput_ResetsToInitialState()
        {
            // Act
            _calculator.AppendDigit("5");
            _calculator.SetOperation("+");
            _calculator.AppendDigit("3");
            _calculator.Clear();

            // Assert
            Assert.AreEqual("0", _calculator.Result);
        }

        [TestMethod]
        public void ClearEntry_AfterInput_ClearsOnlyCurrentEntry()
        {
            // Act
            _calculator.AppendDigit("5");
            _calculator.SetOperation("+");
            _calculator.AppendDigit("3");
            _calculator.ClearEntry();
            _calculator.AppendDigit("7");
            _calculator.CalculateResult();

            // Assert
            Assert.AreEqual("12", _calculator.Result);
        }

        [TestMethod]
        public void Backspace_MultipleDigits_RemovesLastDigit()
        {
            // Act
            _calculator.AppendDigit("1");
            _calculator.AppendDigit("2");
            _calculator.AppendDigit("3");
            _calculator.Backspace();

            // Assert
            Assert.AreEqual("12", _calculator.Result);
        }

        [TestMethod]
        public void PercentOperation_Basic_CalculatesCorrectPercentage()
        {
            // Act
            _calculator.AppendDigit("50");
            _calculator.PercentOperation();

            // Assert
            Assert.AreEqual("0,5", _calculator.Result);
        }

        [TestMethod]
        public void PercentOperation_InExpression_CalculatesPercentOfFirstNumber()
        {
            // Act
            _calculator.AppendDigit("200");
            _calculator.SetOperation("+");
            _calculator.AppendDigit("10");
            _calculator.PercentOperation();  // 10% z 200 = 20
            _calculator.CalculateResult();   // 200 + 20 = 220

            // Assert
            Assert.AreEqual("220", _calculator.Result);
        }
    }
}
