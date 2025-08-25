using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SandWindowsForms
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(Form1 mainForm)
        {
            InitializeComponent();
            this.BackColor = Color.YellowGreen;
            System.Drawing.Drawing2D.GraphicsPath starPath = new GraphicsPath();
            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;
            double radiusOuter = Math.Min(ClientSize.Width, ClientSize.Height) * 0.45;
            double radiusInner = radiusOuter * 0.35;
            for (int i = 0; i < 8; i++)
            {
                double angleOuter = i * Math.PI / 4 + Math.PI / 8;
                double xOuter = centerX + radiusOuter * Math.Cos(angleOuter);
                double yOuter = centerY + radiusOuter * Math.Sin(angleOuter);
                if (i > 0 || starPath.PointCount == 0)
                    starPath.AddLine((float)xOuter, (float)yOuter, (float)xOuter, (float)yOuter);
                double angleInner = i * Math.PI / 4 + Math.PI / 8 + Math.PI / 8;
                double xInner = centerX + radiusInner * Math.Cos(angleInner);
                double yInner = centerY + radiusInner * Math.Sin(angleInner);
                starPath.AddLine((float)xOuter, (float)yOuter, (float)xInner, (float)yInner);
            }
            starPath.CloseFigure();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = new Region(starPath);
            var button = new Button()
            {
                Name = "button3",
                Text = "(::)",
                Size = new Size(50, 50),
                BackColor = Color.Blue,
                ForeColor = Color.Yellow
            };
            button.Left = (ClientSize.Width - button.Width) / 2;
            button.Top = (ClientSize.Height - button.Height) / 2;
            button.MouseDown += Button_MouseDown;
            this.Controls.Add(button);
        }

        private void Button_MouseDown(object? sender, MouseEventArgs e)
        {
            Controls.Remove((sender as Button));
            this.Close();
        }
    }
}
