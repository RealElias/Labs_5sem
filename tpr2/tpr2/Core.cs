using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using ZedGraph;

namespace tpr2
{
    public class Core
    {

        private double _a, _b, _c;

        public ICollection<Entity> TeachingRow = Entity.GenerateEntities(
                new[] { 2, 1, 2, 3, 4, 5, 6, 4, 5, 6 },
                new[] { 2, 3, 3, 5, 4, 6, 5, 6, 5, 7 },
                new[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 }
                );

        public Entity TeachCalculateLine(double alpha)
        {
            var weightVector = new Entity(1,1,1);

            Int32 CountOfUnmatched = 1;

            while (CountOfUnmatched != 0)
            {
                CountOfUnmatched = 0;
                foreach (Entity e in TeachingRow)
                {

                    Int32 resultY = weightVector.X * e.X + weightVector.Y * e.Y <= -weightVector.Class ? 0 : 1;

                    if (resultY != e.Class)
                        ++CountOfUnmatched;
                    else
                        continue;
                    
                    weightVector.X += alpha * e.X * (e.Class - resultY);
                    weightVector.Y += alpha * e.Y * (e.Class - resultY);
                    weightVector.Class += alpha * (e.Class - resultY);
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
            return (-_c - _a * x) / _b;
        }


        public Entity AnalitCalculateLine()
        {
            ICollection<Entity> class0 =
                TeachingRow.Select(entity => entity).Where(entity => entity.Class == 0).ToList();
            ICollection<Entity> class1 =
                TeachingRow.Select(entity => entity).Where(entity => entity.Class == 1).ToList();

            var etalon0 = new Entity();
            var etalon1 = new Entity();
            var averagePoint = new Entity();
            var Line = new Entity();
            var Perpendicular = new Entity();
            double _a, _b, _c;
            etalon0.X = class0.Average(e => e.X);
            etalon0.Y = class0.Average(e => e.Y);
            etalon1.X = class1.Average(e => e.X);
            etalon1.Y = class1.Average(e => e.Y);
            averagePoint.X = (etalon0.X + etalon1.X)/2;
            averagePoint.Y = (etalon0.Y + etalon1.Y)/2;
            Line.X = 1/(etalon1.X - etalon0.X);
            Line.Y = -1/(etalon1.Y - etalon0.Y);
            Line.Class = -Line.X * etalon0.X - Line.Y * etalon0.Y;
            Perpendicular.X = -1 / (-Line.X / Line.Y);
            Perpendicular.Y = -1;
            Perpendicular.Class = averagePoint.Y - Perpendicular.X * averagePoint.X;
            return Perpendicular;
        }
    }
}
