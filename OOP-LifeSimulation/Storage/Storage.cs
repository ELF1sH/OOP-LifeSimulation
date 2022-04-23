using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_LifeSimulation
{
    public class Storage<T> where T : IStoragable
    {
        public List<T> Items { get; } = new List<T>();

        public void AddItems(List<T> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                Items.Add(item);
            }
        }

        public List<IStoragable> RemoveItems(int amount)
        {
            var outputItems = new List<IStoragable>();
            for (int i = 0; i < amount; i++)
            {
                if (Items.Count > 0)
                {
                    outputItems.Add(Items[^1]);
                    Items.RemoveAt(Items.Count - 1);
                }
                else
                {
                    break;
                }
            }
            return outputItems;
        }
    }
}
