using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    abstract class ProlificPlant : Plant
    {
        public List<Fruit> Fruits { get; } = new List<Fruit>();
        protected bool isFruitPoisonous;
        protected bool doesMakeFruitsInWinter;

        public ProlificPlant(int _x, int _y, PlantStatus _plantStatus, Map _map) : base(_x, _y, _plantStatus, _map)
        {
            
        }

        public void GenerateFruits()
        {
            for (int i = 0; i < 5; i++)
            {
                Fruits.Add(new Fruit(Coords.X, Coords.Y, isFruitPoisonous, PlantType));
            }
        }

        public List<Fruit> GrowFruits()
        {
            var outputFruits = new List<Fruit>();
            if (map.CurSeason == Season.Winter && !doesMakeFruitsInWinter)
            {
                return outputFruits;
            }
            else if (PlantStatus == PlantStatus.Adult)
            {
                foreach (Fruit fruit in Fruits)
                {
                    bool isReadyToDrop = fruit.Grow();
                    if (isReadyToDrop)
                    {
                        fruit.UpdateCoords(map.GetFreeCellCloseToPlant(fruit.Coords));
                        outputFruits.Add(fruit);
                    }
                }
                foreach (Fruit fruit in outputFruits)
                {
                    Fruits.Remove(fruit);
                }
            }
            return outputFruits;
        }
    }
}
