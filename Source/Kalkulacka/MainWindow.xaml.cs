using System.Windows;

namespace SimpleCalculator
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// V MVVM vzoru je kód-behind minimální, většina funkcionality je ve ViewModelu
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
