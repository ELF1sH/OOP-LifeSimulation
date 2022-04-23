using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_LifeSimulation
{
    public enum BiomStatus { Water, Grass, Desert, Hill}

    public enum Season { Summer, Winter }



    public enum PlantType { Bush, Carrot, PoisonousMushroom, AppleTree, PoisonousBerryBush, Peas, Tree }

    public enum PlantStatus { Seeds, Sprout, Adult, Rotted }

    public enum PlantResult { Nothing, HasGrown, Dead }



    public enum AnimalType { Horse, Elephant, Rabbit, Wolf, Leopard, Tiger, Hedgehog, Monkey, Bear, Human }

    public enum AnimalResult { Nothing, SearchingForFood, EatedPlant, EatedPoisonousPlant, EatedAnimal, Dead}

    public enum FeedType { Herbivorous, Carnivorous, Omnivorous }

    public enum Gender { Male, Female }

    public enum SearchGoal { Food, Female, TamableAnimal, Owner, Materials, Construction, Warehouse, ClosestBuilding, ConcretePos, HerbFood, CarnFood }

    public enum CurPath { None, Food, Female, Owner, Materials, Construction, Warehouse, Home, HerbFood, CarnFood, Barn, BarnToGetFood }

    public enum Role { Hunter, Collector, Builder, Shepherd }
}
