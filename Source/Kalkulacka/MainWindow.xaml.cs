using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleCalculator
{
    public partial class MainWindow : Window
    {
        private double firstNumber = 0;
        private double secondNumber = 0;
        private string currentOperation = "";
        private bool isNewCalculation = true;
        private bool isOperationSelected = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Obsluha kliknutí na číslové tlačítko
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string number = button.Content.ToString();

            if (isNewCalculation || Display.Text == "0" || isOperationSelected)
            {
                Display.Text = number;
                isNewCalculation = false;
                isOperationSelected = false;
            }
            else
            {
                Display.Text += number;
            }
        }

        // Obsluha kliknutí na desetinnou čárku
        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            if (isOperationSelected)
            {
                Display.Text = "0,";
                isOperationSelected = false;
                return;
            }

            if (!Display.Text.Contains(","))
            {
                Display.Text += ",";
            }
        }

        // Obsluha kliknutí na tlačítko pro změnu znaménka
        private void PlusMinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text != "0")
            {
                if (Display.Text.StartsWith("-"))
                {
                    Display.Text = Display.Text.Substring(1);
                }
                else
                {
                    Display.Text = "-" + Display.Text;
                }
            }
        }

        // Obsluha kliknutí na tlačítko pro operace
        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string operation = button.Content.ToString();

            // Provedení okamžitých operací (%, 1/x, x², √x)
            if (operation == "%" || operation == "¹/ₓ" || operation == "x²" || operation == "²√x")
            {
                double currentValue = double.Parse(Display.Text);
                switch (operation)
                {
                    case "%":
                        if (currentOperation != "" && !isNewCalculation)
                        {
                            secondNumber = firstNumber * (currentValue / 100);
                            Display.Text = secondNumber.ToString();
                        }
                        else
                        {
                            Display.Text = (currentValue / 100).ToString();
                        }
                        break;
                    case "¹/ₓ":
                        if (currentValue != 0)
                        {
                            Display.Text = (1 / currentValue).ToString();
                        }
                        else
                        {
                            Display.Text = "Nelze dělit nulou";
                            isNewCalculation = true;
                        }
                        break;
                    case "x²":
                        Display.Text = (currentValue * currentValue).ToString();
                        break;
                    case "²√x":
                        if (currentValue >= 0)
                        {
                            Display.Text = Math.Sqrt(currentValue).ToString();
                        }
                        else
                        {
                            Display.Text = "Neplatná operace";
                            isNewCalculation = true;
                        }
                        break;
                }
            }
            // Standardní aritmetické operace (+, -, ×, ÷)
            else
            {
                if (currentOperation != "" && !isOperationSelected)
                {
                    CalculateResult();
                }

                firstNumber = double.Parse(Display.Text);
                currentOperation = operation;
                isOperationSelected = true;
            }
        }

        // Obsluha kliknutí na tlačítko rovná se
        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentOperation != "")
            {
                CalculateResult();
                currentOperation = "";
                isNewCalculation = true;
            }
        }

        // Výpočet výsledku operace
        private void CalculateResult()
        {
            secondNumber = double.Parse(Display.Text);
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
                        Display.Text = "Nelze dělit nulou";
                        isNewCalculation = true;
                        return;
                    }
                    break;
            }

            Display.Text = result.ToString();
            firstNumber = result;
        }

        // Obsluha kliknutí na tlačítko CE (Clear Entry)
        private void CeButton_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            isOperationSelected = false;
        }

        // Obsluha kliknutí na tlačítko C (Clear)
        private void CButton_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            firstNumber = 0;
            secondNumber = 0;
            currentOperation = "";
            isNewCalculation = true;
            isOperationSelected = false;
        }

        // Obsluha kliknutí na tlačítko Backspace
        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isNewCalculation && !isOperationSelected && Display.Text.Length > 0)
            {
                Display.Text = Display.Text.Length == 1 ? "0" : Display.Text.Substring(0, Display.Text.Length - 1);
            }
        }
    }
}
