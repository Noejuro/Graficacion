using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica_2
{
    class Program
    {
        public int[,] bin035 = new int[1000, 800];
        public int width, height;
        public double m00 = 0, m01 = 0, m10 = 0, m20 = 0, m02 = 0, m11 = 0, m30 = 0, m12 = 0, m21 = 0, m03 = 0, xCM = 0, yCM = 0, xCMC = 0, yCMC = 0, u20 = 0, u02 = 0, u00 = 0, u01 = 0, u10 = 0, u11 = 0, u30 = 0, u12 = 0, u21 = 0, u03 = 0;
        static void Main(string[] args)
        {
            int j = 0, x = 0, i = 0;
            Program objeto = new Program();
            //Creacion de un vector con el nombre de los archivos a usar
            String[] nombres = new String[50] { "035.bmp", "035T.bmp", "035.bmp", "03545.bmp", "03590.bmp", "035180.bmp", "035270.bmp", "035.bmp", "035ZI.bmp", "035ZO.bmp", "068.bmp", "068T.bmp", "068.bmp", "06845.bmp", "06890.bmp", "068180.bmp", "068270.bmp", "068.bmp", "068ZI.bmp", "068ZO.bmp", "007.bmp", "007T.bmp", "007.bmp", "00745.bmp", "00790.bmp", "007180.bmp", "007270.bmp", "007.bmp", "007ZI.bmp", "007ZO.bmp", "033.bmp", "033T.bmp", "033.bmp", "03345.bmp", "03390.bmp", "033180.bmp", "033270.bmp", "033.bmp", "033ZI.bmp", "033ZO.bmp", "043.bmp", "043T.bmp", "043.bmp", "04345.bmp", "04390.bmp", "043180.bmp", "043270.bmp", "043.bmp", "043ZI.bmp", "043ZO.bmp" };
            //ciclo usado para mostrar los datos de cada uno de las imagenes
            do
            {
                x = 0;
                Console.Write("IMAGEN\t\t        CENTRO DE MASA\t\t\t\t\t\t\tMOMENTOS CENTRALES");
                objeto.llenar(nombres[i]);
                Console.Write("\n" + nombres[i] + " ");
                objeto.centroM(nombres[i]);
                objeto.momentosC();
                i++;
                objeto.llenar(nombres[i]);
                Console.Write("\n" + nombres[i]);
                objeto.centroM(nombres[i]);
                objeto.momentosC();
                i++;

                Console.Write("\n\nIMAGEN\t\t        CENTRO DE MASA\t\t\t\t\t\t\tMOMENTOS HU");
                do
                {
                    objeto.llenar(nombres[i]);
                    Console.Write("\n" + nombres[i] + " ");
                    objeto.centroM(nombres[i]);
                    objeto.rotacionHU();
                    i++; x++;
                } while (x < 5);
                x = 0;
                Console.Write("\n\nIMAGEN\t\t        CENTRO DE MASA\t\t\t\t\t\t\tMOMENTOS ESCALAMIENTO");
                do
                {
                    objeto.llenar(nombres[i]);
                    Console.Write("\n" + nombres[i] + " ");
                    objeto.centroM(nombres[i]);
                    objeto.momentosCN();
                    i++; x++;
                } while (x < 3);
                Console.Write("\n\n████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████\n");
            } while (i < 50);
            Console.Read();
        }

        //Metodo para el paso de los datos (1,0) de la imagen a una matriz global
        public void llenar(String nombre)
        {
            Array.Clear(bin035, 0, bin035.Length);
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string filePath035 = Path.Combine(System.IO.Path.GetDirectoryName(path), nombre);
            Bitmap O35 = new Bitmap(filePath035); 
            double funcion;
            height = O35.Height; width = O35.Width;
            int promedio;
            Color[][] colorMatrix = new Color[width][];
            for (int i = 0; i < width; i++)
            {
                colorMatrix[i] = new Color[height];
                for (int j = 0; j < height; j++)
                {
                    colorMatrix[i][j] = new Color();
                    colorMatrix[i][j] = O35.GetPixel(i, j);
                    funcion = ((.2125 * colorMatrix[i][j].R) + (.7154 * colorMatrix[i][j].G) + (.0721 * colorMatrix[i][j].B));
                    promedio = Convert.ToInt32(funcion);
                    if (promedio < 80) //Uso del umbral para decidir si es 1 o 0 en la matriz
                        bin035[i,j] = 1;
                    else
                        bin035[i,j] = 0;
                }
            }
        }

        //Metodo para la obtencion de cada uno de los momentos de orden que vayamos a utilizar, ademas del calculo del Centro de Masa
        public void centroM(String nombre)
        {
            m00 = 0; m01 = 0; m10 = 0; m02 = 0; m20 = 0; m11 = 0; m30 = 0; m12 = 0; m03 = 0; //Limpieza de las variables

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (bin035[i, j] == 1)
                    {
                        //Calculo de cada uno de los momentos
                        m00 += (Math.Pow(j, 0)) * (Math.Pow(i, 0));
                        m01 += (Math.Pow(j, 0)) * (Math.Pow(i, 1));
                        m10 += (Math.Pow(j, 1)) * (Math.Pow(i, 0));
                        m02 += (Math.Pow(j, 0)) * (Math.Pow(i, 2));
                        m20 += (Math.Pow(j, 2)) * (Math.Pow(i, 0));
                        m11 += (Math.Pow(j, 1)) * (Math.Pow(i, 1));
                        m30 += (Math.Pow(j, 3)) * (Math.Pow(i, 0));
                        m12 += (Math.Pow(j, 1)) * (Math.Pow(i, 2));
                        m03 += (Math.Pow(j, 0)) * (Math.Pow(i, 3)); ;
                    } 
            //Calculo del centro de masa
            xCM = m10 / m00;
            yCM = m01 / m00;
            //Impresion del centro de masa
            Console.Write("\t(" + xCM + "," + yCM + ")");
        }


        //Metodo para el calculo de los momentos centrales para la traslacion
        public void momentosC()
        {
            u20 = 0; u02 = 0; u00 = 0; u01 = 0; u10 = 0; u11 = 0; u30 = 0; u12 = 0; u21 = 0; u03 = 0; //Limpieza de las variables
            //Calculo de los momentos centrales
            u00 = m00;
            u20 = m20 - xCM * m10;
            u02 = m02 - yCM * m01;
            u11 = m11 - yCM * m10;
            u30 = m30 - (3 * xCM * m20) + (2 * (Math.Pow(xCM, 2)) * m10);
            u12 = m12 - (2 * yCM * m11) - (xCM * m02) + (2 * (Math.Pow(yCM, 2)) * m10);
            u21 = m21 - (2 * xCM * m11) - (yCM * m20) + (2 * (Math.Pow(xCM, 2)) * m01);
            u03 = m03 - (3 * yCM * m02) + (2 * (Math.Pow(yCM, 2)) * m01);
            //Impresion de los momentos centrales
            Console.Write("\t" + Math.Floor(u00) + " " + u10 + " " + u01 + " " + Math.Floor(u20) + " " + Math.Floor(u02) + " " + Math.Floor(u11));
            Console.Write(" " + Math.Floor(u30) + " " + Math.Floor(u12) + " " + Math.Floor(u21) + " " + Math.Floor(u03));


        }
        //Metodo para la obtencion de los invariantes de HU para la rotacion
        public void rotacionHU()
        {
            
            double O1 = 0, O2 = 0, O3 = 0; //Declaracion de los invariantes de HU
            //Calculo de cada uno de los invariantes de HU con uso de los metodos centrales antes declarados
            O1 = u20 + u02;
            O2 = (Math.Pow((u20 - u02), 2)) + (4 * (Math.Pow(u11, 2))); 
            O3 = (Math.Pow((u30 - (3 * u12)), 2)) + (Math.Pow(((3 * u21) - u03), 2));
            //Impresion de los invariantes de HU
            Console.Write("\tO1:" + O1 + "    O2:" + O2 + "    O3:" + O3 );
        }
        //Metodo para el calculo de los momentos centrales Normalizados y las invariantes a cambios de escala
        public void momentosCN()
        {
            //Declaracion de los momentos centrales normalizados e invariantes a cambios de escala
            double eo1 = 0, eo2 = 0, eo3 = 0, n20 = 0, n02 = 0, n11 = 0, n30 = 0, n12 = 0, n21 = 0, n03 = 0, y = 0;
            //Obtencion de los momentos centrales normalizados
            n30 = u30 / (Math.Pow(u00, 3));

            n20 = u20 / (Math.Pow(u00, 2));

            n02 = u02 / (Math.Pow(u00, 2));

            n11 = u11 / (Math.Pow(u00, 2));

            n12 = u12 / (Math.Pow(u00, 3));

            n21 = u21 / (Math.Pow(u00, 3));

            n03 = u03 / (Math.Pow(u00, 3));
            //Calculo de las invariantes a cambios de escala haciendo uso de los momentos centrales normalizados
            eo1 = n20 + n02;
            eo2 = (Math.Pow((n20 - n02), 2)) + (4 * Math.Pow((n11), 2));
            eo3 = (Math.Pow((n30 - (3* n12)), 2)) + (Math.Pow(((3 * n21) - n03), 2));
            //Impresion de los invariantes a cambios de escala
            Console.Write("\t\t" + eo1 + "  " + eo2 + "  " + eo3);

        }
    }
}
