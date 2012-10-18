using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tpr2
{
    public class Core
    {

        private double A, B, C;
        public ICollection<Entity> teachingRow = Entity.GenerateEntities(
                new[] { 2, 1, 2, 3, 4, 5, 6, 4, 5, 6 },
                new[] { 2, 3, 3, 5, 4, 6, 5, 6, 5, 7 },
                new[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 }
                );

        public Entity CalculateLine(double alpha)
        {
            Entity weightVector = new Entity(1, 1, 1);

            Int32 tag = 1, iter = 0, a = 1;

            while (tag != 0)
            {
                tag = 0;
                foreach (Entity e in teachingRow)
                {

                    Int32 resultY = weightVector.X * e.X + weightVector.Y * e.Y <= weightVector.Class ? 0 : 1;

                    if (resultY != e.Class)
                    {
                        ++tag;
                    }
                    else
                    {
                        continue;
                    }

                    switch (a)
                    {
                        case 1:
                            weightVector.X += alpha * e.X * (e.Class - resultY);
                            ++a;
                            break;
                        case 2:
                            weightVector.Y += alpha * e.Y * (e.Class - resultY);
                            ++a;
                            break;
                        case 3:
                            weightVector.Class += alpha * (e.Class + resultY);
                            a = 1;
                            break;
                        default:
                            break;
                    }
                }
                ++iter;
            }
            return weightVector;
        }

        public void SetCoefficientes(Entity vector)
        {
            A = vector.X;
            B = vector.Y;
            C = vector.Class;
        }

        public double f(double x)
        {
            return (-C - A*x) / B + 19;
        }

    }
}
