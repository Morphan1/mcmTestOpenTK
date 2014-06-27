using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using System.Drawing;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.UIHandlers.Menus
{
    public class TextBox: AbstractMenuItem
    {
        Texture StandardTexture;

        PieceOfText Text;

        /// <summary>
        /// Whether to type all characters as '*'.
        /// </summary>
        public bool Password = false;

        int MaxWidth;

        public TextBox(int X, int Y, Texture _standText, int _width)
        {
            StandardTexture = _standText;
            RenderSquare = new Square();
            MaxWidth = _width > 0 ? _width: 1;
            RenderSquare.PositionLow = new Location(X, Y, 0);
            Text = new PieceOfText("", new Location(X + 10, Y, 0), FontSet.GetFont(GLFont.Standard.Name, (int)((float)GLFont.Standard.Size * 1.5f)));
            RenderSquare.PositionHigh = new Location(X + MaxWidth + 20, Y + Text.set.font_default.Height + 8, 0);
        }

        public override void Draw()
        {
            RenderSquare.texture = StandardTexture;
            RenderSquare.Draw();
            Text.Position = new Location((int)(RenderSquare.PositionLow.X + 10), (int)RenderSquare.PositionLow.Y, 0);
            FontSet.DrawColoredText(Text, int.MaxValue, 1, true);
            if (keymark_add > 0.5f)
            {
                float XAdd = FontSet.MeasureFancyText(Text.Text.Substring(0, TypingCursor + 1), Text.set) - 1;
                if (Text.Text.Length > TypingCursor + 1 && Text.Text[TypingCursor] == '^'
                    && TextStyle.IsColorSymbol(Text.Text[TypingCursor + 1]))
                {
                    XAdd -= Text.set.font_default.MeasureString(Text.Text[TypingCursor].ToString());
                }
                PieceOfText SymText = new PieceOfText("|", new Location((int)(Text.Position.X + XAdd), (int)Text.Position.Y, 0), Text.set);
                FontSet.DrawColoredText(SymText, int.MaxValue, 1, Hovered);
            }
        }

        public override void RightClick()
        {
        }

        public bool selected = false;

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
        }

        public string TypingText = "";
        int TypingCursor = 0;
        float keymark_add = 0;

        public override void Tick()
        {
            if (selected)
            {
                KeyHandlerState KeyState = KeyHandler.GetKBState();
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

                string restext;
                if (Password)
                {
                    restext = Utilities.CopyText("*", TypingText.Length);
                }
                else
                {
                    restext = TypingText;
                }
                while (FontSet.MeasureFancyText(">" + restext + "<", Text.set) > MaxWidth)
                {
                    if (TypingCursor == TypingText.Length)
                    {
                        TypingCursor--;
                    }
                    TypingText = TypingText.Substring(0, TypingText.Length - 1);
                    restext = restext.Substring(0, restext.Length - 1);
                }
                // Update rendered text
                if (keymark_add == -1f)
                {
                    keymark_add = 0.5f;
                }
                keymark_add += MainGame.DeltaF;
                Text.Text = ">" + restext;
                if (keymark_add > 1f)
                {
                    keymark_add -= 1f;
                }
            }
            else
            {
                string restext;
                if (Password)
                {
                    restext = Utilities.CopyText("*", TypingText.Length);
                }
                else
                {
                    restext = TypingText;
                }
                keymark_add = -1f;
                Text.Text = ">" + restext;
            }
        }
    }
}
