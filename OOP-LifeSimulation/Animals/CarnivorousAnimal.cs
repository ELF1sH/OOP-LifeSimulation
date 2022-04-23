using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    abstract class CarnivorousAnimal : Animal
    {
        public CarnivorousAnimal(int _x, int _y, Map _map, int _animalId) : base(_x, _y, _map, _animalId)
        {
            FeedType = FeedType.Carnivorous;
        }

        protected override AnimalResult CheckFood(Unit food)
        {
            if (food == null && targetAnimal != null)
            {
                if (Math.Abs(Coords.X - targetAnimal.Coords.X) + Math.Abs(Coords.Y - targetAnimal.Coords.Y) == 1)
                {
                    SearchFor(SearchGoal.Food);
                    targetAnimal = null;
                    return AnimalResult.EatedAnimal;
                }
                return AnimalResult.Nothing;
            }
            AnimalResult animalResult = AnimalResult.Nothing;
            if (food is Animal)
            {
                // Console.WriteLine("Eated animal");
                animalResult = AnimalResult.EatedAnimal;
                targetAnimal = null;
                UpdateHP__HealthyFood();
            }
            // Detaching eaten units from map
            if (food is Animal) map.DetachAnimalFromCell(Coords.X, Coords.Y, (food as Animal).AnimalType);

            return animalResult;
        }
    }
}
