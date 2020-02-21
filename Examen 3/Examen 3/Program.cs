using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace Examen_3
{
    class Program
    {
        Boolean termino = false, chequeo = false, first;
        List<int> CF8 = new List<int>();
        public List<int> CordX = new List<int>();
        public List<int> CordY = new List<int>();
        public List<int> PCordX = new List<int>();
        public List<int> PCordY = new List<int>();
        int g = 0, h = 0;
        public int[,] matrix = new int[128, 242]; public int[,] matrixt = new int[128, 242]; public int[,] contorno = new int[128, 242]; public int[,] original;
        public double[] cmI, cmO;
        int[,] etiquetas = new int[242, 242];
        public int centroXO = 0, centroYO = 0, centroXI = 0, centroYI = 0, difCMX = 0, difCMY = 0;
        private int etiqueta = 1;

        static void Main(string[] args)
        {
            Program objeto = new Program();
            
            objeto.matrix = objeto.llenado("Equipo3.png");
            objeto.matrix = objeto.silueta();
            for (int i = 2; i < 4; i++)
            {
                objeto.buscar(objeto.matrix);
                if (objeto.chequeo == true)
                    objeto.Recursividad(objeto.g, objeto.h, i);
            }

            //objeto.imprimir();
            //objeto.CompConec();
            objeto.Resultados();
            Console.ReadLine();
            Console.ReadLine();
        }

        public int[,] llenado(string archivo)
        {

            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));
            int height = 110, width = 110, promedio;
            if (archivo == "Equipo3.png")
            {
                height = 242; width = 242;
            }
            int[,] mat = new int[height, width];
            double funcion;
            for (int i = 0; i < b1.Width; i++)
                for (int j = 0; j < b1.Height; j++)
                {
                    funcion = ((.2125 * b1.GetPixel(i, j).R) + (.7154 * b1.GetPixel(i, j).G) + (.0721 * b1.GetPixel(i, j).B));
                    promedio = Convert.ToInt32(funcion);
                    if (promedio < 220)
                        mat[j, i] = 1;
                    else
                        mat[j, i] = 0;
                }
            return mat;
        }

        public void imprimir()
        {
            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 242; j++)
                    Console.Write(matrix[i, j]);
                Console.Write("\n");
            }
            Console.ReadLine();

        }

        public void erosion()
        {
            Boolean cheque = true;
            for (int i = 0; i < 953; i++)
            {
                for (int j = 0; j < 660; j++)
                {
                    cheque = true;
                    if (matrixt[i, j] == 1)
                    {
                        matrix[i, j] = 1;
                        //Erosion en n4
                        if (i != 0)
                            if (matrixt[i - 1, j] != 1)
                                cheque = false;
                        if (i != 952)
                            if (matrixt[i + 1, j] != 1)
                                cheque = false;
                        if (j != 0)
                            if (matrixt[i, j - 1] != 1)
                                cheque = false;
                        if (j != 659)
                            if (matrixt[i, j + 1] != 1)
                                cheque = false;
                        if (cheque == false)
                            matrix[i, j] = 0;
                    }
                }
            }
        }

        public void CompConec()
        {
            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < matrix.GetLength(1) - 1; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        etiquetas[i, j] = etiqueta;
                        etiqueta++;
                    }
                }
            }

            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < matrix.GetLength(1) - 1; j++)
                {
                    checar8(i, j);
                }
            }
            for (int j = matrix.GetLength(0) - 1; j >= 0; j--)
            {
                for (int i = matrix.GetLength(1) - 1; i >= 0; i--)
                {
                    checar8(j, i);
                }
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j] = etiquetas[i, j];

        }

        public void checar8(int y, int x)
        {
            int aX, aY;
            if (ValidacionM(y, x) && matrix[y, x] == 1)
            {
                aX = x + 1;
                //Derecha
                if (ValidacionM(y, aX) && matrix[y, aX] == 1)
                {
                    if (etiquetas[y, x] < etiquetas[y, aX])
                    {
                        etiquetas[y, aX] = etiquetas[y, x];
                    }
                }
                aY = y + 1;
                //Abajo Derecha
                if (ValidacionM(aY, aX) && matrix[aY, aX] == 1)
                {
                    if (etiquetas[y, x] < etiquetas[aY, aX])
                    {
                        etiquetas[aY, aX] = etiquetas[y, x];
                    }
                }
                //Abajo
                if (ValidacionM(aY, x) && matrix[aY, x] == 1)
                {
                    if (etiquetas[y, x] < etiquetas[aY, x])
                    {
                        etiquetas[aY, x] = etiquetas[y, x];
                    }
                }
                aX = x - 1;
                //Abajo Izquierda
                if (ValidacionM(aY, aX) && matrix[aY, aX] == 1)
                {
                    if (etiquetas[y, x] < etiquetas[aY, aX])
                    {
                        etiquetas[aY, aX] = etiquetas[y, x];
                    }
                }
                //Izquierda
                if (ValidacionM(y, aX) && matrix[y, aX] == 1)
                {
                    if (etiquetas[y, x] < etiquetas[y, aX])
                    {
                        etiquetas[y, aX] = etiquetas[y, x];
                    }
                }
                aY = y - 1;
                //Arriba Izquierda
                if (ValidacionM(aY, aX) && matrix[aY, aX] == 1)
                {
                    if (etiquetas[y, x] < etiquetas[aY, aX])
                    {
                        etiquetas[aY, aX] = etiquetas[y, x];
                    }
                }
                //Arriba
                if (ValidacionM(aY, x) && matrix[aY, x] == 1)
                {
                    if (etiquetas[y, x] < etiquetas[aY, x])
                    {
                        etiquetas[aY, x] = etiquetas[y, x];
                    }
                }
                aX = x + 1;
                //Arriba Derecha
                if (ValidacionM(aY, aX) && matrix[aY, aX] == 1)
                {
                    if (etiquetas[y, x] < etiquetas[aY, aX])
                    {
                        etiquetas[aY, aX] = etiquetas[y, x];
                    }
                }
            }
        }

        public bool ValidacionM(int y, int x)
        {
            if (x < 0 || y < 0)
                return false;

            else if (x >= matrix.GetLength(1) || y >= matrix.GetLength(0))
                return false;

            else
                return true;
        }

        public bool ValidacionF(int y, int x)
        {
            if (x < 0 || y < 0)
                return false;

            else if (x >= 110 || y >= 110)
                return false;

            else
                return true;
        }

        public int[] Tags()
        {
            int n = 0;
            List<int> etiquetas = new List<int>();
            n = Convert.ToInt32(matrix[0, 0]);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != n && matrix[i, j] != 0 && !etiquetas.Contains(Convert.ToInt32(matrix[i, j])))
                    {
                        etiquetas.Add(Convert.ToInt32(matrix[i, j]));
                        n = Convert.ToInt32(matrix[i, j]);
                    }
                }
            }
            int[] et = etiquetas.ToArray<int>();
            return et;
        }

        public int[,] ventanas(int[] comp, int[,] c, int ancho, int alto, int indice)
        {
            //Y minima
            Console.Write("\n" + comp[indice] + "\n");
            int ymin = 0;
            for (int i = 0; i < ancho; i++)
                for (int j = 0; j < alto; j++)
                    if (c[i, j] == comp[indice])
                    {
                        ymin = i;
                        break;
                        i = ancho;
                    }
            //Y maxima
            int ymax = 0;
            for (int i = ancho - 1; i >= 0; i--)
                for (int j = alto - 1; j >= 0; j--)
                    if (c[i, j] == comp[indice])
                    {
                        ymax = i;
                        break;
                        i = -1;
                    }

            //X maxima
            int xmax = 0;
            for (int i = 0; i < alto; i++)
                for (int j = 0; j < ancho; j++)
                    if (c[j, i] == comp[indice])
                    {
                        xmax = i;
                        break;
                        i = alto;
                    }

            //X Minima
            int xmin = 0;
            for (int i = alto - 1; i >= 0; i--)
                for (int j = ancho - 1; j >= 0; j--)
                    if (c[j, i] == comp[indice])
                    {
                        xmin = i;
                        break;
                        i = -1;
                    }

            //Console.Write("\n\n" + "X maxima: " + xmax + "\nX minima: " + xmin + "\nY minima " + ymin + "\nY maxima " + ymax);

            //Guardado de Matrices
            int[,] objeto = new int[110, 110];
            int difAlt = 110 - (ymin - ymax);
            int difAnch = 110 - (xmax - xmin);
            for (int i = 0; i < 110; i++)
                for (int j = 0; j < 110; j++)
                {
                    if (ymax + i < ancho && xmin + j < alto)
                        if (c[ymax + i, xmin + j] == comp[indice])
                            objeto[i, j] = 1;
                        else
                            objeto[i, j] = 0;
                }


            return objeto;

        }

        public int[,] silueta()
        {
            for (int i = 0; i < 128; i++)
                for (int j = 0; j < 242; j++)
                      matrixt[i, j] = 0;

                for (int i = 0; i < 128; i++)
                {
                    for (int j = 0; j < 242; j++)
                    {
                        if (matrix[i, j] == 1)
                        {
                            if (i != 0)
                                if (matrix[i - 1, j] == 0)
                                    matrixt[i, j] = 1;
                            if (j != 241)
                            {
                                /*if (i != 0)
                                    if (matrix[i - 1, j + 1] == 0)
                                        matrixt[i, j] = 1;*/
                                if (matrix[i, j + 1] == 0)
                                    matrixt[i, j] = 1;
                                /*if (i != 127)
                                    if (matrix[i + 1, j + 1] == 0)
                                        matrixt[i, j] = 1;*/
                            }
                            if (i != 127)
                                if (matrix[i + 1, j] == 0)
                                    matrixt[i, j] = 1;
                            if (j != 0)
                            {
                                /*if (i != 127)
                                    if (matrix[i + 1, j - 1] == 0)
                                        matrixt[i, j] = 1;*/
                                if (matrix[i, j - 1] == 0)
                                    matrixt[i, j] = 1;
                                /*if (i != 0)
                                if (matrix[i - 1, j - 1] == 0)
                                    matrixt[i, j] = 1;*/
                        }
                    }
                    }
                }
            return matrixt;
        }

        public void buscar(int[,] m)
        {
            termino = false;
                for (int i = 0; i < m.GetLength(0); i++)
                    for (int j = 0; j < m.GetLength(1); j++)
                    {
                        if (m[i, j] == 1)
                        {
                            g = i;
                            h = j;
                            i = m.GetLength(0) + 200;
                            chequeo = true;
                            break;
                        }
                        chequeo = false;
                    }

        }

        public void Recursividad(int x, int y, int n)
        {
            matrix[x, y] = n;
            if (x != 0)
            {
                if (matrix[x - 1, y] == 1)
                {
                    //Console.Write("Arriba\n");
                    Recursividad((x - 1), y, n);
                }
                if (y != 241)
                    if (matrix[x - 1, y + 1] == 1)
                    {
                        //Console.Write("Arriba derecha\n");
                        Recursividad((x - 1), (y + 1), n);
                    }
            }
            if (y != 241)
                if (matrix[x, y + 1] == 1)
                {
                    //Console.Write("Derecha\n");
                    Recursividad(x, (y + 1), n);
                }
            if (x != 127)
            {
                if (y != 241)
                    if (matrix[x + 1, y + 1] == 1)
                    {
                        //Console.Write("Abajo derecha\n");
                        Recursividad((x + 1), (y + 1), n);
                    }
                if (matrix[x + 1, y] == 1)
                {
                    //Console.Write("Abajo\n");
                    Recursividad((x + 1), y, n);
                }
            }
            if (y != 0)
            {
                if (x != 127)
                    if (matrix[x + 1, y - 1] == 1)
                    {
                        //Console.Write("Abajo izquierda\n");
                        Recursividad((x + 1), (y - 1), n);
                    }
                if (matrix[x, y - 1] == 1)
                {
                    //Console.Write("Izquierda\n");
                    Recursividad(x, (y - 1), n);
                }
                if (x != 0)
                    if (matrix[x - 1, y - 1] == 1)
                    {
                        //Console.Write("Arriba Izquierda\n");
                        Recursividad((x - 1), (y - 1), n);
                    }
            }
        }

        public void F8(int[,] mat, int x, int y)
        {
            int xmax = mat.GetLength(0);
            int ymax = mat.GetLength(1);

            if (x == g && y == h && first == true)
            {
                chequeo = true;
                return;
            }
            CordX.Add(x);
            CordY.Add(y);
            first = false;

            if (x != 0)
            {
                if (mat[x - 1, y] == 1)
                {
                    //Console.Write("Arriba\n");
                    mat[x - 1, y] = 2;
                    CF8.Add(6);
                    F8(mat, (x - 1), y);
                    if (chequeo == true)
                        return;
                }
            }

            if (y != ymax)
                if (mat[x, y + 1] == 1)
                {
                    //Console.Write("Derecha\n");
                    mat[x, y + 1] = 2;
                    CF8.Add(0);
                    F8(mat, x, (y + 1));
                    if (chequeo == true)
                        return;
                }

            if (x != xmax)
            {
                if (mat[x + 1, y] == 1)
                {
                    //Console.Write("Abajo\n");
                    mat[x + 1, y] = 2;
                    CF8.Add(2);
                    F8(mat, (x + 1), y);
                    if (chequeo == true)
                        return;
                }
            }

            if (y != 0)
            {
                if (mat[x, y - 1] == 1)
                {
                    //Console.Write("Izquierda\n");
                    mat[x, y - 1] = 2;
                    CF8.Add(4);
                    F8(mat, x, (y - 1));
                    if (chequeo == true)
                        return;
                }
            }

            if (x != xmax)
            {
                if (y != ymax)
                    if (mat[x + 1, y + 1] == 1)
                    {
                        //Console.Write("Abajo derecha\n");
                        mat[x + 1, y + 1] = 2;
                        CF8.Add(1);
                        F8(mat, (x + 1), (y + 1));
                        if (chequeo == true)
                            return;
                    }
            }

            if (y != 0)
            {
                if (x != xmax)
                    if (mat[x + 1, y - 1] == 1)
                    {
                        //Console.Write("Abajo izquierda\n");
                        mat[x + 1, y - 1] = 2;
                        CF8.Add(3);
                        F8(mat, (x + 1), (y - 1));
                        if (chequeo == true)
                            return;
                    }

                if (x != 0)
                    if (mat[x - 1, y - 1] == 1)
                    {
                        //Console.Write("Arriba Izquierda\n");
                        mat[x - 1, y - 1] = 2;
                        CF8.Add(5);
                        F8(mat, (x - 1), (y - 1));
                        if (chequeo == true)
                            return;
                    }
            }

            if (x != 0)
            {
                if (y != ymax)
                    if (mat[x - 1, y + 1] == 1)
                    {
                        //Console.Write("Arriba derecha\n");
                        mat[x - 1, y + 1] = 2;
                        CF8.Add(7);
                        F8(mat, (x - 1), (y + 1));
                        if (chequeo == true)
                            return;
                    }
            }
        }

        public char[] AF8(int[] vec)
        {
            char[] res = new char[vec.Length];
            if (vec[0] > vec[res.Length - 1])
                res[0] = (char)(97 + (vec[0] - vec[res.Length - 1]));
            else
                res[0] = (char)(97 + (8 - (vec[res.Length - 1] - vec[0])));

            for (int i = 1; i < res.Length; i++)
            {
                if (vec[i] > vec[i - 1])
                    res[i] = (char)(97 + (vec[i] - vec[i - 1]));
                else if (vec[i] == vec[i - 1])
                    res[i] = 'a';
                else
                    res[i] = (char)(97 + (8 - (vec[i - 1] - vec[i])));
            }

            Console.Write("\nAF8 = {");
            for (int i = 0; i < res.Length; i++)
            {
                Console.Write(((int)res[i] - 97 )+ ",");
            }
            Console.Write("}");

            Console.Write("\n\nCDA = {");
            for (int i = 0; i < res.Length; i++)
            {
                Console.Write(res[i]);
            }
            Console.Write("}");
            return res;
        }

        public void ISE(int[][] Coords, int[][] CoordsPC)
        {
            double ise = 0, e = 0, d2 = 0;
            double temp;
            int k = 0;
            for (int i = 0; i < Coords[0].GetLength(0); i++)
            {
                if (k > CoordsPC[0].GetLength(0) - 2)
                {
                    if ((Coords[0][i] == CoordsPC[0][0] && Coords[1][i] == CoordsPC[1][0]) || i == Coords[0].GetLength(0) - 1)
                    {
                        k++;
                        i--;
                        e = d2;
                        d2 = 0;
                        ise += Math.Pow(e, 2);
                        i = Coords[0].GetLength(0);
                    }
                    else
                    {
                        //Console.Write(k + " " + i + "\n");
                        temp = (((Coords[0][i] - CoordsPC[0][k]) * (CoordsPC[1][0] - CoordsPC[1][k])) - ((Coords[1][i] - CoordsPC[1][k]) * (CoordsPC[0][0] - CoordsPC[0][k])));
                        d2 = Math.Pow(temp, 2);
                        temp = Math.Pow((CoordsPC[0][k] - CoordsPC[0][0]), 2);
                        temp += Math.Pow((CoordsPC[1][k] - CoordsPC[1][0]), 2);
                        d2 = d2 / temp;
                    }
                }
                else
                {

                    if (Coords[0][i] == CoordsPC[0][k + 1] && Coords[1][i] == CoordsPC[1][k + 1])
                    {
                        k++;
                        i--;
                        e = d2;
                        d2 = 0;
                        ise += Math.Pow(e, 2);
                    }
                    else
                    {
                        temp = (((Coords[0][i] - CoordsPC[0][k]) * (CoordsPC[1][k + 1] - CoordsPC[1][k])) - ((Coords[1][i] - CoordsPC[1][k]) * (CoordsPC[0][k + 1] - CoordsPC[0][k])));
                        d2 = Math.Pow(temp, 2);
                        temp = Math.Pow((CoordsPC[0][k] - CoordsPC[0][k + 1]), 2);
                        temp += Math.Pow((CoordsPC[1][k] - CoordsPC[1][k + 1]), 2);
                        d2 = d2 / temp;
                    }
                }
            }
            Console.Write("El valor de error cuadrado integral(ISE) es de: " + ise);
        }

        public void BMP(int[][] CoordsPC, int[,] mat, int[][] Coords)
        {
            int height = mat.GetLength(0), width = mat.GetLength(1);
            Bitmap b1 = new Bitmap(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    b1.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                }
            }

            for (int i = 0; i < Coords[0].GetLength(0); i++)
            {
                b1.SetPixel(Coords[0][i], Coords[1][i], Color.Black);
            }

            for (int i = 0; i < CoordsPC[0].GetLength(0); i++)
            {
                b1.SetPixel(CoordsPC[0][i], CoordsPC[1][i], Color.Red);
            }

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string direccion = Path.Combine(System.IO.Path.GetDirectoryName(path), "PC.png");
            b1.Save(direccion);
        }

        public void PC(char[] cad, int index, int[][] coords)
        {
            int contA = 0, contBH = 0, contHB = 0, contAf = 0;
            bool bh = false, hb = false, aa = false; ;
            String P, Q, R;
            int p, q, r;
            if (index == 0)
            {
                Console.WriteLine("Ingresa los siguientes valores\n(P = 9, Q = 10, R = 11)");
                Console.Write("p: ");
                P = Console.ReadLine();
                p = int.Parse(P);
                while (p > 9)
                {
                    Console.Write("p: ");
                    P = Console.ReadLine();
                    p = int.Parse(P);
                }
                Console.Write("q: ");
                Q = Console.ReadLine();
                q = int.Parse(Q);
                while (q > 10)
                {
                    Console.Write("q: ");
                    Q = Console.ReadLine();
                    q = int.Parse(Q);
                }
                Console.Write("r: ");
                R = Console.ReadLine();
                r = int.Parse(R);
                while (r > 11)
                {
                    Console.Write("r: ");
                    R = Console.ReadLine();
                    r = int.Parse(R);
                }
            }
            else
            {
                Console.WriteLine("\nIngresa los siguientes valores\n(P = 13, Q = 7, R = 4)");
                Console.Write("p: ");
                P = Console.ReadLine();
                p = int.Parse(P);
                while (p > 13)
                {
                    Console.Write("p: ");
                    P = Console.ReadLine();
                    p = int.Parse(P);
                }
                Console.Write("q: ");
                Q = Console.ReadLine();
                q = int.Parse(Q);
                while (q > 7)
                {
                    Console.Write("q: ");
                    Q = Console.ReadLine();
                    q = int.Parse(Q);
                }
                Console.Write("r: ");
                R = Console.ReadLine();
                r = int.Parse(R);
                while (r > 4)
                {
                    Console.Write("r: ");
                    R = Console.ReadLine();
                    r = int.Parse(R);
                }
            }

            for (int i = 0; i < cad.Length; i++)
            {
                if (cad[i] == 'a' && (i == 1 || i == 0))
                {
                    //bh = false; hb = false;
                    contA++;
                    contBH = 0;
                    contHB = 0;
                    contAf = 0;
                    if (contA > p)
                    {
                        Console.Write(" a " + i);
                        PCordX.Add(coords[0][i]);
                        PCordY.Add(coords[1][i]);
                        contA = 1;
                    }
                }
                else if (cad[i] == 'a' && ((cad[i - 1] != 'b' && cad[i - 2] != 'h') || (cad[i - 1] != 'h' && cad[i - 2] != 'b')) && aa == false)
                {
                    //bh = false; hb = false;
                    contA++;
                    contBH = 0;
                    contHB = 0;
                    contAf = 0;
                    if (contA > p)
                    {
                        Console.Write(" a " + i);
                        PCordX.Add(coords[0][i]);
                        PCordY.Add(coords[1][i]);
                        contA = 1;
                    }
                }
                else if (cad[i] == 'b' && cad[i + 1] == 'h')
                {
                    bh = true;
                    contA = 0; contHB = 0; contAf = 0;
                    contBH++;
                    if (contBH > r || hb == true)
                    {
                        Console.Write(" b " + i);
                        PCordX.Add(coords[0][i]);
                        PCordY.Add(coords[1][i]);
                        contBH = 1;
                    }
                    else
                    {
                        i++;
                    }
                    hb = false;
                    aa = false;
                }

                //else if (cad[i] == 'h' && cad[i - 1] == 'b') { }

                else if (cad[i] == 'h' && cad[i + 1] == 'b')
                {
                    hb = true;
                    contA = 0; contBH = 0; contAf = 0;
                    contHB++;
                    if (contHB > r || bh == true)
                    {
                        Console.Write(" h " + i);
                        PCordX.Add(coords[0][i]);
                        PCordY.Add(coords[1][i]);
                        contHB = 1;
                    }
                    else
                    {
                        i++;
                    }
                    bh = false;
                    aa = false;
                }

                //else if (cad[i] == 'b' && cad[i - 1] == 'h') { }

                else if (cad[i] == 'a')
                {
                    aa = true;
                    contAf++;
                    contA = 0;
                    if (contAf > q)
                    {
                        Console.Write(" a " + i);
                        PCordX.Add(coords[0][i]);
                        PCordY.Add(coords[1][i]);
                        contBH = 0; contHB = 0;
                        contAf = 1;
                    }
                }
                else
                {
                    Console.Write(" " + cad[i] + " " + i);
                    PCordX.Add(coords[0][i]);
                    PCordY.Add(coords[1][i]);
                    bh = false;
                    hb = false;
                    aa = false;
                    contA = 0;
                    contAf = 0;
                    contBH = 0;
                    contHB = 0;
                }
            }
            Console.WriteLine();
        }

        public void BuscarPQR(char[] cadena)
        {
            int contA = 0, contBH = 0, contHB = 0, contAf = 0;
            for (int i = 0; i < cadena.Length; i++)
            {
                if (cadena[i] == 'a' && (i == 1 || i == 0))
                {
                    //bh = false; hb = false;
                    contA++;
                    contBH = 0;
                    contHB = 0;
                    contAf = 0;
                    if (cadena[i] == 'b' && (cadena[i] == 'h'))
                    {
                        contBH++;
                        contA = 0;
                    }
                    if (cadena[i] == 'h' && (cadena[i] == 'b'))
                    {
                        contHB++;
                        contA = 0;
                    }
                }
                else if(cadena[i] == 'a' || cadena[i-1] == 'b')
                {
                    contAf++;
                    contA = 0;
                }
            }
        }

        public void Resultados()
        {
            int[,] elemento = new int[110, 110];
            int[] componentes = Tags();
            int[,] figura;
            for (int i = 0; i <= componentes.Length - 2; i++)
            {
                figura = ventanas(componentes, matrix, matrix.GetLength(0), matrix.GetLength(1), i);
                for (int k = 0; k < figura.GetLength(0); k++)
                {
                    for (int j = 0; j < figura.GetLength(1); j++)
                        Console.Write(figura[k, j]);
                    Console.Write("\n");
                }
                
                buscar(figura);
                Console.WriteLine(g + " " + h);
                F8(figura, g, h);
                int[] Cadena = CF8.ToArray();
                CordX.RemoveAt(CordX.Count - 1);
                CordY.RemoveAt(CordY.Count - 1);
                int[] Xs = CordX.ToArray();
                int[] Ys = CordY.ToArray();
                /*for (int l = 0; l < Cadena.GetLength(0); l++)
                {
                    Console.Write(Cadena[l] + " " + Xs[l] + " " + Ys[l] + "\n");
                }*/
                int[][] coor = new int[2][];
                coor[0] = Xs;
                coor[1] = Ys;

                Console.Write("F8 = {");
                for (int l = 0; l < Cadena.GetLength(0); l++)
                {
                    Console.Write(Cadena[l] + ",");
                }
                Console.Write("}\n");
                char[] cad = AF8(Cadena);
                //BuscarPQR(cad);
                PC(cad, i, coor);
                Xs = PCordX.ToArray();
                Ys = PCordY.ToArray();
                int[][] PJ = new int[2][];
                PJ[0] = Xs;
                PJ[1] = Ys;
                BMP(PJ, figura, coor);
                ISE(coor, PJ);
                Console.ReadLine();
                CF8.Clear();
                CordX.Clear();
                CordY.Clear();
                PCordX.Clear();
                PCordY.Clear();
                /*string pattern = "(Mr\\.? |Mrs\\.? |Miss |Ms\\.? )";
                string[] names = { "Mr. Henry Hunt", "Ms. Sara Samuels",
                         "Abraham Adams", "Ms. Nicole Norris" };
                foreach (string name in names)
                    Console.WriteLine(Regex.Replace(name, pattern, String.Empty));*/
            }
        }
    }
}
