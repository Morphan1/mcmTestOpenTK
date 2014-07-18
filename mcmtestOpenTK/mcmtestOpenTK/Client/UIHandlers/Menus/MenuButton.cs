using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using System.Drawing;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.UIHandlers.Menus
{
    public abstract class MenuButton: AbstractMenuItem
    {
        Texture StandardTexture;
        Texture HoverTexture;

        PieceOfText Text;

        public MenuButton(string _text, int X, int Y, Texture _hovText, Texture _standText)
        {
            HoverTexture = _hovText;
            StandardTexture = _standText;
            RenderSquare = new Square();
            RenderSquare.PositionLow = new Location(X, Y, 0);
            Text = new PieceOfText(_text, new Location(X + 10, Y + 4, 0), FontSet.GetFont(GLFont.Standard.Name, GLFont.Standard.Size * 2));
            RenderSquare.PositionHigh = new Location(X + FontSet.MeasureFancyText(Text.Text, Text.set) + 20, Y + Text.set.font_default.Height + 10, 0);
        }

        public override void ClickOutside()
        {
        }

        public override void Draw()
        {
            RenderSquare.texture = Hovered ? HoverTexture : StandardTexture;
            RenderSquare.Draw();
            Text.Position = new Location((int)(RenderSquare.PositionLow.X + 10), (int)RenderSquare.PositionLow.Y + 4, 0);
            FontSet.DrawColoredText(Text, int.MaxValue, 1, Hovered);
        }

        public override void RightClick()
        {
        }

        public override void Hover()
        {
        }

        public override void Unhover()
        {
        }

        public override void Tick()
        {
        }

        public override void Recalc()
        {
        }
    }
}
