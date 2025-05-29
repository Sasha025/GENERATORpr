namespace PrometeusWayReferenceControlWindow
{
    using PrometeusWayReferenceControl;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class Form1 : Form
    {
        private IContainer components;
        private ListBox listBox1;
        private ListBox listBox2;

        public Form1()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            MapRoutes routes = new MapRoutes();
            routes.CreateRoutes(((FormMain) base.Owner).Map.stationMap);
            stopwatch.Stop();
            this.Text = $"{"Простые маршруты"} [{routes.Count}] (ms:{stopwatch.ElapsedMilliseconds})";
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
            foreach (MapRoute route in routes)
            {
                this.listBox1.Items.Add(route);
            }
        }

        private void InitializeComponent()
        {
            this.listBox1 = new ListBox();
            this.listBox2 = new ListBox();
            base.SuspendLayout();
            this.listBox1.Dock = DockStyle.Left;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new Size(0x189, 0x13c);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox2.Dock = DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new Point(0x189, 0);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new Size(0x198, 0x13c);
            this.listBox2.TabIndex = 1;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x321, 320);
            base.Controls.Add(this.listBox2);
            base.Controls.Add(this.listBox1);
            base.Name = "Form1";
            this.Text = "Простые маршруты";
            base.Shown += new EventHandler(this.Form1_Shown);
            base.ResumeLayout(false);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
            {
                this.listBox2.Items.Clear();
                MapRoute selectedItem = (MapRoute) this.listBox1.SelectedItem;
                int num = 0;
                while (true)
                {
                    if (num >= selectedItem.lines.Count)
                    {
                        ((FormMain) base.Owner).Map.stationMap.DeselectAll();
                        ((FormMain) base.Owner).Map.stationMap.lines.selectedLines.AddRange(selectedItem.lines);
                        ((FormMain) base.Owner).Refresh();
                        break;
                    }
                    this.listBox2.Items.Add(selectedItem.lines[num]);
                    num++;
                }
            }
        }
    }
}

