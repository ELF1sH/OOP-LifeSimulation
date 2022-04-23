using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OOP_LifeSimulation
{
    public class Game
    {
        private readonly Engine engine;
        private readonly int fieldSize = 1000;
        private readonly GameForm gameForm;

        private int mapWidth;

        public Game(GameForm _gameForm)
        {
            gameForm = _gameForm;
            engine = new Engine(gameForm, fieldSize);

            mapWidth = gameForm.FieldPictureBox.Width;
        }

        public void LaunchGame()
        {
            var timer = new Timer
            {
                Interval = 1000
            };
            timer.Tick += new EventHandler((sender, e) => TimerTick(sender, e));
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            engine.Update(sender, e, mapWidth);
            if (gameForm.IsInfoFormOpened)
            {
                var dataList = engine.GetUpdatedAnimalInfo();
                if (dataList.Count > 0)
                {
                    gameForm.InfoForm.UpdateLabel(dataList);
                }
                else
                {
                    gameForm.CloseInfoForm();
                }
            }
        }

        public void GetAnimalInfo(Point p)
        {
            var dataList = engine.GetAnimalInfo(p);
            if (dataList.Count > 0)
            {
                gameForm.OpenInfoForm();
                gameForm.InfoForm.UpdateLabel(dataList);
            }
        }

        public void ZoomGame(bool isIn)
        {
            if (isIn && mapWidth > 10000)  
            {
                mapWidth -= 4000;
                gameForm.FieldPictureBox.Size = new Size(mapWidth, mapWidth);
                engine.UpdateZoom();
            }
            else if (!isIn && mapWidth < 22000)
            {
                mapWidth += 4000;
                gameForm.FieldPictureBox.Size = new Size(mapWidth, mapWidth);
                engine.UpdateZoom();
            }
        }
    }
}
