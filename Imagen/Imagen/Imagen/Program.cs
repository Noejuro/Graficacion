using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Imagen
{
    class Program
    {
        public Tuple<Color[][], int, int> GetBitmapColorMatrix(string filePath, int umbral)
        {
            Bitmap b1 = new Bitmap(filePath);
            Bitmap b2 = new Bitmap(filePath);

            int height = b1.Height, width = b1.Width, promedio;
            double funcion; 
            Color[][] colorMatrix = new Color[width][];
            int[][] grises = new int[width][];
            for (int i = 0; i < width; i++)
            {
                colorMatrix[i] = new Color[height];
                grises[i] = new int[height];
                for(int j = 0; j <height; j++)
                {
                    colorMatrix[i][j] = new Color();
                    colorMatrix[i][j] = b1.GetPixel(i, j);
                    funcion = ((.2125 * colorMatrix[i][j].R) + (.7154 * colorMatrix[i][j].G) + (.0721 * colorMatrix[i][j].B));
                    promedio = Convert.ToInt32(funcion);
                    b1.SetPixel(i, j, Color.FromArgb(colorMatrix[i][j].A, promedio, promedio, promedio));
                    if (promedio < umbral)
                        b2.SetPixel(i, j, Color.Black);
                    else
                        b2.SetPixel(i, j, Color.White);
                }
            }
            
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string direccion = Path.Combine(System.IO.Path.GetDirectoryName(path), "Gray.png");
            string direccion2 = Path.Combine(System.IO.Path.GetDirectoryName(path), "Black&White.png");
            b1.Save(direccion);
            b2.Save(direccion2);
            return Tuple.Create(colorMatrix,height,width);
        }
        static void Main(string[] args)
        {
            int umbral;
            Console.Write("Ingresa el Umbral\n>");
            umbral = Console.Read();
            Console.Write("Espere un momento...");
            Program clase = new Program();
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string direccion = Path.Combine(System.IO.Path.GetDirectoryName(path), "Photo.png");
            var resultado = clase.GetBitmapColorMatrix(direccion,umbral);
            Color[][] imagen = resultado.Item1;
            int height = resultado.Item2;
            int width = resultado.Item3;
            Console.Write("La imagen de tamaño " + width + "x" + height + " se ha creado en escala de grises.\nPresione ENTER para salir.");
            Console.Read();
            Console.Read();
            Console.Read();
            Console.Read();
            Console.Read();
        }
    }
}
