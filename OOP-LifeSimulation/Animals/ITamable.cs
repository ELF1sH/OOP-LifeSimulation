using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public interface ITamable
    {
        public bool IsTamed { get; set; }
        public int OwnerId { get; set; }
        public void ComeToOwner(int x, int y);
        public Human Owner { get; set; }
        public void TamedBehavior();
        public void RemoveOwner();
    }
}
