using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;

namespace mcmtestOpenTK.Client.UIHandlers.Menus
{
    public class MenuSystem
    {
        /// <summary>
        /// All items in this menu.
        /// </summary>
        public List<AbstractMenuItem> MenuItems;

        /// <summary>
        /// Adds an item to the menu system.
        /// </summary>
        /// <param name="item">The item to add</param>
        public void Add(AbstractMenuItem item)
        {
            item.Menus = this;
            MenuItems.Add(item);
        }

        public MenuSystem()
        {
        }

        /// <summary>
        /// The currently shown notice.
        /// </summary>
        public string Notice = null;

        MenuButton NoticeOK;

        MenuLabel NoticeLabel;

        Square NoticeRenderSquare;

        FontSet Set;

        /// <summary>
        /// Shows an "OK" box message.
        /// </summary>
        /// <param name="message">The message</param>
        public void ShowNotice(string message)
        {
            message += "\n";
            int index = 0;
            Notice = "";
            for (int i = 0; i < message.Length; i++)
            {
                if (FontSet.MeasureFancyText(message.Substring(index, i - index), Set) > MainGame.ScreenWidth - 100)
                {
                    int target = i;
                    for (; i > index; i--)
                    {
                        if (message[i] == ' ')
                        {
                            target = i;
                            break;
                        }
                    }
                    i = target;
                    message = message.Substring(0, target) + "\n" + message.Substring(target + 1, message.Length - (target + 1));
                }
                if (message[i] == '\n')
                {
                    Notice += message.Substring(index, i - index);
                    index = i;
                }
            }
            while (Notice.EndsWith("\n") || Notice.EndsWith("\r") || Notice.EndsWith(" "))
            {
                Notice = Notice.Substring(0, Notice.Length - 1);
            }
            Location size = FontSet.MeasureFancyLinesOfText(Notice, Set) + new Location(20, Set.font_default.Height * 3, 0);
            NoticeRenderSquare = new Square();
            NoticeRenderSquare.PositionLow = new Location(MainGame.ScreenWidth / 2 - size.X / 2, MainGame.ScreenHeight / 2 - size.Y / 2, 0);
            NoticeRenderSquare.PositionHigh = new Location(MainGame.ScreenWidth / 2 + size.X / 2, MainGame.ScreenHeight / 2 + size.Y / 2, 0);
            NoticeRenderSquare.texture = Texture.GetTexture("menus/notice");
            NoticeOK = new NoticeOKButton(MainGame.ScreenWidth / 2 - 20, (int)(MainGame.ScreenHeight / 2 + size.Y / 2 - Set.font_default.Height * 2));
            NoticeLabel = new MenuLabel(message, (int)(MainGame.ScreenWidth / 2 - size.X / 2 + 10), (int)(MainGame.ScreenHeight / 2 - size.Y / 2));
            NoticeOK.Menus = this;
            NoticeLabel.Menus = this;
            for (int i = 0; i < MenuItems.Count; i++)
            {
                if (MenuItems[i].Hovered)
                {
                    MenuItems[i].Hovered = false;
                    MenuItems[i].Unhover();
                }
            }
        }

        /// <summary>
        /// Prepares the menu system for use.
        /// </summary>
        public void Init()
        {
            MenuItems = new List<AbstractMenuItem>();
            Set = FontSet.GetFont(GLFont.Standard.Name, GLFont.Standard.Size * 2);
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
            if (Notice != null)
            {
                TickItem(NoticeLabel, mx, my, pressedleft, pressedright);
                TickItem(NoticeOK, mx, my, pressedleft, pressedright);
                KeyHandler.GetKBState();
                return;
            }
            for (int i = 0; i < MenuItems.Count; i++)
            {
                TickItem(MenuItems[i], mx, my, pressedleft, pressedright);
            }
            KeyHandler.GetKBState();
        }

        public void RotateTextBoxSelect()
        {
            bool cansel = false;
            for (int i = 0; i < MenuItems.Count; i++)
            {
                if (MenuItems[i] is TextBox)
                {
                    if (cansel)
                    {
                        ((TextBox)MenuItems[i]).selected = true;
                        return;
                    }
                    else if (((TextBox)MenuItems[i]).selected)
                    {
                        ((TextBox)MenuItems[i]).selected = false;
                        cansel = true;
                    }
                }
            }
            if (!cansel)
            {
                return;
            }
            for (int i = 0; i < MenuItems.Count; i++)
            {
                if (MenuItems[i] is TextBox)
                {
                    ((TextBox)MenuItems[i]).selected = true;
                    return;
                }
            }
        }

        void TickItem(AbstractMenuItem item, int mx, int my, bool pressedleft, bool pressedright)
        {
            item.Tick();
            if (item.RenderSquare.Contains(mx, my))
            {
                if (!item.Hovered)
                {
                    item.Hovered = true;
                    item.Hover();
                }
                if (pressedleft)
                {
                    item.LeftClick((int)(mx - item.RenderSquare.PositionLow.X),
                        (int)(my - item.RenderSquare.PositionLow.Y));
                }
                if (pressedright)
                {
                    item.RightClick();
                }
            }
            else
            {
                if (item.Hovered)
                {
                    item.Hovered = false;
                    item.Unhover();
                }
                if (pressedleft)
                {
                    item.ClickOutside();
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
            if (Notice != null)
            {
                NoticeRenderSquare.Draw();
                NoticeLabel.Draw();
                NoticeOK.Draw();
            }
        }
    }
}
