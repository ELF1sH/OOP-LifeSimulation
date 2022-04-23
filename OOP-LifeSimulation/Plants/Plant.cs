using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_LifeSimulation
{
    public abstract class Plant : Unit
    {
        protected readonly Map map;

        public PlantStatus PlantStatus { get; private set; }
        public PlantType PlantType { get; protected set; }

        private int age;

        private readonly int seedsAge = 0;
        private readonly int sproutAge = 25;
        private readonly int adultAge = 55;
        private readonly int rottedAge = 130;

        protected bool isWintery;

        public Plant(int _x, int _y, PlantStatus _plantStatus, Map _map) : base(_x, _y)
        {
            PlantStatus = _plantStatus;
            switch (PlantStatus)
            {
                case PlantStatus.Seeds: age = seedsAge; break;
                case PlantStatus.Sprout: age = sproutAge; break;
                case PlantStatus.Adult: age = adultAge; break;
                case PlantStatus.Rotted: age = rottedAge; break;
            }
            map = _map;
        }
       
        public PlantResult Grow()
        {
            if (map.CurSeason == Season.Winter && (PlantStatus != PlantStatus.Seeds && !isWintery))
            {
                return PlantResult.Dead;
            }
            else if (map.CurSeason == Season.Winter && (PlantStatus == PlantStatus.Seeds || isWintery))
            {
                return PlantResult.Nothing;
            }
            else if (map.CurSeason == Season.Summer)
            {
                age++;
                if (age == sproutAge)
                {
                    PlantStatus = PlantStatus.Sprout;
                    return PlantResult.HasGrown;
                }
                else if (age == adultAge)
                {
                    PlantStatus = PlantStatus.Adult;
                    return PlantResult.HasGrown;
                }
                else if (age == rottedAge)
                {
                    PlantStatus = PlantStatus.Rotted;
                    return PlantResult.HasGrown;
                }
            }
            return PlantResult.Nothing;
        }

        public List<Plant> GenerateSeeds()
        {
            var seeds = new List<Plant>();
            int value = new Random().Next(0, 50);
            if (map.CurSeason == Season.Winter) return seeds;
            if (value == 0 && PlantStatus == PlantStatus.Adult)
            {
                Type typeOfPlant = GetType();
                var p = map.GetFreeCellCloseToPlant(Coords);
                var newObj = Activator.CreateInstance(typeOfPlant, p.X, p.Y, PlantStatus.Seeds, map);
                Plant plant = newObj as Plant;
                seeds.Add(plant);
            }
            return seeds;
        }
    }
}
