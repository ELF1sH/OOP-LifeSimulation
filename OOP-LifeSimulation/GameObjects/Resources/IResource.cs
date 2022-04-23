using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_LifeSimulation
{
    public interface IResource<T, U> where T : IStoragable, new() where U : Tool
    {
        public Storage<T> Storage { get; }
        public static Type ToolType { get; } = typeof(U);
    }
}
