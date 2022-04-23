
using System.Windows.Forms;

namespace OOP_LifeSimulation
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FieldPictureBox = new System.Windows.Forms.PictureBox();
            this.ok_label = new System.Windows.Forms.Label();
            this.lookingForFood_label = new System.Windows.Forms.Label();
            this.dead_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FieldPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // FieldPictureBox
            // 
            this.FieldPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.FieldPictureBox.Location = new System.Drawing.Point(0, 23);
            this.FieldPictureBox.Name = "FieldPictureBox";
            this.FieldPictureBox.Size = new System.Drawing.Size(22000, 22000);
            this.FieldPictureBox.TabIndex = 0;
            this.FieldPictureBox.TabStop = false;
            this.FieldPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FieldPictureBox_Click);
            // 
            // ok_label
            // 
            this.ok_label.AutoSize = true;
            this.ok_label.BackColor = System.Drawing.Color.Transparent;
            this.ok_label.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ok_label.ForeColor = System.Drawing.Color.Red;
            this.ok_label.Location = new System.Drawing.Point(0, 0);
            this.ok_label.Name = "ok_label";
            this.ok_label.Size = new System.Drawing.Size(152, 21);
            this.ok_label.TabIndex = 1;
            this.ok_label.Text = "Animals feeling well:";
            // 
            // lookingForFood_label
            // 
            this.lookingForFood_label.AutoSize = true;
            this.lookingForFood_label.BackColor = System.Drawing.Color.Transparent;
            this.lookingForFood_label.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lookingForFood_label.ForeColor = System.Drawing.Color.Orange;
            this.lookingForFood_label.Location = new System.Drawing.Point(210, 0);
            this.lookingForFood_label.Name = "lookingForFood_label";
            this.lookingForFood_label.Size = new System.Drawing.Size(185, 21);
            this.lookingForFood_label.TabIndex = 2;
            this.lookingForFood_label.Text = "Animals looking for food:";
            // 
            // dead_label
            // 
            this.dead_label.AutoSize = true;
            this.dead_label.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dead_label.Location = new System.Drawing.Point(460, 0);
            this.dead_label.Name = "dead_label";
            this.dead_label.Size = new System.Drawing.Size(107, 21);
            this.dead_label.TabIndex = 3;
            this.dead_label.Text = "Animals dead:";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dead_label);
            this.Controls.Add(this.lookingForFood_label);
            this.Controls.Add(this.ok_label);
            this.Controls.Add(this.FieldPictureBox);
            this.Name = "GameForm";
            this.Text = "Life simulation";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.FieldPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public PictureBox FieldPictureBox;
        private Label ok_label;
        private Label lookingForFood_label;
        private Label dead_label;
    }
}