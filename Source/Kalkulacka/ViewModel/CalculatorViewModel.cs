using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SimpleCalculator.Model;

namespace SimpleCalculator.ViewModel
{
    /// <summary>
    /// ViewModel kalkulačky propojující UI s business logikou
    /// </summary>
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private readonly Calculator _calculator;

        public CalculatorViewModel()
        {
            _calculator = new Calculator();

            // Inicializace příkazů
            NumberCommand = new RelayCommand<string>(ExecuteNumberCommand);
            OperationCommand = new RelayCommand<string>(ExecuteOperationCommand);
            EqualsCommand = new RelayCommand(ExecuteEqualsCommand);
            ClearCommand = new RelayCommand(ExecuteClearCommand);
            ClearEntryCommand = new RelayCommand(ExecuteClearEntryCommand);
            BackspaceCommand = new RelayCommand(ExecuteBackspaceCommand);
            DecimalPointCommand = new RelayCommand(ExecuteDecimalPointCommand);
            ToggleSignCommand = new RelayCommand(ExecuteToggleSignCommand);
        }

        /// <summary>
        /// Text zobrazený na displeji kalkulačky
        /// </summary>
        public string DisplayText
        {
            get { return _calculator.Result; }
            private set { OnPropertyChanged(); }
        }

        // Příkazy pro UI
        public ICommand NumberCommand { get; }
        public ICommand OperationCommand { get; }
        public ICommand EqualsCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand ClearEntryCommand { get; }
        public ICommand BackspaceCommand { get; }
        public ICommand DecimalPointCommand { get; }
        public ICommand ToggleSignCommand { get; }

        // Metody provádějící příkazy
        private void ExecuteNumberCommand(string digit)
        {
            _calculator.AppendDigit(digit);
            UpdateDisplayText();
        }

        private void ExecuteOperationCommand(string operation)
        {
            switch (operation)
            {
                case "%":
                    _calculator.PercentOperation();
                    break;
                case "¹/ₓ":
                    _calculator.ReciprocalOperation();
                    break;
                case "x²":
                    _calculator.SquareOperation();
                    break;
                case "²√x":
                    _calculator.SquareRootOperation();
                    break;
                default:
                    _calculator.SetOperation(operation);
                    break;
            }
            UpdateDisplayText();
        }

        private void ExecuteEqualsCommand()
        {
            _calculator.CalculateResult();
            UpdateDisplayText();
        }

        private void ExecuteClearCommand()
        {
            _calculator.Clear();
            UpdateDisplayText();
        }

        private void ExecuteClearEntryCommand()
        {
            _calculator.ClearEntry();
            UpdateDisplayText();
        }

        private void ExecuteBackspaceCommand()
        {
            _calculator.Backspace();
            UpdateDisplayText();
        }

        private void ExecuteDecimalPointCommand()
        {
            _calculator.AppendDecimalPoint();
            UpdateDisplayText();
        }

        private void ExecuteToggleSignCommand()
        {
            _calculator.ToggleSign();
            UpdateDisplayText();
        }

        private void UpdateDisplayText()
        {
            OnPropertyChanged(nameof(DisplayText));
        }

        // Implementace INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Jednoduchá implementace ICommand pro MVVM návrhový vzor
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    /// <summary>
    /// Generická implementace ICommand s parametrem
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
