using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;

namespace mcmtestOpenTK.Client.UIHandlers.Menus
{
    public abstract class AbstractMenuItem
    {
        /// <summary>
        /// Where the menu item is drawn at, and clicks are processed at.
        /// </summary>
        public Square RenderSquare;

        /// <summary>
        /// Whether the mouse is hovered over this item.
        /// </summary>
        public bool Hovered;

        /// <summary>
        /// Called when the menu item is hovered over.
        /// </summary>
        public abstract void Hover();

        /// <summary>
        /// Called when the menu item is no longer hovered over.
        /// </summary>
        public abstract void Unhover();

        /// <summary>
        /// Called when the menu item is right-clicked.
        /// </summary>
        public abstract void RightClick();

        /// <summary>
        /// Called when the menu item is left-clicked.
        /// </summary>
        public abstract void LeftClick();

        /// <summary>
        /// Called when there is a left click outside the menu item.
        /// </summary>
        public abstract void ClickOutside();

        /// <summary>
        /// Draw the menu item.
        /// </summary>
        public abstract void Draw();
    }
}
