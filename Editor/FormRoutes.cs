namespace PrometeusWayReferenceControlWindow
{
    using PrometeusWayReferenceControl;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class FormRoutes : Form
    {
        private Routes routes = new Routes();
        private IContainer components;
        private Panel panel1;
        private ListBox lbRouteInfo;
        private ListBox lbRoutes;

        public FormRoutes(string FileName)
        {
            this.InitializeComponent();
            this.lbRoutes.Items.Clear();
            this.lbRouteInfo.Items.Clear();
            if (!File.Exists(FileName))
            {
                this.lbRouteInfo.Items.Add("файл не найден");
            }
            else
            {
                string[] strArray = File.ReadAllLines(FileName);
                int num = 1;
                RouteInfo item = new RouteInfo();
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i] != "")
                    {
                        item.strings.Add(strArray[i]);
                        if (strArray[i].Contains("line id = "))
                        {
                            string s = strArray[i].Substring(10, strArray[i].IndexOf(",") - 10);
                            item.ids.Add(int.Parse(s));
                        }
                    }
                    else
                    {
                        if (item != null)
                        {
                            this.routes.Add(item);
                            this.lbRoutes.Items.Add(item);
                        }
                        item.id = num;
                        num++;
                        item = new RouteInfo();
                    }
                }
            }
            if (this.lbRoutes.Items.Count > 0)
            {
                this.lbRoutes.SelectedIndex = 0;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.lbRouteInfo = new ListBox();
            this.lbRoutes = new ListBox();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.lbRouteInfo);
            this.panel1.Controls.Add(this.lbRoutes);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x22b, 0x107);
            this.panel1.TabIndex = 0;
            this.lbRouteInfo.Dock = DockStyle.Fill;
            this.lbRouteInfo.FormattingEnabled = true;
            this.lbRouteInfo.Location = new Point(120, 0);
            this.lbRouteInfo.Name = "lbRouteInfo";
            this.lbRouteInfo.Size = new Size(0x1b3, 0xfb);
            this.lbRouteInfo.TabIndex = 1;
            this.lbRoutes.Dock = DockStyle.Left;
            this.lbRoutes.FormattingEnabled = true;
            this.lbRoutes.Location = new Point(0, 0);
            this.lbRoutes.Name = "lbRoutes";
            this.lbRoutes.Size = new Size(120, 0xfb);
            this.lbRoutes.TabIndex = 0;
            this.lbRoutes.SelectedIndexChanged += new EventHandler(this.lbRoutes_SelectedIndexChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x22b, 0x107);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "FormRoutes";
            this.Text = "Маршруты";
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void lbRoutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lbRoutes.SelectedIndex >= 0)
            {
                this.lbRouteInfo.Items.Clear();
                int num = 0;
                while (true)
                {
                    if (num >= this.routes[this.lbRoutes.SelectedIndex].strings.Count)
                    {
                        if (base.Owner != null)
                        {
                            FormMain owner = (FormMain) base.Owner;
                            owner.Map.stationMap.DeselectAll();
                            int num2 = 0;
                            while (true)
                            {
                                if (num2 >= this.routes[this.lbRoutes.SelectedIndex].ids.Count)
                                {
                                    owner.Refresh();
                                    break;
                                }
                                MapLine item = owner.Map.stationMap.lines.Exists((int) this.routes[this.lbRoutes.SelectedIndex].ids[num2]);
                                if (item != null)
                                {
                                    owner.Map.stationMap.lines.selectedLines.Add(item);
                                }
                                num2++;
                            }
                        }
                        break;
                    }
                    this.lbRouteInfo.Items.Add(this.routes[this.lbRoutes.SelectedIndex].strings[num]);
                    num++;
                }
            }
        }

        private class RouteInfo
        {
            public int id;
            public ArrayList ids = new ArrayList();
            public ArrayList strings = new ArrayList();

            public override string ToString() => 
                $"Route # {this.id}";
        }

        private class Routes : List<FormRoutes.RouteInfo>
        {
        }
    }
}

