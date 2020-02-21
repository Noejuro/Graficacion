using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Examen2
{
    class Program
    {
        public int[,] matrix = new int[953, 953]; public int[,] matrixt = new int[953, 660]; public int[,] original; 
        public double[] cmI, cmO; 
        int[,] etiquetas = new int[953, 953];
        public int centroXO = 0, centroYO = 0, centroXI = 0, centroYI = 0, difCMX = 0, difCMY = 0;
        private int etiqueta = 1;

        static void Main(string[] args)
        {
            Program objeto = new Program();
            Calculos centro = new Calculos();
            objeto.matrix = objeto.llenado("Equipo3.gif");
            objeto.CompConec();
            objeto.Resultados();
            Console.ReadLine();
            Console.ReadLine();
        }

        public int[,] llenado(string archivo)
        {
            
            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));           
            int height = 110, width = 110, promedio;
            if(archivo == "Equipo3.gif")
            {
                height = 953; width = 953;
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
                for (int i = 0; i < 660; i++)
                {
                    for (int j = 0; j < 953; j++)
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
            Console.Write("\n" + comp[indice]);
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

        public void Tanimoto(int[,] o1, int[,] o2)
        {
            /*Console.Write("\n");
            for (int i = 0; i < 110; i++)
                {
                    for (int j = 0; j < 110; j++)
                    {
                        System.Console.ForegroundColor = ConsoleColor.White;
                        if (o1[i,j] == 1 && o2[i, j] == 0)
                            System.Console.ForegroundColor = ConsoleColor.Blue;
                        if (o2[i, j] == 1 && o1[i, j] == 0)
                            System.Console.ForegroundColor = ConsoleColor.Red;
                        if (o2[i, j] == 1 && o1[i, j] == 1)
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("0");
                    }
                Console.Write("\n");
            }
            Console.ReadLine();*/
            float pA, pB, pAB;
            int[,] nAB = new int[110, 110];
            pA = pB = pAB = 0;
            for (int i = 0; i < 110; i++)
            {
                for (int j = 0; j < 110; j++)
                {
                    if (o1[i, j] == 0 && o2[i, j] == 0)
                    {
                        nAB[i, j] = 0;
                    }

                    if (o1[i, j] == 0 && o2[i, j] == 1)
                    {
                        nAB[i, j] = 2;
                        pB = pB + 1;
                    }

                    if (o1[i, j] == 1 && o2[i, j] == 0)
                    {
                        nAB[i, j] = 1;
                        pA = pA + 1;
                    }

                    if (o1[i, j] == 1 && o2[i, j] == 1)
                    {
                        nAB[i, j] = 3;
                        pAB = pAB + 1;
                        pA = pA + 1;
                        pB = pB + 1;

                    }
                }

            }
            Console.Write("Pixeles AB: " + pAB + "\n");
            Console.Write("Pixeles A: " + pA + "\n");
            Console.Write("Pixeles B: " + pB + "\n");
            float resultado = pAB / (pA + pB - pAB);
            Console.Write("COEFICIENTE TANIMOTO: " + (resultado));
        }

        public int[,] Traslacion(int[,] matriz, int cmx, int cmy)
        {
            int[,] res = new int[matriz.GetLength(0), matriz.GetLength(1)];

            for (int i = 0; i <= matriz.GetLength(0) - 1; i++)
                for (int j = 0; j < matriz.GetLength(1) - 1; j++)
                    if (matriz[i, j] == 1 && ValidacionF(i + cmx, j + cmy))
                        res[i + cmx, j + cmy] = 1;

            return res;
        }

        public int[,] Mirror(int[,] matriz, string paDonde)
        {
            int[,] res = new int[matriz.GetLength(0), matriz.GetLength(1)];
            for (int i = 0; i < matriz.GetLength(0); i++)
                for (int j = 0; j < matriz.GetLength(1); j++)
                    if (matriz[i, j] == 1)
                        if (paDonde == "x")
                            res[matriz.GetLength(0) - (i + 1), j] = 1;
                        else
                            res[i, matriz.GetLength(1) - (j + 1)] = 1;

            return res;
        }

        public int[,] Rotacion(int[,] matriz, double grados, int xcm, int ycm)
        {
            int ix, iy;
            double x, y;
            int[,] res = new int[matriz.GetLength(0), matriz.GetLength(1)];
            double angulo = grados * Math.PI / 180.0; 
            for (int i = 0; i <= matriz.GetLength(0) - 1; i++)
                for (int j = 0; j < matriz.GetLength(1) - 1; j++)
                    if (matriz[i, j] == 1)
                    {
                        x = (Math.Cos(angulo) * (i - xcm)) - (Math.Sin(angulo) * (j - ycm));
                        y = (Math.Sin(angulo) * (i - xcm)) + (Math.Cos(angulo) * (j - ycm));
                        ix = (int)Math.Round(x) + xcm;
                        iy = (int)Math.Round(y) + ycm;
                        if (ValidacionF(ix, iy))
                            res[ix, iy] = 1;
                    }
            return res;
        }

        public void Resultados()
        {

            Calculos centro = new Calculos();
            int[,] rotada = new int[110, 110];
            int[,] espejo = new int[110, 110];
            string[] originales = { "fork.gif", "hammer.gif", "hat.gif", "heart.gif", "jar.gif" };
            int angulo = 45;

            int[,] elemento = new int[110, 110];
            int[] componentes = Tags();

            int[,] figura;
            for (int k = 0; k <= componentes.Length - 1; k++)
            {
                figura = ventanas(componentes, matrix, matrix.GetLength(0), matrix.GetLength(1), k);
                if (centro.MomentoAlgebraico(figura, 0, 0) >= 560 && centro.MomentoAlgebraico(figura, 0, 0) <= 5200)
                {
                    Console.Write("\nFigura No." + (k+1) + "\n");
                    for (int i = 0; i < 110; i++)
                    {
                        for (int j = 0; j < 110; j++)
                            Console.Write(figura[i, j]);
                        Console.Write("\n");
                    }
                    for (int index = 0; index < 5; index++)
                    {
                        Console.Write("\n\nComparacion con: " + originales[index]);
                        Console.Write("\n\nNORMAL\n");
                        //Datos de la imagen origial
                        original = llenado(originales[index]);
                        cmO = centro.CentroDeMasa(original);
                        centroXO = (int)Math.Round(cmO[0]);
                        centroYO = (int)Math.Round(cmO[1]);

                        //Datos de la figura a tratar
                        cmI = centro.CentroDeMasa(figura);
                        centroXI = (int)Math.Round(cmI[0]);
                        centroYI = (int)Math.Round(cmI[1]);

                        //Sacamos las diferencias de los centros de masa
                        difCMX = centroXO - centroXI;
                        difCMY = centroYO - centroYI;

                        //Trasladamos la figura basandonos en las diferencias de los centros de masa (Esto para acercarlo al centro de masa del objeto original)
                        figura = Traslacion(figura, difCMX, difCMY);
                        //Sacamos nuevamente el centro de masa de la figura a tratar
                        cmI = centro.CentroDeMasa(figura);
                        centroXI = (int)Math.Round(cmI[0]);
                        centroYI = (int)Math.Round(cmI[1]);

                        //Console.Write("\n\nComparacion figura \'" + k + "\', con " + originales[index] + ":\n\n");
                        //Calculamos tanimoto
                        Tanimoto(figura, original);
                        
                        //Empezamos con rotaciones
                        for (int r = 0; r < 3; r++)
                        {
                            Console.Write("\n\nROTACION " + angulo + "°");
                            rotada = Rotacion(figura, angulo, centroXI, centroYI);
                            //Datos de la figura a tratar
                            cmI = centro.CentroDeMasa(rotada);
                            centroXI = (int)Math.Round(cmI[0]);
                            centroYI = (int)Math.Round(cmI[1]);

                            //Sacamos las diferencias de los centros de masa
                            difCMX = centroXO - centroXI;
                            difCMY = centroYO - centroYI;

                            //Trasladamos la figura basandonos en las diferencias de los centros de masa (Esto para acercarlo al centro de masa del objeto original)
                            rotada = Traslacion(rotada, difCMX, difCMY);
                            //Console.Write("\n\nComparacion figura \'" + k + "\' " + angulo + "°, con " + originales[index] + ":\n\n");
                            Console.Write("\n");
                            //Comparamos con tanimoto
                            Tanimoto(rotada, original);
                            angulo += angulo;
                        }
                        angulo = 45;
                        //Seguimos con la figura en espejo
                        Console.Write("\n\nESPEJO EN X\n");
                        //Espejo en X
                        espejo = Mirror(figura, "x");
                        //Datos de la figura a tratar
                        cmI = centro.CentroDeMasa(espejo);
                        centroXI = (int)Math.Round(cmI[0]);
                        centroYI = (int)Math.Round(cmI[1]);

                        //Sacamos las diferencias de los centros de masa
                        difCMX = centroXO - centroXI;
                        difCMY = centroYO - centroYI;

                        //Trasladamos la figura basandonos en las diferencias de los centros de masa (Esto para acercarlo al centro de masa del objeto original)
                        espejo = Traslacion(espejo, difCMX, difCMY);
                        //Console.Write("\n\nComparacion figura \'" + k + "\' Espejo en X, con " + originales[index] + ":\n\n");
                        Tanimoto(espejo, original);
                        Console.Write("\n\nESPEJO EN Y\n");
                        //Espejo en Y
                        espejo = Mirror(figura, "y");
                        //Datos de la figura a tratar
                        cmI = centro.CentroDeMasa(espejo);
                        centroXI = (int)Math.Round(cmI[0]);
                        centroYI = (int)Math.Round(cmI[1]);

                        //Sacamos las diferencias de los centros de masa
                        difCMX = centroXO - centroXI;
                        difCMY = centroYO - centroYI;

                        //Trasladamos la figura basandonos en las diferencias de los centros de masa (Esto para acercarlo al centro de masa del objeto original)
                        espejo = Traslacion(espejo, difCMX, difCMY);
                        //Console.Write("\n\nComparacion figura \'" + k + "\' Espejo en Y, con " + originales[index] + ":\n\n");
                        Tanimoto(espejo, original);
                        Console.ReadLine();
                        Console.Write("\n\n████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████\n");
                    }
                    Console.ReadLine();
                }
            }
        }
    }
}
