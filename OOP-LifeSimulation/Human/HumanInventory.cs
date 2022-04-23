using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_LifeSimulation
{
    public class HumanInventory
    {
        public Storage<IStoragable> Storage { get; } = new Storage<IStoragable>();

        public int InventorySize { get; } = 5;
        
        public Unit GetFood()
        {
            Unit food = null;
            foreach (var item in Storage.Items)
            {
                if (item is Unit)
                {
                    food = item as Unit;
                    Storage.Items.Remove(item);
                    break;
                }
            }
            return food;
        }

        public int GetAvailableSpace()
        {
            return InventorySize - Storage.Items.Count;
        }

        public List<Material> RemoveMaterials()
        {
            List<Material> outputList = new List<Material>();
            foreach (var item in Storage.Items.ToArray())
            {
                if (item is Material)
                {
                    outputList.Add(item as Material);
                    Storage.Items.Remove(item);
                }
            }
            return outputList;
        }

        public List<Unit> GetAllFood()
        {
            var outputList = new List<Unit>();
            foreach (var item in Storage.Items.ToArray())
            {
                if (item is Unit)
                {
                    outputList.Add(item as Unit);
                    Storage.Items.Remove(item);
                }
            }
            return outputList;
        }
    }
}
