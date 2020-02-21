using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen2
{
    class Calculos
    {
        private double m00;
        private double m01;
        private double m10;
        private double m11;
        private double m20;
        private double m02;

        public double MomentoAlgebraico(int[,] binario, int p, int q)
        {
            int x = binario.GetLength(0);
            int y = binario.GetLength(1);
            double momento = 0;
            double xp, yq;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (binario[i, j] == 1)
                    {
                        xp = Math.Pow(i, p);
                        yq = Math.Pow(j, q);
                        momento = momento + (xp * yq);
                    }
                }
            }
            return momento;
        }

        public double[] CentroDeMasa(int[,] bin)
        {
            double[] CM = new double[2];
            double A, x, y;
            A = MomentoAlgebraico(bin, 0, 0);
            x = MomentoAlgebraico(bin, 1, 0);
            y = MomentoAlgebraico(bin, 0, 1);

            m00 = A;
            m10 = x;
            m01 = y;
            CM[0] = x / A;
            CM[1] = y / A;
            return CM;
        }

        public double MomentoCentral(int[,] binario, int orden1, int orden2, double X, double Y)
        {
            double momentoC = 0;
            if (orden1 == 0 && orden2 == 0)
            {
                momentoC = m00;
            }
            if (orden1 == 1 && orden2 == 0)
            {
                momentoC = 0;
            }
            if (orden1 == 0 && orden2 == 1)
            {
                momentoC = 0;
            }
            return momentoC;
        }
    }
}
