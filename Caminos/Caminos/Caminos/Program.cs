using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caminos
{
    class Program
    {
        public int[,] camino = new int[9, 17];
        int x = 0, y = 0;
        Boolean termino = false, chequeo = false;
        public void llenar()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 17; j++)
                    camino[i, j] = 0;
            camino[1, 2] = camino[1, 3] = camino[1, 6] = camino[1, 7] = camino[1, 10] = camino[1, 11] = camino[1, 14] = camino[1, 15] = 1;
            camino[2, 1] = camino[2, 2] = camino[2, 3] = camino[2, 4] = camino[2, 5] = camino[2, 6] = camino[2, 7] = camino[2, 8] = camino[2, 11] = camino[2, 12] = camino[2, 13] = camino[2, 14] = 1;
            camino[3, 3] = camino[3, 4] = camino[3, 5] = camino[3, 6] = camino[3, 10] = camino[3, 11] = camino[3, 12] = camino[3, 13] = 1;
            camino[4, 2] = camino[4, 3] = camino[4, 4] = camino[4, 5] = camino[4, 9] = camino[4, 10] = camino[4, 11] = camino[4, 14] = camino[4, 15] = 1;
            camino[5, 1] = camino[5, 2] = camino[5, 3] = camino[5, 6] = camino[5, 7] = camino[5, 11] = camino[5, 12] = camino[5, 13] = 1;
            camino[6, 2] = camino[6, 3] = camino[6, 9] = camino[6, 10] = camino[6, 14] = camino[6, 15] = 1;
            camino[7, 6] = camino[7, 7] = camino[7, 8] = camino[7, 9] = camino[7, 12] = camino[7, 13] = camino[7, 14] = camino[7, 15] = 1;
        }

        public void imprimir()
        {
            Console.Write("  ");
            for (int j = 0; j < 17; j++)
            {
                if (j < 10)
                    Console.Write(" " + j + " ");
                else
                    Console.Write(j + " ");
            }
            Console.Write("\n");
            for (int i = 0; i < 9; i++)
            {
                Console.Write(i + " ");
                for (int j = 0; j < 17; j++)
                {
                    System.Console.ForegroundColor = ConsoleColor.White;
                    if (camino[i, j] == 1)
                        System.Console.ForegroundColor = ConsoleColor.Blue;
                    if (camino[i, j] == 2)
                        System.Console.ForegroundColor = ConsoleColor.Red;
                    if (camino[i, j] == 3)
                        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                    if (camino[i, j] == 4)
                        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                    if (camino[i, j] == 5)
                        System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    if (camino[i, j] == 6)
                        System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    if (camino[i, j] == 7)
                        System.Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("███");
                }
                Console.Write("\n");
            }
            //Console.Write(x + " " + y + "\n");
        }

        public void buscar(int n)
        {
            termino = false;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 17; j++)
                    if (camino[i, j] == n)
                    {
                        x = i;
                        y = j;
                        i = 9;
                        chequeo = true;
                        break;
                    }
        }

        public void checar(int n)
        {
            camino[x, y] = n;
            imprimir();
            if (camino[x - 1, y] == 1)
            {
                camino[x - 1, y] = n;
                x = x - 1;
                checar(n);
            }
            if (camino[x, y + 1] == 1)
            {
                camino[x, y + 1] = n;
                y = y + 1;
                checar(n);
            }
            if (camino[x + 1, y] == 1)
            {
                camino[x + 1, y] = n;
                x = x + 1;
                checar(n);
            }
            if (camino[x, y - 1] == 1)
            {
                camino[x, y - 1] = n;
                y = y - 1;
                checar(n);
            }
            if(termino == false)
                wacha(n);
        }

        public void checar8(int n)
        {
            camino[x, y] = n;
            imprimir();
            if (camino[x - 1, y] == 1)
            {
                camino[x - 1, y] = n;
                x = x - 1;
                checar8(n);
            }
            if (camino[x - 1, y + 1] == 1)
            {
                camino[x - 1, y + 1] = n;
                x = x - 1; y = y + 1;
                checar8(n);
            }
            if (camino[x, y + 1] == 1)
            {
                camino[x, y + 1] = n;
                y = y + 1;
                checar8(n);
            }
            if (camino[x + 1, y + 1] == 1)
            {
                camino[x + 1, y + 1] = n;
                x = x + 1; y = y + 1;
                checar8(n);
            }
            if (camino[x + 1, y] == 1)
            {
                camino[x + 1, y] = n;
                x = x + 1;
                checar8(n);
            }
            if (camino[x + 1, y - 1] == 1)
            {
                camino[x + 1, y - 1] = n;
                x = x + 1; y = y - 1;
                checar8(n);
            }
            if (camino[x, y - 1] == 1)
            {
                camino[x, y - 1] = n;
                y = y - 1;
                checar8(n);
            }
            if (camino[x - 1, y - 1] == 1)
            {
                camino[x - 1, y - 1] = n;
                x = x - 1; y = y - 1;
                checar8(n);
            }
            if (termino == false)
                wacha8(n);
        }

        public void wacha(int n)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 17; j++)
                {
                    //Console.Write(i + " " + j + "\n");
                    if (camino[i, j] == n)
                    {
                        if (camino[i - 1, j] == 1)
                        {
                            x = i; y = j;
                            checar(n);
                        }
                        if (camino[i, j + 1] == 1)
                        {
                            x = i; y = j;
                            checar(n);
                        }
                        if (camino[i + 1, j] == 1)
                        {
                            x = i; y = j;
                            checar(n);
                        }
                        if (camino[i, j - 1] == 1)
                        {
                            x = i; y = j;
                            checar(n);
                        }
                    }
                }
            termino = true;
        }

        public void wacha8(int n)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 17; j++)
                {
                    //Console.Write(i + " " + j + "\n");
                    if (camino[i, j] == n)
                    {
                        if (camino[i - 1, j] == 1)
                        {
                            x = i; y = j;
                            checar8(n);
                        }
                        if (camino[i - 1, j + 1] == 1)
                        {
                            x = i; y = j;
                            checar8(n);
                        }
                        if (camino[i, j + 1] == 1)
                        {
                            x = i; y = j;
                            checar8(n);
                        }
                        if (camino[i + 1, j + 1] == 1)
                        {
                            x = i; y = j;
                            checar8(n);
                        }
                        if (camino[i + 1, j] == 1)
                        {
                            x = i; y = j;
                            checar8(n);
                        }
                        if (camino[i + 1, j - 1] == 1)
                        {
                            x = i; y = j;
                            checar8(n);
                        }
                        if (camino[i, j - 1] == 1)
                        {
                            x = i; y = j;
                            checar8(n);
                        }
                        if (camino[i - 1, j - 1] == 1)
                        {
                            x = i; y = j;
                            checar8(n);
                        }
                    }
                }
            termino = true;
        }

        static void Main(string[] args)
        {
            Program objeto = new Program();
            objeto.llenar();
            objeto.imprimir();
            int n = 2,op = 0;
            Console.Write("¿n4 o n8? (Escribe 4 u 8)\n>");
            op = Console.Read();
            if (op == 4)
            {
                do
                {
                    objeto.chequeo = false;
                    objeto.buscar(1);
                    objeto.checar(n);
                    Console.Write("\n");
                    n++;
                } while (objeto.chequeo == true);
            }
            else
            {
                do
                {
                    objeto.chequeo = false;
                    objeto.buscar(1);
                    objeto.checar8(n);
                    Console.Write("\n");
                    n++;
                } while (objeto.chequeo == true);
            }
            Console.Write("\nHasta la proximaaaaa");
            Console.Read();
            Console.Read();
            Console.Read();
            Console.Read();
            Console.Read();
        }
    }
}
