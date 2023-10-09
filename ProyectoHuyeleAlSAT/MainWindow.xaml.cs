using ProyectoHuyeleAlSAT.Models;
using ProyectoHuyeleAlSAT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoHuyeleAlSAT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = (MainWindowViewModel)this.DataContext;
            vm.GenerarMapa();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            vm.cuadritos[vm.jugador.Col, vm.jugador.Ren].Fill = Brushes.White;
            switch (e.Key)
            {
                case Key.Left:
                    vm.jugador.Ren -= 1;
                    break;
                case Key.Up:
                    vm.jugador.Col -= 1;
                    break;
                case Key.Right:
                    vm.jugador.Ren += 1;
                    break;
                case Key.Down:
                    vm.jugador.Col += 1;
                    break;
                default:
                    break;
            }
            vm.cuadritos[vm.jugador.Col, vm.jugador.Ren].Fill = Brushes.Green;
        }
    }
}