using System;
using System.Drawing;
namespace starcraftbot.proxybot
{
	/// <summary> GUI for showing the ProxyBot's view of the game state.</summary>
	[Serializable]
	public class StarCraftFrame:System.Windows.Forms.Panel
	{
		
		/// <summary> </summary>
		private const long serialVersionUID = - 7819869083135025772L;
		
		/// <summary>draw object IDs </summary>
		private static bool drawIDs = true;
		
		/// <summary>draw the starting locations </summary>
		private static bool drawStartSpots = true;
		
		/// <summary>draw mineral and gas locations </summary>
		private static bool drawResources = true;
		
		/// <summary>reference to the proxy bot </summary>
		private ProxyBot proxyBot;
		
		/// <summary>pixels per map tile, StarCraft is 32 </summary>
		private int tileSize = 6;
		
		/// <summary>height of the resource panel </summary>
		private int panelHeight = 30;
		
		/// <summary>font size for unit ids </summary>
		private int textSize = 8;
		
		/// <summary> Constructs a JFrame and draws the ProxyBot's state.</summary>
		public StarCraftFrame()
		{
			proxyBot = ProxyBot.Proxy;
			
			int width = proxyBot.Map.MapWidth;
			int height = proxyBot.Map.MapHeight;
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setPreferredSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			Size = new System.Drawing.Size(tileSize * width, tileSize * height + panelHeight);
			
			System.Windows.Forms.Form frame = SupportClass.FormSupport.CreateForm("Proxy Bot");
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			frame.Controls.Add(this);
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
            //TODO:Fix size
			//frame.pack();
			frame.Closing += new System.ComponentModel.CancelEventHandler(this.StarCraftFrame_Closing_EXIT_ON_CLOSE);
			//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_int_int'"
			frame.Location = new System.Drawing.Point(640, 0);
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			frame.Visible = true;
		}
		
		public virtual void  start()
		{
			while (true)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 25));
					//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
					Refresh();
				}
				catch (System.Exception e)
				{
				}
			}
		}
		
		protected override void  OnPaint(System.Windows.Forms.PaintEventArgs g_EventArg)
		{
			System.Drawing.Graphics g = null;
			if (g_EventArg != null)
				g = g_EventArg.Graphics;
			if (proxyBot.Units == null)
			{
				return ;
			}
			
			// tile set
			Map map = proxyBot.Map;
			for (int y = 0; y < map.MapHeight; y++)
			{
				for (int x = 0; x < map.MapWidth; x++)
				{
					int walkable = map.isWalkable(x, y)?1:0;
					int buildable = map.isBuildable(x, y)?1:0;
					int height = map.getHeight(x, y);
					
					int c = (70 * walkable + 60 * buildable + 50 * height);
					SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(c, c, (int) (c * 3 / 4)));
					g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), x * tileSize, panelHeight + y * tileSize, tileSize, tileSize);
				}
			}
			
			// Starting locations
			if (drawStartSpots)
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				foreach(StartingLocation location in proxyBot.StartingLocations)
				{
					g.FillRectangle(Brushes.Orange, location.X * tileSize, panelHeight + location.Y * tileSize, 4 * tileSize, 3 * tileSize);
				}
			}
			
			// minerals
			if (drawResources)
			{
                SolidBrush sb = new SolidBrush(System.Drawing.Color.FromArgb(0, 255, 255));
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				foreach(Unit unit in proxyBot.Units)
				{
					if (unit.Type.Id == Constants.Resource_Mineral_Field)
					{
                        g.FillRectangle(sb, unit.X * tileSize, panelHeight + unit.Y * tileSize, tileSize, tileSize);
					}
				}
			}
			
			// gas
			if (drawResources)
			{
                SolidBrush sb = new SolidBrush(System.Drawing.Color.FromArgb(0, 128, 0));
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				foreach(Unit unit in proxyBot.Units)
				{
					if (unit.Type.Id == Constants.Resource_Vespene_Geyser)
					{
                        g.FillRectangle(sb, unit.X * tileSize, panelHeight + unit.Y * tileSize, unit.Type.TileWidth * tileSize, unit.Type.TileHeight * tileSize);
					}
				}
			}
			
			// enemy units
            SolidBrush sb2 = new SolidBrush(System.Drawing.Color.FromArgb(255, 0, 0));
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			foreach(Unit unit in proxyBot.Units)
			{
				if (unit.PlayerID == proxyBot.EnemyID)
				{
                    g.FillRectangle(sb2, unit.X * tileSize, panelHeight + unit.Y * tileSize, unit.Type.TileWidth * tileSize, unit.Type.TileHeight * tileSize);
				}
			}
			
			// player units
            SolidBrush sb3 = new SolidBrush(System.Drawing.Color.FromArgb(0, 255, 0));
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			foreach(Unit unit in proxyBot.Units)
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
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				foreach(Unit unit in proxyBot.Units)
				{
					g.DrawString("" + unit.ID, f, sb4,  unit.X * tileSize, panelHeight + unit.Y * tileSize + textSize - 2);
				}
			}
		}
		private void  StarCraftFrame_Closing_EXIT_ON_CLOSE(System.Object sender, System.ComponentModel.CancelEventArgs  e)
		{
			e.Cancel = true;
			SupportClass.CloseOperation((System.Windows.Forms.Form) sender, 3);
		}
	}
}