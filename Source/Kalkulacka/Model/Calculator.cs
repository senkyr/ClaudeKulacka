using System;

namespace SimpleCalculator.Model
{
    /// <summary>
    /// Model reprezentující logiku kalkulačky
    /// </summary>
    public class Calculator
    {
        private double firstNumber;
        private double secondNumber;
        private string currentOperation;
        private bool isNewInput;

        public Calculator()
        {
            Clear();
        }

        /// <summary>
        /// Vyčistí celou kalkulačku
        /// </summary>
        public void Clear()
        {
            firstNumber = 0;
            secondNumber = 0;
            currentOperation = "";
            isNewInput = true;
            Result = "0";
        }

        /// <summary>
        /// Vyčistí aktuální vstup
        /// </summary>
        public void ClearEntry()
        {
            Result = "0";
            isNewInput = true;
        }

        /// <summary>
        /// Výsledek aktuálního výpočtu nebo vstupní hodnota
        /// </summary>
        public string Result { get; private set; }

        /// <summary>
        /// Přidá číslici do aktuálního vstupu
        /// </summary>
        /// <param name="digit">Číslice k přidání</param>
        public void AppendDigit(string digit)
        {
            if (isNewInput || Result == "0")
            {
                Result = digit;
                isNewInput = false;
            }
            else
            {
                Result += digit;
            }
        }

        /// <summary>
        /// Přidá desetinnou čárku do aktuálního vstupu
        /// </summary>
        public void AppendDecimalPoint()
        {
            if (isNewInput)
            {
                Result = "0,";
                isNewInput = false;
                return;
            }

            if (!Result.Contains(","))
            {
                Result += ",";
            }
        }

        /// <summary>
        /// Změní znaménko aktuálního vstupu
        /// </summary>
        public void ToggleSign()
        {
            if (Result != "0")
            {
                if (Result.StartsWith("-"))
                {
                    Result = Result.Substring(1);
                }
                else
                {
                    Result = "-" + Result;
                }
            }
        }

        /// <summary>
        /// Odstraní poslední znak z aktuálního vstupu
        /// </summary>
        public void Backspace()
        {
            if (!isNewInput && Result.Length > 0)
            {
                Result = Result.Length == 1 ? "0" : Result.Substring(0, Result.Length - 1);
            }
        }

        /// <summary>
        /// Provede operaci procent
        /// </summary>
        public void PercentOperation()
        {
            double currentValue = ParseCurrentValue();

            if (currentOperation != "" && !isNewInput)
            {
                // Procento z první hodnoty
                secondNumber = firstNumber * (currentValue / 100);
                Result = secondNumber.ToString();
            }
            else
            {
                // Převod na procenta
                Result = (currentValue / 100).ToString();
            }
        }

        /// <summary>
        /// Vypočítá převrácenou hodnotu aktuálního vstupu (1/x)
        /// </summary>
        public void ReciprocalOperation()
        {
            double currentValue = ParseCurrentValue();

            if (currentValue != 0)
            {
                Result = (1 / currentValue).ToString();
            }
            else
            {
                Result = "Nelze dělit nulou";
                isNewInput = true;
            }
        }

        /// <summary>
        /// Vypočítá druhou mocninu aktuálního vstupu (x²)
        /// </summary>
        public void SquareOperation()
        {
            double currentValue = ParseCurrentValue();
            Result = (currentValue * currentValue).ToString();
        }

        /// <summary>
        /// Vypočítá druhou odmocninu aktuálního vstupu (√x)
        /// </summary>
        public void SquareRootOperation()
        {
            double currentValue = ParseCurrentValue();

            if (currentValue >= 0)
            {
                Result = Math.Sqrt(currentValue).ToString();
            }
            else
            {
                Result = "Neplatná operace";
                isNewInput = true;
            }
        }

        /// <summary>
        /// Nastaví aritmetickou operaci (+, -, ×, ÷)
        /// </summary>
        /// <param name="operation">Operace k nastavení</param>
        public void SetOperation(string operation)
        {
            if (currentOperation != "" && !isNewInput)
            {
                CalculateResult();
            }

            firstNumber = ParseCurrentValue();
            currentOperation = operation;
            isNewInput = true;
        }

        /// <summary>
        /// Provede výpočet na základě nastavené operace
        /// </summary>
        public void CalculateResult()
        {
            if (currentOperation == "")
                return;

            secondNumber = ParseCurrentValue();
            double result = 0;

            switch (currentOperation)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "−":
                    result = firstNumber - secondNumber;
                    break;
                case "×":
                    result = firstNumber * secondNumber;
                    break;
                case "÷":
                    if (secondNumber != 0)
                    {
                        result = firstNumber / secondNumber;
                    }
                    else
                    {
                        Result = "Nelze dělit nulou";
                        isNewInput = true;
                        return;
                    }
                    break;
            }

            Result = result.ToString();
            firstNumber = result;
            currentOperation = "";
            isNewInput = true;
        }

        private double ParseCurrentValue()
        {
            if (double.TryParse(Result, out double value))
            {
                return value;
            }
            return 0;
        }
    }
}
