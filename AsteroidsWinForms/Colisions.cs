using System;
using System.Drawing;

namespace Asteroids {
    public static class Colisions {
        /// <summary>
        /// Collision system that uses the SAT (Separated Axis System). This theorem consists of verifying if there is an axis on which the projections of the objects do not overlap.
        /// It consists of simulating the creation of several axis depending on the edges of the objects, then projecting the leftmost and rightmost vertex according to that same axis.
        /// Next, it checks that in all the projected axes, there is at least one in which there is no overlap of the projected line segments.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool VerifyCollision(GameObject obj1, GameObject obj2) {
            
            GameObject objAux1 = obj1;
            GameObject objAux2 = obj2;

            for(int objeto = 0; objeto < 2; objeto++) {
                if(objeto == 1) {
                    objAux1 = obj2;
                    objAux2 = obj1;
                }

                for(int i = 0; i < objAux1.Vertices.Length; i++) {
                    int j = (i + 1) % objAux1.Vertices.Length;
                    PointF projEixo = new PointF(-(objAux1.Vertices[j].Y - objAux1.Vertices[i].Y), objAux1.Vertices[j].X - objAux1.Vertices[i].X);
                    float k = (float)Math.Sqrt(Math.Pow(projEixo.X, 2) + Math.Pow(projEixo.Y, 2));
                    projEixo = new PointF(projEixo.X / k, projEixo.Y / k);

                    float min1 = float.PositiveInfinity, min2 = float.PositiveInfinity, max1 = float.NegativeInfinity, max2 = float.NegativeInfinity;
                    for(int p = 0; p < objAux1.Vertices.Length; p++) {
                        float q = (objAux1.Vertices[p].X * projEixo.X + objAux1.Vertices[p].Y * projEixo.Y);
                        min1 = Math.Min(min1, q);
                        max1 = Math.Max(max1, q);
                    }

                    for(int p = 0; p < objAux2.Vertices.Length; p++) {
                        float q = (objAux2.Vertices[p].X * projEixo.X + objAux2.Vertices[p].Y * projEixo.Y);
                        min2 = Math.Min(min2, q);
                        max2 = Math.Max(max2, q);
                    }

                    if(!(max2 >= min1 && max1 >= min2))
                        return false;
                }
            }
            return true;
        }
    }
}