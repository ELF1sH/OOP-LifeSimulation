using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    abstract public class OmnivorousAnimal : Animal
    {
        public OmnivorousAnimal(int _x, int _y, Map _map, int _animalId) : base(_x, _y, _map, _animalId)
        {
            FeedType = FeedType.Omnivorous;
        }
    }
}
