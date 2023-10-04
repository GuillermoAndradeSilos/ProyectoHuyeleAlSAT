using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoHuyeleAlSAT.Models
{
    public class AStar
    {
        List<Nodo> abiertos = new();
        List<Nodo> cerrados = new();
        public async Task<IEnumerable<Nodo>> Buscar(Nodo origen, Nodo destino)
        {
            Nodo.ColDest = destino.Col;
            Nodo.RenDest = destino.Ren;
            bool existeRuta = true;
            bool solucionEncontrada = false;
            Nodo mejorNodo = null;
            abiertos.Add(origen);
            do
            {
                if (abiertos.Count == 0)
                {
                    existeRuta = false;
                }
                else
                {
                    mejorNodo = abiertos.OrderBy(n => n.F).First();
                    abiertos.Remove(mejorNodo);
                    cerrados.Add(mejorNodo);
                    Console.WriteLine($"{mejorNodo.Ren},{mejorNodo.Col}");
                    if (mejorNodo.Col != destino.Col || mejorNodo.Ren != destino.Ren)
                    {
                        var sucesores = mejorNodo.GenerarSucesores();
                        foreach (var nodo in sucesores)
                        {
                            Nodo? viejo = abiertos
                                      .FirstOrDefault(n => n.Ren == nodo.Ren
                                                        && n.Col == nodo.Col);
                            if (viejo != null)
                            {
                                if (nodo.G < viejo.G)
                                {
                                    viejo.G = nodo.G;
                                    viejo.Padre = mejorNodo;
                                }
                            }
                            else
                            {
                                viejo = cerrados.FirstOrDefault(
                                        n => n.Ren == nodo.Ren &&
                                        n.Col == nodo.Col
                                    );
                                if (viejo != null)
                                {
                                    if (nodo.G < viejo.G)
                                    {
                                        viejo.Padre = mejorNodo;
                                        viejo.G = nodo.G;
                                        propagarG(viejo);
                                    }
                                }
                                else
                                {
                                    nodo.Padre = mejorNodo;
                                    abiertos.Add(nodo);
                                }
                            }
                        }
                    }
                    else
                    {
                        solucionEncontrada = true;
                    }
                }

            } while (existeRuta && !solucionEncontrada);
            if (solucionEncontrada)
            {
                List<Nodo> solucion = new List<Nodo>();
                solucion.Add(mejorNodo);
                while (mejorNodo.Padre != null)
                {
                    mejorNodo = mejorNodo.Padre;
                    solucion.Add(mejorNodo);
                }
                solucion.Reverse();

                return solucion;
            }
            else
                return Enumerable.Empty<Nodo>();
        }
        void propagarG(Nodo nodo)
        {
            var hijos = abiertos.Where(n => n.Padre == nodo);
            foreach (var hijo in hijos)
            {
                hijo.G = nodo.G + 1;
            }
            hijos = cerrados.Where(n => n.Padre == nodo);
            foreach (var hijo in hijos)
            {
                hijo.G = nodo.G + 1;
                propagarG(hijo);
            }
        }
    }
}
