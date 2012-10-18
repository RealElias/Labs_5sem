using System;
using System.Collections.Generic;

namespace tpr2
{
    public class Core
    {

        private double _a;
        private double _b;
        private double _c;

        public ICollection<Entity> TeachingRow = Entity.GenerateEntities(
                new[] { 2, 1, 2, 3, 4, 5, 6, 4, 5, 6 },
                new[] { 2, 3, 3, 5, 4, 6, 5, 6, 5, 7 },
                new[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 }
                );

        public Entity CalculateLine(double alpha)
        {
            var weightVector = new Entity(1, 1, 1);

            Int32 tag = 1, a = 1;

            while (tag != 0)
            {
                tag = 0;
                foreach (Entity e in TeachingRow)
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
                            {
                                weightVector.X += alpha*e.X*(e.Class - resultY);
                                ++a;
                                break;
                            }
                        case 2:
                            {
                                weightVector.Y += alpha*e.Y*(e.Class - resultY);
                                ++a;
                                break;
                            }
                        case 3:
                            {
                                weightVector.Class += alpha*(e.Class + resultY);
                                a = 1;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
            return weightVector;
        }

        public void SetCoefficientes(Entity vector)
        {
            _a = vector.X;
            _b = vector.Y;
            _c = vector.Class;
        }

        public double F(double x)
        {
            return (_c - _a*x) / _b;
        }

    }
}
