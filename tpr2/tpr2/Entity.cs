using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tpr2
{
    public class Entity
    {
        public Double X { get; set; }
        public Double Y { get; set; }
        public Double Class { get; set; }

        public Entity()
        {
            this.X = 0;
            this.Y = 0;
            this.Class = 0;
        }

        public Entity(Int32 X, Int32 Y, Int32 Class)
        {
            this.X = X;
            this.Y = Y;
            this.Class = Class;
        }

        public static ICollection<Entity> GenerateEntities(Int32[] xArray, Int32[] yArray, Int32[] classArray)
        {
            ICollection<Entity> entities = new List<Entity>();
            for (int i = 0; i < xArray.Length; ++i)
            {
                entities.Add(new Entity()
                {
                    X = xArray[i],
                    Y = yArray[i],
                    Class = classArray[i]
                });
            }
            return entities;
        }
    }

}
