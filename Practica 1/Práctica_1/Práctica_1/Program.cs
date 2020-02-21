using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Práctica_1
{
    class Program
    {
        //int[nivel, columnas, filas]
        public int[,,] matrix = new int[64, 64, 64];
        public int[,,] matrixt = new int[64, 64, 64];
        Boolean termino = false, chequeo = false;
        public StreamWriter sw,se;
        public int RanR, RanB, RanG;
       
        int f = 0,g=0,h=0;

        static void Main(string[] args)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string dirscr = Path.Combine(System.IO.Path.GetDirectoryName(path), "etiquetado3D.scr");
            Random r = new Random();
            int n = 2;
            Program objeto = new Program();
            objeto.sw = new StreamWriter(dirscr);
            objeto.ObtenerMatriz();
            objeto.silueta();
            objeto.RanB = r.Next(0, 255);
            objeto.RanG = r.Next(0, 255);
            objeto.RanR = r.Next(0, 255);
            objeto.sw.WriteLine("Color");
            objeto.sw.WriteLine("CO");
            objeto.sw.WriteLine(objeto.RanR + "," + objeto.RanG + "," + objeto.RanB);
            do
            {
                objeto.chequeo = false;
                objeto.buscar(1);
                if(objeto.chequeo == true)
                {
                    objeto.checar8(objeto.f,objeto.g,objeto.h,n);
                }
                n++;
                objeto.RanG = r.Next(0, 255);
                objeto.RanB = r.Next(0, 255);
                objeto.RanR = r.Next(0, 255);
                objeto.sw.WriteLine("Color");
                objeto.sw.WriteLine("CO");
                objeto.sw.WriteLine(objeto.RanR + "," + objeto.RanG + "," + objeto.RanB);
            } while (objeto.chequeo == true);
            objeto.imprimir();
            objeto.sw.Close();
            string dirtxt = Path.Combine(System.IO.Path.GetDirectoryName(path), "CC.txt");
            objeto.se = new StreamWriter(dirtxt);
            objeto.se.WriteLine("Componentes Conectadas: " + (n - 3));
            objeto.se.Close();
            Console.ReadLine();
        }

        public void ObtenerMatriz()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string direccion = Path.Combine(System.IO.Path.GetDirectoryName(path), "voxels.txt");
            String input = File.ReadAllText(direccion);
            int a = 0, s = 0;
            String fila;
            foreach (var row in input.Split('\n'))
            {
                foreach (var col in row.Trim().Split(' '))
                {
                    fila = row.Trim();
                    for (int j = 0; j < 64; j++)
                    {
                        matrixt[s, a, j] = int.Parse(fila[j].ToString());
                        System.Console.ForegroundColor = ConsoleColor.White;
                        if (matrixt[s, a, j] == 1)
                            System.Console.ForegroundColor = ConsoleColor.Red;
                    }
                    a++;
                    if (a == 64 && s < 63)
                    {
                        s++; a = 0;
                        System.Console.ForegroundColor = ConsoleColor.Blue;
                    }
                }
            }
        }

        public void silueta()
        {
            for (int k = 0; k < 64; k++)
            {
                for (int i = 0; i < 64; i++)
                {
                    for (int j = 0; j < 64; j++)
                    {
                        matrix[k, i, j] = 0;
                    }
                }
            }
            for (int k = 0; k < 64; k++)
            {
                for (int i = 0; i < 64; i++)
                {
                    for (int j = 0; j < 64; j++)
                    {
                        if(matrixt[k,i,j] == 1)
                        {
                            if (i != 0)
                                if (matrixt[k, i - 1, j] == 0)
                                    matrix[k, i, j] = 1;
                            if (j != 63)
                            {
                                if (i != 0)
                                    if (matrixt[k, i - 1, j + 1] == 0)
                                        matrix[k, i, j] = 1;
                                if (matrixt[k, i, j + 1] == 0)
                                    matrix[k, i, j] = 1;
                                if (i != 63)
                                    if (matrixt[k, i + 1, j + 1] == 0)
                                        matrix[k, i, j] = 1;
                            }
                            if (i != 63)
                                if (matrixt[k, i + 1, j] == 0)
                                    matrix[k, i, j] = 1;
                            if (j != 0)
                            {
                                if (i != 63)
                                    if (matrixt[k, i + 1, j - 1] == 0)
                                        matrix[k, i, j] = 1;
                                if (matrixt[k, i, j - 1] == 0)
                                    matrix[k, i, j] = 1;
                                if (i != 0)
                                    if (matrixt[k, i - 1, j - 1] == 0)
                                        matrix[k, i, j] = 1;
                            }
                            if (k != 63)
                                if (matrixt[k + 1, i, j] == 0)
                                    matrix[k, i, j] = 1;
                            if (k != 0)
                                if (matrixt[k - 1, i, j] == 0)
                                    matrix[k, i, j] = 1;
                        }
                    }
                }
            }
        }

        public void buscar(int n)
        {
            termino = false;
            for(int k = 0; k < 64; k++)
                for (int i = 0; i < 64; i++)
                    for (int j = 0; j < 64; j++)
                    {
                        if (matrix[k, i, j] == n)
                        {
                            g = i;
                            h = j;
                            f = k;
                            i = 64;
                            k = 64;
                            chequeo = true;
                            break;
                        }
                        chequeo = false;
                    }
                    
        }

        public void checar8(int z, int x, int y,int n)
        {
            matrix[z, x, y] = n;
            sw.WriteLine("_box");
            sw.WriteLine("C");
            sw.WriteLine(y + "," + x + "," + z);
            sw.WriteLine("C");
            sw.WriteLine("1");
            if (x != 0)
            {
                if (matrix[z, x - 1, y] == 1)
                {
                    //Console.Write("Arriba\n");
                    checar8(z, (x - 1), y, n);
                }
                if(y != 63)
                    if (matrix[z, x - 1, y + 1] == 1)
                    {
                        //Console.Write("Arriba derecha\n");
                        checar8(z, (x - 1), (y + 1), n);
                    }
            }
            if (y != 63)
                if (matrix[z, x, y + 1] == 1)
                {
                    //Console.Write("Derecha\n");
                    checar8(z, x, (y+1), n);
                }
            if (x != 63)
            {
                if(y != 63)
                    if (matrix[z, x + 1, y + 1] == 1)
                    {
                        //Console.Write("Abajo derecha\n");
                        checar8(z, (x + 1), (y + 1), n);
                    }
                if (matrix[z, x + 1, y] == 1)
                {
                    //Console.Write("Abajo\n");
                    checar8(z, (x + 1), y, n);
                }
            }
            if(y != 0)
            {
                if(x != 63)
                    if (matrix[z, x + 1, y - 1] == 1)
                    {
                        //Console.Write("Abajo izquierda\n");
                        checar8(z, (x + 1), (y - 1), n);
                    }
                if (matrix[z, x, y - 1] == 1)
                {
                    //Console.Write("Izquierda\n");
                    checar8(z, x, (y - 1), n);
                }
                if (x != 0)
                    if (matrix[z, x - 1, y - 1] == 1)
                    {
                        //Console.Write("Arriba Izquierda\n");
                        checar8(z, (x - 1), (y - 1), n);
                    }
            }
            

            if(z != 63)
            {
                if (matrix[z + 1, x, y] == 1)
                {
                    //Console.Write("Encima\n");
                    checar8((z + 1), x, y, n);
                }
                if(x != 0)
                    if (matrix[z + 1, x - 1, y] == 1)
                    {
                        //Console.Write("Arriba E\n");
                        checar8((z + 1), (x - 1), y, n);
                    }
                if (y != 63)
                {
                    if (x != 0)
                        if (matrix[z + 1, x - 1, y + 1] == 1)
                        {
                            //Console.Write("Arriba derecha E\n");
                            checar8((z + 1), (x - 1), (y + 1), n);
                        }
                    if (matrix[z + 1, x, y + 1] == 1)
                    {
                        //Console.Write("Derecha E\n");
                        checar8((z + 1), x, (y + 1), n);
                    }
                    if (x != 63)
                        if (matrix[z + 1, x + 1, y + 1] == 1)
                        {
                            //Console.Write("Abajo derecha E\n");
                            checar8((z + 1), (x + 1), (y + 1), n);
                        }
                }
                if(x != 63)
                    if (matrix[z + 1, x + 1, y] == 1)
                    {
                        //Console.Write("Abajo E\n");
                        checar8((z + 1), (x + 1), y, n);
                    }
                if (y != 0)
                {
                    if(x != 63)
                        if (matrix[z + 1, x + 1, y - 1] == 1)
                        {
                            //Console.Write("Abajo izquierda E\n");
                            checar8((z + 1), (x + 1), (y - 1), n);
                        }
                    if (matrix[z + 1, x, y - 1] == 1)
                    {
                        //Console.Write("Izquierda E\n");
                        checar8((z + 1), x, (y - 1), n);
                    }
                    if(x != 0)
                        if (matrix[z + 1, x - 1, y - 1] == 1)
                        {
                            //Console.Write("Arriba Izquierda E\n");
                            checar8((z + 1), (x - 1), (y - 1), n);
                        }
                }
                    
            }
            
        }
        public void imprimir()
        {
            for (int k = 0; k < 64; k++)
            {
                System.Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n\nCapa No. " + (k + 1) + "\n");
                for (int i = 0; i < 64; i++)
                {
                    if (i < 10)
                        Console.Write(i + "  ");
                    else
                        Console.Write(i + " ");
                    for (int j = 0; j < 64; j++)
                    {
                        System.Console.ForegroundColor = ConsoleColor.White;
                        if (matrix[k, i, j] == 1)
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                        if (matrix[k, i, j] == 2)
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                        if (matrix[k, i, j] == 3)
                            System.Console.ForegroundColor = ConsoleColor.Blue;
                        if (matrix[k, i, j] == 4)
                            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                        if (matrix[k, i, j] == 5)
                            System.Console.ForegroundColor = ConsoleColor.Magenta;
                        if (matrix[k, i, j] == 6)
                            System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                        if (matrix[k, i, j] == 7)
                            System.Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(matrix[k, i, j]);
                    }
                    Console.Write("\n");
                }
            }
        }
    }
}
