using GalaSoft.MvvmLight.Command;
using ProyectoHuyeleAlSAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProyectoHuyeleAlSAT.ViewModels
{
    public class MainWindowViewModel
    {
        public Rectangle[,] cuadritos;
        List<Nodo> Enemigos;
        public Nodo jugador;
        public ICommand GenerarCommand { get; set; }
        //public ICommand ResolverCommand { get; set; }
        DispatcherTimer timer = new DispatcherTimer();
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        public MainWindowViewModel()
        {
            GenerarCommand = new RelayCommand(GenerarMapa);
            //ResolverCommand = new RelayCommand(Resolver);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) => Resolver(null, jugador);
        }
        //Movimiento enemigo
        private async void Resolver(Nodo enem, Nodo juga)
        {
            AStar astar = new AStar();
            var solucion = await astar.Buscar(enem, juga);
            List<Nodo> aja = solucion.ToList();
            if (aja.Count > 1)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    cuadritos[enem.Col, enem.Ren].Fill = Brushes.White;
                    enem.Ren = aja[1].Ren;
                    enem.Col = aja[1].Col;
                    cuadritos[enem.Col, enem.Ren].Fill = Brushes.Blue;
                });
            }
            if (enem.Col == juga.Col && enem.Ren == juga.Ren)
            {
                mw.txtVictoria.Text = "Se ACABO ESTA FREGADERA";
                timer.Stop();
            }
        }
        private void GenerarMapa()
        {
            timer.Stop();
            mw.tablero.Children.Clear();
            mw.txtVictoria.Text = "";
            int filas = int.Parse(mw.txtFilas.Text);
            int columnas = int.Parse(mw.txtColumnas.Text);
            int obstaculos = int.Parse(mw.txtObstaculos.Text);
            mw.tablero.Rows = filas;
            mw.tablero.Columns = columnas;
            cuadritos = new Rectangle[columnas, filas];
            Nodo.Tablero = new bool[columnas, filas];
            for (int i = 0; i < columnas; i++)
            {
                for (int j = 0; j < filas; j++)
                {
                    cuadritos[i, j] = new Rectangle()
                    {
                        Stroke = Brushes.Black
                    };
                    mw.tablero.Children.Add(cuadritos[i, j]);
                }
            }
            Random r = new Random();
            for (int i = 0; i < obstaculos; i++)
            {
                int fila = r.Next(filas);
                int columna = r.Next(columnas);
                cuadritos[columna, fila].Fill
                    = Brushes.DarkSlateGray;
                Nodo.Tablero[columna, fila] = true;
            }
            jugador = new Nodo();
            do
            {
                int fila = r.Next(filas);
                int columna = r.Next(columnas);
                jugador.Col = columna;
                jugador.Ren = fila;
            } while (Nodo.Tablero[jugador.Col, jugador.Ren]);

            Enemigos = new List<Nodo>();
            for (int i = 0; i < 3; i++)
            {
                int fila = r.Next(filas);
                int columna = r.Next(columnas);
                Enemigos.Add(new Nodo { Col = columna, Ren = fila });
                cuadritos[Enemigos[i].Col, Enemigos[i].Ren].Fill = Brushes.Blue;
            }
            cuadritos[jugador.Col, jugador.Ren].Fill = Brushes.Green;
            timer.Start();
        }
    }
}
