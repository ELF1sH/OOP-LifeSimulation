using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    class Statistic
    {
        private GameForm gameForm;

        private int deadAnimals = 0;
        public Statistic(GameForm _gameForm)
        {
            gameForm = _gameForm;
        }
        public void UpdateStatistic(int ok, int hungry, int dead)
        {
            UpdateOkAnimals(ok);
            UpdateHungryAnimals(hungry);
            AddDeadAnimals(dead);
        }
        private void UpdateOkAnimals(int value)
        {
            gameForm.UpdateOkLabel(value);
        }
        private void UpdateHungryAnimals(int value)
        {
            gameForm.UpdateHungryLabel(value);
        }
        private void AddDeadAnimals(int value)
        {
            deadAnimals += value;
            gameForm.UpdateDeadLabel(deadAnimals);
        }
    }
}
