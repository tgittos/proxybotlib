using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyBotLib.GUI
{
    public class GraphicsManager
    {
        //Instance of GDI+ drawing surfaces graphics hashtable
        static public GraphicsHashTable manager = new GraphicsHashTable();

        /// <summary>
        /// Creates a new Graphics object from the device context handle associated with the Graphics
        /// parameter
        /// </summary>
        /// <param name="oldGraphics">Graphics instance to obtain the parameter from</param>
        /// <returns>A new GDI+ drawing surface</returns>
        public static System.Drawing.Graphics CreateGraphics(System.Drawing.Graphics oldGraphics)
        {
            System.Drawing.Graphics createdGraphics;
            System.IntPtr hdc = oldGraphics.GetHdc();
            createdGraphics = System.Drawing.Graphics.FromHdc(hdc);
            oldGraphics.ReleaseHdc(hdc);
            return createdGraphics;
        }

        /// <summary>
        /// This method draws a Bezier curve.
        /// </summary>
        /// <param name="graphics">It receives the Graphics instance</param>
        /// <param name="array">An array of (x,y) pairs of coordinates used to draw the curve.</param>
        public static void Bezier(System.Drawing.Graphics graphics, int[] array)
        {
            System.Drawing.Pen pen;
            pen = GraphicsManager.manager.GetPen(graphics);
            try
            {
                graphics.DrawBezier(pen, array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]);
            }
            catch (System.IndexOutOfRangeException e)
            {
                throw new System.IndexOutOfRangeException(e.ToString());
            }
        }

        /// <summary>
        /// Gets the text size width and height from a given GDI+ drawing surface and a given font
        /// </summary>
        /// <param name="graphics">Drawing surface to use</param>
        /// <param name="graphicsFont">Font type to measure</param>
        /// <param name="text">String of text to measure</param>
        /// <returns>A point structure with both size dimentions; x for width and y for height</returns>
        public static System.Drawing.Point GetTextSize(System.Drawing.Graphics graphics, System.Drawing.Font graphicsFont, System.String text)
        {
            System.Drawing.Point textSize;
            System.Drawing.SizeF tempSizeF;
            tempSizeF = graphics.MeasureString(text, graphicsFont);
            textSize = new System.Drawing.Point();
            textSize.X = (int)tempSizeF.Width;
            textSize.Y = (int)tempSizeF.Height;
            return textSize;
        }

        /// <summary>
        /// Gets the text size width and height from a given GDI+ drawing surface and a given font
        /// </summary>
        /// <param name="graphics">Drawing surface to use</param>
        /// <param name="graphicsFont">Font type to measure</param>
        /// <param name="text">String of text to measure</param>
        /// <param name="width">Maximum width of the string</param>
        /// <param name="format">StringFormat object that represents formatting information, such as line spacing, for the string</param>
        /// <returns>A point structure with both size dimentions; x for width and y for height</returns>
        public static System.Drawing.Point GetTextSize(System.Drawing.Graphics graphics, System.Drawing.Font graphicsFont, System.String text, System.Int32 width, System.Drawing.StringFormat format)
        {
            System.Drawing.Point textSize;
            System.Drawing.SizeF tempSizeF;
            tempSizeF = graphics.MeasureString(text, graphicsFont, width, format);
            textSize = new System.Drawing.Point();
            textSize.X = (int)tempSizeF.Width;
            textSize.Y = (int)tempSizeF.Height;
            return textSize;
        }

        /// <summary>
        /// Gives functionality over a hashtable of GDI+ drawing surfaces
        /// </summary>
        public class GraphicsHashTable : System.Collections.Hashtable
        {
            /// <summary>
            /// Gets the graphics object from the given control
            /// </summary>
            /// <param name="control">Control to obtain the graphics from</param>
            /// <returns>A graphics object with the control's characteristics</returns>
            public System.Drawing.Graphics GetGraphics(System.Windows.Forms.Control control)
            {
                System.Drawing.Graphics graphic;
                if (control.Visible == true)
                {
                    graphic = control.CreateGraphics();
                    SetColor(graphic, control.ForeColor);
                    SetFont(graphic, control.Font);
                }
                else
                {
                    graphic = null;
                }
                return graphic;
            }

            /// <summary>
            /// Sets the background color property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given background color.
            /// </summary>
            /// <param name="graphic">Graphic element to search or add</param>
            /// <param name="color">Background color to set</param>
            public void SetBackColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties)this[graphic]).BackColor = color;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.BackColor = color;
                    Add(graphic, tempProps);
                }
            }

            /// <summary>
            /// Gets the background color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns White.
            /// </summary>
            /// <param name="graphic">Graphic element to search</param>
            /// <returns>The background color of the graphic</returns>
            public System.Drawing.Color GetBackColor(System.Drawing.Graphics graphic)
            {
                if (this[graphic] == null)
                    return System.Drawing.Color.White;
                else
                    return ((GraphicsProperties)this[graphic]).BackColor;
            }

            /// <summary>
            /// Sets the text color property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given text color.
            /// </summary>
            /// <param name="graphic">Graphic element to search or add</param>
            /// <param name="color">Text color to set</param>
            public void SetTextColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties)this[graphic]).TextColor = color;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.TextColor = color;
                    Add(graphic, tempProps);
                }
            }

            /// <summary>
            /// Gets the text color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns White.
            /// </summary>
            /// <param name="graphic">Graphic element to search</param>
            /// <returns>The text color of the graphic</returns>
            public System.Drawing.Color GetTextColor(System.Drawing.Graphics graphic)
            {
                if (this[graphic] == null)
                    return System.Drawing.Color.White;
                else
                    return ((GraphicsProperties)this[graphic]).TextColor;
            }

            /// <summary>
            /// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
            /// </summary>
            /// <param name="graphic">Graphic element to search or add</param>
            /// <param name="brush">GraphicBrush to set</param>
            public void SetBrush(System.Drawing.Graphics graphic, System.Drawing.SolidBrush brush)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties)this[graphic]).GraphicBrush = brush;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.GraphicBrush = brush;
                    Add(graphic, tempProps);
                }
            }

            /// <summary>
            /// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
            /// </summary>
            /// <param name="graphic">Graphic element to search or add</param>
            /// <param name="brush">GraphicBrush to set</param>
            public void SetPaint(System.Drawing.Graphics graphic, System.Drawing.Brush brush)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties)this[graphic]).PaintBrush = brush;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.PaintBrush = brush;
                    Add(graphic, tempProps);
                }
            }

            /// <summary>
            /// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
            /// </summary>
            /// <param name="graphic">Graphic element to search or add</param>
            /// <param name="color">Color to set</param>
            public void SetPaint(System.Drawing.Graphics graphic, System.Drawing.Color color)
            {
                System.Drawing.Brush brush = new System.Drawing.SolidBrush(color);
                if (this[graphic] != null)
                    ((GraphicsProperties)this[graphic]).PaintBrush = brush;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.PaintBrush = brush;
                    Add(graphic, tempProps);
                }
            }


            /// <summary>
            /// Gets the HatchBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Blank.
            /// </summary>
            /// <param name="graphic">Graphic element to search</param>
            /// <returns>The HatchBrush setting of the graphic</returns>
            public System.Drawing.Drawing2D.HatchBrush GetBrush(System.Drawing.Graphics graphic)
            {
                if (this[graphic] == null)
                    return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid, System.Drawing.Color.Black, System.Drawing.Color.Black);
                else
                    return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid, ((GraphicsProperties)this[graphic]).GraphicBrush.Color, ((GraphicsProperties)this[graphic]).GraphicBrush.Color);
            }

            /// <summary>
            /// Gets the HatchBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Blank.
            /// </summary>
            /// <param name="graphic">Graphic element to search</param>
            /// <returns>The Brush setting of the graphic</returns>
            public System.Drawing.Brush GetPaint(System.Drawing.Graphics graphic)
            {
                if (this[graphic] == null)
                    return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid, System.Drawing.Color.Black, System.Drawing.Color.Black);
                else
                    return ((GraphicsProperties)this[graphic]).PaintBrush;
            }

            /// <summary>
            /// Sets the GraphicPen property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given Pen.
            /// </summary>
            /// <param name="graphic">Graphic element to search or add</param>
            /// <param name="pen">Pen to set</param>
            public void SetPen(System.Drawing.Graphics graphic, System.Drawing.Pen pen)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties)this[graphic]).GraphicPen = pen;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.GraphicPen = pen;
                    Add(graphic, tempProps);
                }
            }

            /// <summary>
            /// Gets the GraphicPen property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Black.
            /// </summary>
            /// <param name="graphic">Graphic element to search</param>
            /// <returns>The GraphicPen setting of the graphic</returns>
            public System.Drawing.Pen GetPen(System.Drawing.Graphics graphic)
            {
                if (this[graphic] == null)
                    return System.Drawing.Pens.Black;
                else
                    return ((GraphicsProperties)this[graphic]).GraphicPen;
            }

            /// <summary>
            /// Sets the GraphicFont property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given Font.
            /// </summary>
            /// <param name="graphic">Graphic element to search or add</param>
            /// <param name="Font">Font to set</param>
            public void SetFont(System.Drawing.Graphics graphic, System.Drawing.Font font)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties)this[graphic]).GraphicFont = font;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.GraphicFont = font;
                    Add(graphic, tempProps);
                }
            }

            /// <summary>
            /// Gets the GraphicFont property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Microsoft Sans Serif with size 8.25.
            /// </summary>
            /// <param name="graphic">Graphic element to search</param>
            /// <returns>The GraphicFont setting of the graphic</returns>
            public System.Drawing.Font GetFont(System.Drawing.Graphics graphic)
            {
                if (this[graphic] == null)
                    return new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                else
                    return ((GraphicsProperties)this[graphic]).GraphicFont;
            }

            /// <summary>
            /// Sets the color properties for a given Graphics object. If the element doesn't exist, then it adds the graphic element to the hashtable with the color properties set with the given value.
            /// </summary>
            /// <param name="graphic">Graphic element to search or add</param>
            /// <param name="color">Color value to set</param>
            public void SetColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
            {
                if (this[graphic] != null)
                {
                    ((GraphicsProperties)this[graphic]).GraphicPen.Color = color;
                    ((GraphicsProperties)this[graphic]).GraphicBrush.Color = color;
                    ((GraphicsProperties)this[graphic]).color = color;
                }
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.GraphicPen.Color = color;
                    tempProps.GraphicBrush.Color = color;
                    tempProps.color = color;
                    Add(graphic, tempProps);
                }
            }

            /// <summary>
            /// Gets the color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Black.
            /// </summary>
            /// <param name="graphic">Graphic element to search</param>
            /// <returns>The color setting of the graphic</returns>
            public System.Drawing.Color GetColor(System.Drawing.Graphics graphic)
            {
                if (this[graphic] == null)
                    return System.Drawing.Color.Black;
                else
                    return ((GraphicsProperties)this[graphic]).color;
            }

            /// <summary>
            /// This method gets the TextBackgroundColor of a Graphics instance
            /// </summary>
            /// <param name="graphic">The graphics instance</param>
            /// <returns>The color value in ARGB encoding</returns>
            public System.Drawing.Color GetTextBackgroundColor(System.Drawing.Graphics graphic)
            {
                if (this[graphic] == null)
                    return System.Drawing.Color.Black;
                else
                {
                    return ((GraphicsProperties)this[graphic]).TextBackgroundColor;
                }
            }

            /// <summary>
            /// This method set the TextBackgroundColor of a Graphics instace
            /// </summary>
            /// <param name="graphic">The graphics instace</param>
            /// <param name="color">The System.Color to set the TextBackgroundColor</param>
            public void SetTextBackgroundColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
            {
                if (this[graphic] != null)
                {
                    ((GraphicsProperties)this[graphic]).TextBackgroundColor = color;
                }
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.TextBackgroundColor = color;
                    Add(graphic, tempProps);
                }
            }

            /// <summary>
            /// Structure to store properties from System.Drawing.Graphics objects
            /// </summary>
            class GraphicsProperties
            {
                public System.Drawing.Color TextBackgroundColor = System.Drawing.Color.Black;
                public System.Drawing.Color color = System.Drawing.Color.Black;
                public System.Drawing.Color BackColor = System.Drawing.Color.White;
                public System.Drawing.Color TextColor = System.Drawing.Color.Black;
                public System.Drawing.SolidBrush GraphicBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                public System.Drawing.Brush PaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                public System.Drawing.Pen GraphicPen = new System.Drawing.Pen(System.Drawing.Color.Black);
                public System.Drawing.Font GraphicFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            }
        }
    }
}
