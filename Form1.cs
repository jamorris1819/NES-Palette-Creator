using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace NES_Palette_Generator
{
    public partial class Form1 : Form
    {
        Dictionary<string, Color> colors;
        List<TextBox> boxes;

        public Form1()
        {
            InitializeComponent();
            boxes = new List<TextBox>();
            colors = new Dictionary<string, Color>();
            // Load in RGB data for the colours
            string s = "124,124,124;0,0,252;0,0,188;68,40,188;148,0,132;168,0,32;168,16,0;136,20,0;80,48,0;0,120,0;0,104,0;0,88,0;0,64,88;0,0,0;0,0,0;0,0,0;188,188,188;0,120,248;0,88,248;104,68,252;216,0,204;228,0,88;248,56,0;228,92,16;172,124,0;0,184,0;0,168,0;0,168,68;0,136,136;0,0,0;0,0,0;0,0,0;248,248,248;60,188,252;104,136,252;152,120,248;248,120,248;248,88,152;248,120,88;252,160,68;248,184,0;184,248,24;88,216,84;88,248,152;0,232,216;120,120,120;0,0,0;0,0,0;252,252,252;164,228,252;184,184,248;216,184,248;248,184,248;248,164,192;240,208,176;252,224,168;248,216,120;216,248,120;184,248,184;184,248,216;0,252,252;248,216,248;0,0,0;0,0,0";
            string[] parts = s.Split(';');
            int counter = 0;
            foreach (string part in parts)
            {
                string[] vals = part.Split(',');
                byte r = byte.Parse(vals[0]);
                byte g = byte.Parse(vals[1]);
                byte b = byte.Parse(vals[2]);
                Color c = Color.FromArgb(r, g, b);
                colors.Add(counter.ToString("X"), c);
                counter++;
            }

            tbS11.TextChanged += SetBackground;
            tbS21.TextChanged += SetBackground;
            tbS31.TextChanged += SetBackground;
            tbS41.TextChanged += SetBackground;

            tbB11.TextChanged += SetBackground;
            tbB21.TextChanged += SetBackground;
            tbB31.TextChanged += SetBackground;
            tbB41.TextChanged += SetBackground;

            boxes.Add(tbS11);
            boxes.Add(tbS12);
            boxes.Add(tbS13);
            boxes.Add(tbS14);
            boxes.Add(tbS21);
            boxes.Add(tbS22);
            boxes.Add(tbS23);
            boxes.Add(tbS24);
            boxes.Add(tbS31);
            boxes.Add(tbS32);
            boxes.Add(tbS33);
            boxes.Add(tbS34);
            boxes.Add(tbS41);
            boxes.Add(tbS42);
            boxes.Add(tbS43);
            boxes.Add(tbS44);


            boxes.Add(tbB11);
            boxes.Add(tbB12);
            boxes.Add(tbB13);
            boxes.Add(tbB14);
            boxes.Add(tbB21);
            boxes.Add(tbB22);
            boxes.Add(tbB23);
            boxes.Add(tbB24);
            boxes.Add(tbB31);
            boxes.Add(tbB32);
            boxes.Add(tbB33);
            boxes.Add(tbB34);
            boxes.Add(tbB41);
            boxes.Add(tbB42);
            boxes.Add(tbB43);
            boxes.Add(tbB44);
            foreach (TextBox tb in boxes)
            {
                tb.TextChanged += Update;
            }
        }

        private void Update(object sender, EventArgs e)
        {
            foreach (TextBox tbox in boxes)
            {
                if (colors.ContainsKey(tbox.Text))
                    tbox.BackColor = colors[tbox.Text];
                else
                    tbox.BackColor = Color.White;
            }
        }

        private void SetBackground(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;
            tbS11.Text = text;
            tbS21.Text = text;
            tbS31.Text = text;
            tbS41.Text = text;
            
            tbB11.Text = text;
            tbB21.Text = text;
            tbB31.Text = text;
            tbB41.Text = text;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear?", "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (TextBox tbox in boxes)
                {
                    tbox.Text = "";
                    tbox.BackColor = Color.White;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[32];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(char)int.Parse(boxes[i].Text, System.Globalization.NumberStyles.HexNumber);
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PAL files (*.pal)|*.pal";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                File.WriteAllBytes(sfd.FileName, data);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PAL files (*.pal)|*.pal";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    byte[] data = File.ReadAllBytes(ofd.FileName);
                    for (int i = 0; i < data.Length; i++)
                        boxes[i].Text = ((int)data[i]).ToString("X");
                }
                catch
                {
                    MessageBox.Show("Invalid data", "Error");
                }
            }
        }
    }
}
