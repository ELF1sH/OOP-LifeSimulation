using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_LifeSimulation
{
    public partial class GameForm : Form
    {
        private Game _game;
        private bool isCtrlPressed = false;

        private int pictureBoxSize;
        private int fieldSize = 1000;
        private int cellSize;

        public bool IsInfoFormOpened { get; private set; } = false;
        public InfoForm InfoForm { get; set; }

        public GameForm()
        {
            InitializeComponent();
            pictureBoxSize = FieldPictureBox.Width;
            cellSize = pictureBoxSize / fieldSize;

            KeyUp += GameForm_KeyUp;
            KeyDown += GameForm_KeyDown;
            MouseWheel += GameForm_MouseWheel;
        }

        private void GameForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (isCtrlPressed)
                { 
                    _game.ZoomGame(false);
                }
            }
            else if (e.Delta < 0)
            {
                if (isCtrlPressed)
                {
                    _game.ZoomGame(true);
                }
            }
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isCtrlPressed = false;
            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isCtrlPressed = true;
            }
        }

        public void SetGameObject(ref Game game)
        {
            _game = game;
        }



        #region Functions for statistic
        public void UpdateOkLabel(int value)
        {
            ok_label.Text = "Animals feeling well: " + value.ToString();
        }
        public void UpdateHungryLabel(int value)
        {
            lookingForFood_label.Text = "Animals looking for food: " + value.ToString();
        }
        public void UpdateDeadLabel(int value)
        {
            dead_label.Text = "Animals dead: " + value.ToString();
        }
        #endregion


        private void FieldPictureBox_Click(object sender, MouseEventArgs e)
        {
            Point location = e.Location;

            _game.GetAnimalInfo(new Point(location.X / cellSize, location.Y / cellSize));
        }

        public void  OpenInfoForm()
        {
            InfoForm = new InfoForm();
            IsInfoFormOpened = true;
            DialogResult dr = InfoForm.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                CloseInfoForm();
            }
        }

        public void CloseInfoForm()
        {
            InfoForm.Close();
            IsInfoFormOpened = false;
        }
    }
}
