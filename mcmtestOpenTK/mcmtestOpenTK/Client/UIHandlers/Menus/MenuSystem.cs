using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.UIHandlers.Menus
{
    public class MenuSystem
    {
        /// <summary>
        /// All items in this menu.
        /// </summary>
        public List<AbstractMenuItem> MenuItems;

        public MenuSystem()
        {
        }

        /// <summary>
        /// Prepares the menu system for use.
        /// </summary>
        public void Init()
        {
            MenuItems = new List<AbstractMenuItem>();
        }

        /// <summary>
        /// Ticks the menu system.
        /// </summary>
        public void Tick()
        {
            int mx = MainGame.PrimaryGameWindow.Mouse.X;
            int my = MainGame.PrimaryGameWindow.Mouse.Y;
            bool pressedleft = MouseHandler.CurrentMouse.IsButtonDown(MouseButton.Left)
                && !MouseHandler.PreviousMouse.IsButtonDown(MouseButton.Left);
            bool pressedright = MouseHandler.CurrentMouse.IsButtonDown(MouseButton.Right)
                && !MouseHandler.PreviousMouse.IsButtonDown(MouseButton.Right);
            for (int i = 0; i < MenuItems.Count; i++)
            {
                if (MenuItems[i].RenderSquare.Contains(mx, my))
                {
                    if (!MenuItems[i].Hovered)
                    {
                        MenuItems[i].Hovered = true;
                        MenuItems[i].Hover();
                    }
                    if (pressedleft)
                    {
                        MenuItems[i].LeftClick();
                    }
                    if (pressedright)
                    {
                        MenuItems[i].RightClick();
                    }
                }
                else
                {
                    if (MenuItems[i].Hovered)
                    {
                        MenuItems[i].Hovered = false;
                        MenuItems[i].Unhover();
                    }
                    if (pressedleft)
                    {
                        MenuItems[i].ClickOutside();
                    }
                }
            }
        }

        /// <summary>
        /// Draws the menu system.
        /// </summary>
        public void Draw()
        {
            for (int i = 0; i < MenuItems.Count; i++)
            {
                MenuItems[i].Draw();
            }
        }
    }
}
