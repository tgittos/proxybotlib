using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using starcraftbot.proxybot;

namespace StarCraftBot_net.proxybot
{
    public partial class StarCraftFrame : Form
    {
        public StarCraftFrame(ProxyBot pProxy)
        {
            InitializeComponent();



            proxyBot = pProxy;


            this.Paint += new System.Windows.Forms.PaintEventHandler(StarCraftFrame_Paint);
            this.Load += new EventHandler(StarCraftFrame_Load);
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;
        }

        
		/// <summary>draw object IDs </summary>
		private static bool drawIDs = true;
		
		/// <summary>draw the starting locations </summary>
		private static bool drawStartSpots = true;
		
		/// <summary>draw mineral and gas locations </summary>
		private static bool drawResources = true;
		
		/// <summary>reference to the proxy bot </summary>
		private ProxyBot proxyBot;
		
		/// <summary>pixels per map tile, StarCraft is 32 </summary>
		private int tileSize = 3;
		
		/// <summary>height of the resource panel </summary>
		private int panelHeight = 30;
		
		/// <summary>font size for unit ids </summary>
		private int textSize = 8;
		


        void StarCraftFrame_Load(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            int widthW = proxyBot.Map.MapWidth;
            int heightW = proxyBot.Map.MapHeight;
            ClientSize = new System.Drawing.Size(tileSize * widthW, tileSize * heightW + panelHeight);
        }

        void StarCraftFrame_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            


            System.Drawing.Graphics g = e.Graphics;

            if (proxyBot.Units == null)
            {
                return;
            }

            SetSize();

            // tile set
            Map map = proxyBot.Map;
            for (int y = 0; y < map.MapHeight; y++)
            {
                for (int x = 0; x < map.MapWidth; x++)
                {
                    int walkable = map.isWalkable(x, y) ? 1 : 0;
                    int buildable = map.isBuildable(x, y) ? 1 : 0;
                    int height = map.getHeight(x, y);
                    Brush c = GetMapBackColor(walkable, buildable, height);
                    //int c = (70 * walkable + 60 * buildable + 50 * height);
                    //SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(c, c, (int)(c * 3 / 4)));
                    g.FillRectangle(c, x * tileSize, panelHeight + y * tileSize, tileSize, tileSize);
                }
            }

            // Starting locations
            if (drawStartSpots)
            {
                foreach (StartingLocation location in proxyBot.StartingLocations)
                {
                    g.FillRectangle(Brushes.Orange, location.X * tileSize, panelHeight + location.Y * tileSize, 4 * tileSize, 3 * tileSize);
                }
            }

            // minerals
            if (drawResources)
            {
                SolidBrush sb = new SolidBrush(System.Drawing.Color.FromArgb(0, 255, 255));
                foreach (Unit unit in proxyBot.Units)
                {
                    if (unit.Type.ID == Constants.Resource_Mineral_Field)
                    {
                        g.FillRectangle(sb, unit.X * tileSize, panelHeight + unit.Y * tileSize, tileSize, tileSize);
                    }
                }
            }

            // gas
            if (drawResources)
            {
                SolidBrush sb = new SolidBrush(System.Drawing.Color.FromArgb(0, 128, 0));
                foreach (Unit unit in proxyBot.Units)
                {
                    if (unit.Type.ID == Constants.Resource_Vespene_Geyser)
                    {
                        g.FillRectangle(sb, unit.X * tileSize, panelHeight + unit.Y * tileSize, unit.Type.TileWidth * tileSize, unit.Type.TileHeight * tileSize);
                    }
                }
            }

            // enemy units
            SolidBrush sb2 = new SolidBrush(System.Drawing.Color.FromArgb(255, 0, 0));
            foreach (Unit unit in proxyBot.Units)
            {
                if (unit.PlayerID == proxyBot.EnemyID)
                {
                    g.FillRectangle(sb2, unit.X * tileSize, panelHeight + unit.Y * tileSize, unit.Type.TileWidth * tileSize, unit.Type.TileHeight * tileSize);
                }
            }

            // player units
            SolidBrush sb3 = new SolidBrush(System.Drawing.Color.FromArgb(0, 255, 0));
            foreach (Unit unit in proxyBot.Units)
            {
                if (unit.PlayerID == proxyBot.PlayerID)
                {
                    g.FillRectangle(sb3, unit.X * tileSize, panelHeight + unit.Y * tileSize, unit.Type.TileWidth * tileSize, unit.Type.TileHeight * tileSize);
                }
            }

            // status panel
            SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(255, 255, 255));
            g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), 0, 0, Width, panelHeight);

            // minerals
            SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(0, 0, 255));
            g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), 5, 10, 10, 10);
            SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(0, 0, 0));
            g.DrawRectangle(SupportClass.GraphicsManager.manager.GetPen(g), 5, 10, 10, 10);
            SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(0, 0, 0));
            //UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
            g.DrawString("" + proxyBot.Player.Minerals, SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), 25, 20 - SupportClass.GraphicsManager.manager.GetFont(g).Height);

            // gas
            SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(0, 255, 0));
            g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), 105, 10, 10, 10);
            SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(0, 0, 0));
            g.DrawRectangle(SupportClass.GraphicsManager.manager.GetPen(g), 105, 10, 10, 10);
            SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(0, 0, 0));
            //UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
            g.DrawString("" + proxyBot.Player.Gas, SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), 125, 20 - SupportClass.GraphicsManager.manager.GetFont(g).Height);

            // supply
            SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(0, 0, 0));
            //UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
            g.DrawString((proxyBot.Player.SupplyUsed / 2) + "/" + (proxyBot.Player.SupplyTotal / 2), SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), 200, 20 - SupportClass.GraphicsManager.manager.GetFont(g).Height);

            // unit IDs
            if (drawIDs)
            {
                SolidBrush sb4 = new SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255));
                //UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1075'"
                Font f = new System.Drawing.Font("ariel", textSize, System.Drawing.FontStyle.Regular);
                foreach (Unit unit in proxyBot.Units)
                {
                    g.DrawString("" + unit.ID, f, sb4, unit.X * tileSize, panelHeight + unit.Y * tileSize + textSize - 2);
                }
            }
        }

        

        private Brush GetMapBackColor(int walkable, int buildable, int height)
        {
            if (walkable == 1)
            {
                if (buildable == 1)
                {
                    return GetColorLighterIfHigher(Color.Black, height);

                }
                else
                {
                    return GetColorLighterIfHigher(Color.Gray, height);
                }
            }
            else
            {
                return GetColorLighterIfHigher(Color.Yellow, height);
            }

        }
        Dictionary<int, Brush> brushCache = new Dictionary<int, Brush>();

        private Brush GetColorLighterIfHigher(Color brush, int height)
        {
            float lighness = (3-height)/2;
            float r = brush.R * lighness;
            float g = brush.G * lighness;
            float b = brush.B * lighness;
            Color res = Color.FromArgb(brush.A, (int)r, (int)g, (int)b);
            int colorCode = res.ToArgb();
            if (brushCache.ContainsKey(colorCode))
            {
                return brushCache[colorCode];
            }
            else
            {
                Brush br = new SolidBrush(res);
                brushCache.Add(colorCode, br);
                return br;
            }



        }
		


		private void  StarCraftFrame_Closing_EXIT_ON_CLOSE(System.Object sender, System.ComponentModel.CancelEventArgs  e)
		{
			e.Cancel = true;
			SupportClass.CloseOperation((System.Windows.Forms.Form) sender, 3);
		}
    }
}
