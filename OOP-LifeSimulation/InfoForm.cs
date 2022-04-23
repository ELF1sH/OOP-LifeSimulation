using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OOP_LifeSimulation
{
    public partial class InfoForm : Form
    {
        public int counter = 0;

        private readonly List<Control> labels = new List<Control>();
        private readonly List<Control> infoLabels = new List<Control>();

        public InfoForm()
        {
            InitializeComponent();
            labels.Add(label1);
            labels.Add(label2);
            labels.Add(label3);
            labels.Add(label4);
            labels.Add(label5);
            labels.Add(label6);
            labels.Add(label7);
            labels.Add(label8);
            infoLabels.Add(label1Info);
            infoLabels.Add(label2Info);
            infoLabels.Add(label3Info);
            infoLabels.Add(label4Info);
            infoLabels.Add(label5Info);
            infoLabels.Add(label6Info);
            infoLabels.Add(label7Info);
            infoLabels.Add(label8Info);
        }

        public void UpdateLabel(List<string> dataList)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                infoLabels[i].Visible = true;
                labels[i].Text = dataList[i];
            }
        }
    }
}
