using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_LifeSimulation
{
    public class Cell
    {
        public BiomStatus BiomStatus { get; private set; }

        public bool HasPlant { get; private set; }
        public Plant Plant { get; private set; }

        public bool HasFruit { get; private set; }
        public Fruit Fruit { get; private set; } = null;

        public bool HasAnimal { get; private set; }
        public List<Animal> Animals { get; private set; }

        public Resource Resource { get; set; }
        public Building Building { get; set; } = null;
        public List<Material> MaterialsOnConstruction { get; } = new List<Material>();

        public Cell(BiomStatus _bs)
        {
            BiomStatus = _bs;
            Animals = new List<Animal>();

            HasPlant = false;
        }

        public void AttachPlant(ref Plant _plant)
        {
            HasPlant = true;
            Plant = _plant;
        }

        public void DetachPlant()
        {
            HasPlant = false;
            Plant = null;
        }

        public void AttachFruit(ref Fruit _fruit)
        {
            HasFruit = true;
            Fruit = _fruit;
        }

        public void DetachFruit()
        {
            HasFruit = false;
            Fruit = null;
        }

        public void AttachAnimal(ref Animal _animal)
        {
            HasAnimal = true;
            Animals.Add(_animal);
        }

        public void DetachAnimal(AnimalType animalType)
        {
            if (Animals.Count == 0)
            {
                HasAnimal = false;
                return; 
            }
            foreach (var an in Animals)
            {
                if (an.AnimalType == animalType)
                {
                    Animals.Remove(an);
                    break;
                }
            }
            if (Animals.Count == 0) HasAnimal = false;
        }
    }
}
