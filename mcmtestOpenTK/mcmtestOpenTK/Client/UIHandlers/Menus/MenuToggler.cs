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
    class MenuToggler: AbstractMenuItem
    {
        PieceOfText Text;

        Square Label;

        Texture Label_Checked;
        Texture Label_Unchecked;

        /// <summary>
        /// Whether the checkbox is ticked or not.
        /// </summary>
        public bool toggled = false;

        public MenuToggler(string _text, int X, int Y)
        {
            RenderSquare = new Square();
            RenderSquare.PositionLow = new Location(X, Y, 0);
            Text = new PieceOfText(_text, new Location(X + 20, Y, 0), FontSet.GetFont(GLFont.Standard.Name, GLFont.Standard.Size * 2));
            Location Measured = FontSet.MeasureFancyLinesOfText(Text.Text, Text.set);
            RenderSquare.PositionHigh = new Location(X + Measured.X + 25, Y + Measured.Y, 0);
            Label = new Square();
            Label.PositionLow = new Location(X, Y + 5, 0);
            Label.PositionHigh = new Location(X + 32, Y + 37, 0);
            Label_Checked = Texture.GetTexture("menus/toggler_on");
            Label_Unchecked = Texture.GetTexture("menus/toggler_off");
        }

        public override void ClickOutside()
        {
        }

        public override void Draw()
        {
            Label.PositionLow = new Location(RenderSquare.PositionLow.X, RenderSquare.PositionLow.Y + 5, 0);
            Label.PositionHigh = new Location(RenderSquare.PositionLow.X + 32, RenderSquare.PositionLow.Y + 37, 0);
            Label.texture = toggled ? Label_Checked : Label_Unchecked;
            Label.Draw();
            Text.Position = new Location((int)RenderSquare.PositionLow.X + 25, (int)RenderSquare.PositionLow.Y, 0);
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
            toggled = !toggled;
        }

        public override void RightClick()
        {
        }

        public override void Tick()
        {
        }
    }
}
