using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using System.Drawing;
using mcmtestOpenTK.Client.GraphicsHandlers;

namespace mcmtestOpenTK.Client.UIHandlers.Menus
{
    class MenuLabel: AbstractMenuItem
    {
        PieceOfText Text;

        public MenuLabel(string _text, int X, int Y)
        {
            RenderSquare = new Square();
            RenderSquare.PositionLow = new Location(X, Y, 0);
            Text = new PieceOfText(_text, new Location(X, Y, 0), FontSet.GetFont(GLFont.Standard.Name, GLFont.Standard.Size * 2));
            Location Measured = FontSet.MeasureFancyLinesOfText(Text.Text, Text.set);
            RenderSquare.PositionHigh = new Location(X + Measured.X, Y + Measured.Y, 0);
        }

        public override void ClickOutside()
        {
        }

        public override void Draw()
        {
            Text.Position = new Location((int)RenderSquare.PositionLow.X, (int)RenderSquare.PositionLow.Y, 0);
            FontSet.DrawColoredText(Text, int.MaxValue, 1, Hovered);
        }

        public override void Hover()
        {
        }

        public override void Unhover()
        {
        }

        public override void LeftClick(int X, int Y)
        {
        }

        public override void RightClick()
        {
        }

        public override void Tick()
        {
        }
    }
}
