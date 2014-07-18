using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using System.Drawing;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.UIHandlers.Menus
{
    public class TextBox: AbstractMenuItem
    {
        /// <summary>
        /// The backdrop box image.
        /// </summary>
        Texture StandardTexture;

        /// <summary>
        /// The PieceOfText used to render the text.
        /// </summary>
        PieceOfText Text;

        /// <summary>
        /// Whether to type all characters as '*'.
        /// </summary>
        public bool Password = false;

        /// <summary>
        /// How wide the text can go.
        /// </summary>
        int MaxWidth;

        public CVar cvar;

        public string text;

        public TextBox(int X, int Y, Texture _standText, int _width, CVar _cvar)
        {
            StandardTexture = _standText;
            RenderSquare = new Square();
            MaxWidth = _width > 0 ? _width: 1;
            RenderSquare.PositionLow = new Location(X, Y, 0);
            Text = new PieceOfText("", new Location(X + 10, Y + 4, 0), FontSet.GetFont(GLFont.Standard.Name, (int)((float)GLFont.Standard.Size * 1.5f)));
            RenderSquare.PositionHigh = new Location(X + MaxWidth + 20, Y + Text.set.font_default.Height + 8, 0);
            cvar = _cvar;
            TypingText = cvar.Value;
            FixCursor();
        }

        /// <summary>
        /// Renders the text box.
        /// </summary>
        public override void Draw()
        {
            RenderSquare.texture = StandardTexture;
            RenderSquare.Draw();
            Text.Position = new Location((int)(RenderSquare.PositionLow.X + 10), (int)RenderSquare.PositionLow.Y + 4, 0);
            FontSet.DrawColoredText(Text, int.MaxValue, 1, true);
            if (keymark_add > 0.5f)
            {
                float XAdd = FontSet.MeasureFancyText(Text.Text.Substring(0, VisiCursor + 1), Text.set) - 1;
                if (Text.Text.Length > VisiCursor + 1 && Text.Text[VisiCursor] == '^'
                    && TextStyle.IsColorSymbol(Text.Text[VisiCursor + 1]))
                {
                    XAdd -= Text.set.font_default.MeasureString(Text.Text[VisiCursor].ToString());
                }
                PieceOfText SymText = new PieceOfText("|", new Location((int)(Text.Position.X + XAdd), (int)Text.Position.Y, 0), Text.set);
                FontSet.DrawColoredText(SymText, int.MaxValue, 1, Hovered);
            }
        }

        public override void RightClick()
        {
        }

        /// <summary>
        /// Jumps the cursor to the end of the text, useful when TypingText is modified.
        /// </summary>
        public void FixCursor()
        {
            TypingCursor = TypingText.Length;
            VisiCursor = TypingCursor;
        }

        /// <summary>
        /// Whether the text box is currently selected.
        /// </summary>
        public bool selected = false;

        /// <summary>
        /// Selects the TextBox, and calculates an updated cursor position.
        /// </summary>
        /// <param name="x">The X coordinate clicked, relative to the textbox's corner</param>
        /// <param name="y">The Y coordinate clicked, relative to the textbox's corner</param>
        public override void LeftClick(int x, int y)
        {
            selected = true;
            x -= (int)(Text.Position.X - RenderSquare.PositionLow.X);
            float plen = 0;
            for (int i = 0; i < TypingText.Length; i++)
            {
                float len = FontSet.MeasureFancyText(">" + TypingText.Substring(0, i), Text.set);
                len += (len - plen) / 2;
                if (x > plen && x < len)
                {
                    TypingCursor = i;
                    return;
                }
                plen = len;
            }
            TypingCursor = TypingText.Length;
            keymark_add = 0.5f;
        }

        /// <summary>
        /// Deselects the TextBox.
        /// </summary>
        public override void ClickOutside()
        {
            keymark_add = 0f;
            selected = false;
        }

        public override void Hover()
        {
        }

        public override void Unhover()
        {
        }

        public virtual void Enter()
        {
            bool cansel = false;
            for (int i = 0; i < Menus.MenuItems.Count; i++)
            {
                if (Menus.MenuItems[i] is TextBox)
                {
                    if (cansel)
                    {
                        ((TextBox)Menus.MenuItems[i]).selected = true;
                        return;
                    }
                    else if (((TextBox)Menus.MenuItems[i]).selected)
                    {
                        ((TextBox)Menus.MenuItems[i]).selected = false;
                        cansel = true;
                    }
                }
                else if (Menus.MenuItems[i] is MenuToggler)
                {
                    if (cansel)
                    {
                        Menus.MenuItems[i].LeftClick(0, 0);
                        return;
                    }
                }
            }
            if (!cansel)
            {
                return;
            }
            for (int i = 0; i < Menus.MenuItems.Count; i++)
            {
                if (Menus.MenuItems[i] is TextBox)
                {
                    ((TextBox)Menus.MenuItems[i]).selected = true;
                    return;
                }
                else if (Menus.MenuItems[i] is MenuToggler)
                {
                    Menus.MenuItems[i].LeftClick(0, 0);
                    return;
                }
            }
        }

        /// <summary>
        /// The text currently entered in the box.
        /// </summary>
        public string TypingText = "";

        /// <summary>
        /// The cursor location.
        /// </summary>
        int TypingCursor = 0;
        int VisiCursor = 0;

        /// <summary>
        /// Whether to add a | symbol where the cursor is.
        /// </summary>
        float keymark_add = 0;

        /// <summary>
        /// Update the TextBox, handle keyboard input.
        /// </summary>
        public override void Tick()
        {
            if (selected)
            {
                // Grab and clear the keyboard state
                KeyHandlerState KeyState = KeyHandler.GetKBState();
                // Handle backspaces presses
                if (KeyState.InitBS > 0)
                {
                    keymark_add = 0.5f;
                    string partone = TypingCursor > 0 ? TypingText.Substring(0, TypingCursor) : "";
                    string parttwo = TypingCursor < TypingText.Length ? TypingText.Substring(TypingCursor) : "";
                    if (partone.Length > KeyState.InitBS)
                    {
                        partone = partone.Substring(0, partone.Length - KeyState.InitBS);
                        TypingCursor -= KeyState.InitBS;
                    }
                    else
                    {
                        TypingCursor -= partone.Length;
                        partone = "";
                    }
                    TypingText = partone + parttwo;
                }
                // Handle delete key presses
                if (KeyState.EndDelete > 0)
                {
                    keymark_add = 0.5f;
                    string partone = TypingCursor > 0 ? TypingText.Substring(0, TypingCursor) : "";
                    string parttwo = TypingCursor < TypingText.Length ? TypingText.Substring(TypingCursor) : "";
                    if (parttwo.Length > KeyState.EndDelete)
                    {
                        parttwo = parttwo.Substring(KeyState.EndDelete);
                    }
                    else
                    {
                        parttwo = "";
                    }
                    TypingText = partone + parttwo;
                }
                // handle input text
                if (KeyState.KeyboardString.Contains('\t'))
                {
                    if (Menus != null)
                    {
                        Menus.RotateTextBoxSelect();
                        KeyState.KeyboardString = "";
                    }
                    else
                    {
                        KeyState.KeyboardString = KeyState.KeyboardString.Replace("\t", "    ");
                    }
                }
                KeyState.KeyboardString = KeyState.KeyboardString.Replace("\r", "");
                if (KeyState.KeyboardString.Contains("\n"))
                {
                    KeyState.KeyboardString = KeyState.KeyboardString.Replace("\n", "");
                    Enter();
                }
                if (KeyState.KeyboardString.Length > 0)
                {
                    keymark_add = 0.5f;
                    if (TypingText.Length == TypingCursor)
                    {
                        TypingText += Utilities.CleanStringInput(KeyState.KeyboardString);
                    }
                    else
                    {
                        TypingText = TypingText.Insert(TypingCursor, Utilities.CleanStringInput(KeyState.KeyboardString));
                    }
                    TypingCursor += KeyState.KeyboardString.Length;
                }
                // handle copying
                if (KeyState.CopyPressed && !Password)
                {
                    if (TypingText.Length > 0)
                    {
                        System.Windows.Forms.Clipboard.SetText(TypingText);
                    }
                }
                // handle cursor left/right movement
                if (KeyState.LeftRights != 0)
                {
                    TypingCursor += KeyState.LeftRights;
                    if (TypingCursor < 0)
                    {
                        TypingCursor = 0;
                    }
                    if (TypingCursor > TypingText.Length)
                    {
                        TypingCursor = TypingText.Length;
                    }
                    keymark_add = 0.5f;
                }
            }
            string restext;
            if (Password)
            {
                restext = Utilities.CopyText("*", TypingText.Length);
            }
            else
            {
                restext = TypingText;
            }
            VisiCursor = TypingCursor;
            string ftext = "";
            int downs = 1;
            int ups = 0;
            bool flip = true;
            VisiCursor = 0;
            while (FontSet.MeasureFancyText(">" + ftext + "<", Text.set) < MaxWidth)
            {
                if (flip && TypingCursor - downs >= 0)
                {
                    ftext = restext[TypingCursor - downs] + ftext;
                    VisiCursor++;
                    downs++;
                }
                else if (TypingCursor + ups < restext.Length)
                {
                    ftext = ftext + restext[TypingCursor + ups];
                    ups++;
                }
                else if (TypingCursor - downs < 0)
                {
                    break;
                }
                flip = !flip;
            }
            restext = ftext;
            Text.Text = ">" + restext;
            if (selected)
            {
                // Update rendered text
                if (keymark_add == -1f)
                {
                    keymark_add = 0.5f;
                }
                keymark_add += MainGame.DeltaF;
                if (keymark_add > 1f)
                {
                    keymark_add -= 1f;
                }
                cvar.Set(TypingText);
            }
        }
        public override void Recalc()
        {
            TypingText = cvar.Value;
            FixCursor();
        }
    }
}
