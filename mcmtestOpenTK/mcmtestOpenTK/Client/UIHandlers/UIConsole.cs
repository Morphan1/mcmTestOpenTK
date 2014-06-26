using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommandHandlers;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.UIHandlers
{
    public class UIConsole
    {
        /// <summary>
        /// Holds the Graphics text object, for rendering.
        /// </summary>
        public static PieceOfText ConsoleText;

        /// <summary>
        /// Holds the text currently being typed.
        /// </summary>
        public static PieceOfText Typing;

        /// <summary>
        /// Holds the "scrolled-up" text.
        /// </summary>
        public static PieceOfText ScrollText;

        /// <summary>
        /// How many lines the console should have.
        /// </summary>
        public static int Lines = 100;

        /// <summary>
        /// How long across (in pixels) console text may be.
        /// </summary>
        public static int MaxWidth = MainGame.ScreenWidth - 50;

        /// <summary>
        /// Any added text, for logging purposes.
        /// </summary>
        public static string NewText = "";

        /// <summary>
        /// Reference for locking the NewText variable.
        /// </summary>
        public static Object NewTextLock = new Object();

        /// <summary>
        /// Whether the console is open.
        /// </summary>
        public static bool Open = false;

        /// <summary>
        /// The text currently being typed by the user.
        /// </summary>
        public static string TypingText = "";

        /// <summary>
        /// Where in the typing text the cursor is at.
        /// </summary>
        public static int TypingCursor = 0;

        /// <summary>
        /// What line has been scrolled to:
        /// 0 = farthest down, -LINES = highest up.
        /// The selected line will be rendered at the bottom of the screen.
        /// </summary>
        public static int ScrolledLine = 0;

        /// <summary>
        /// How many recent commands to store.
        /// </summary>
        public static int MaxRecentCommands = 50;

        /// <summary>
        /// A list of all recently execute command-lines.
        /// </summary>
        public static List<string> RecentCommands = new List<string>() { "" };

        /// <summary>
        /// What spot in the RecentCommands the user is currently at.
        /// </summary>
        public static int RecentSpot = 0;

        static bool ready = false;

        static string pre_waiting = "";

        static int extralines = 0;
        static double LineBack = 0;

        /// <summary>
        /// Prepares the console.
        /// </summary>
        public static void InitConsole()
        {
            ready = true;
            ConsoleText = new PieceOfText(Utilities.CopyText("\n", Lines), new Point(5, 0));
            Typing = new PieceOfText("", new Point(5, 0));
            ScrollText = new PieceOfText("^1" + Utilities.CopyText("/\\ ", 150), new Point(5, 0));
            MaxWidth = MainGame.ScreenWidth - 10;
            WriteLine("Console loaded!");
            Write(pre_waiting);
        }

        /// <summary>
        /// Writes a line of text to the console.
        /// </summary>
        /// <param name="text">The text to be written</param>
        public static void WriteLine(string text)
        {
            Write(TextStyle.Default + text + "\n");
        }

        /// <summary>
        /// Writes text to the console.
        /// </summary>
        /// <param name="text">The text to be written</param>
        public static void Write(string text)
        {
            if (!ready)
            {
                pre_waiting += text;
                return;
            }
            if (!ConsoleText.Text.EndsWith("\n"))
            {
                for (int x = ConsoleText.Text.Length - 1; x > 0; x--)
                {
                    if (ConsoleText.Text[x] == '\n')
                    {
                        string snippet = ConsoleText.Text.Substring(x + 1, ConsoleText.Text.Length - (x + 1));
                        text = snippet + text;
                        ConsoleText.Text = ConsoleText.Text.Substring(0, x + 1);
                        break;
                    }
                }
            }
            text = text.Replace('\r', ' ');
            lock (NewTextLock)
            {
                NewText += text;
            }
            int linestart = 0;
            int i = 0;
            for (i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    linestart = i + 1;
                    i++;
                    continue;
                }
                if (FontSet.MeasureFancyText(text.Substring(linestart, i - linestart), ConsoleText.set) > MaxWidth)
                {
                    i -= 1;
                    for (int x = i; x > 0 && x > linestart + 5; x--)
                    {
                        if (text[x] == ' ')
                        {
                            i = x + 1;
                            break;
                        }
                    }
                    text = text.Substring(0, i) + "\n" + (i < text.Length ? text.Substring(i, text.Length - i): "");
                    linestart = i + 1;
                    i++;
                }
            }
            int lines = Utilities.CountCharacter(text, '\n');
            if (lines > 0)
            {
                int linecount = 0;
                for (i = 0; i < ConsoleText.Text.Length; i++)
                {
                    if (ConsoleText.Text[i] == '\n')
                    {
                        linecount++;
                        if (linecount >= lines)
                        {
                            break;
                        }
                    }
                }
                ConsoleText.Text = ConsoleText.Text.Substring(i + 1, ConsoleText.Text.Length - (i + 1));
            }
            extralines += lines;
            if (extralines > 3)
            {
                extralines = 3;
            }
            LineBack = 3f;
            ConsoleText.Text += text;
        }

        static bool keymark_add = false;
        static double keymark_delta = 0f;
        static bool mouse_was_captured = false;

        /// <summary>
        /// Updates the console, called every tick.
        /// </summary>
        public static void Tick()
        {
            KeyHandlerState KeyState = KeyHandler.GetKBState();
            // Update open/close state
            if (KeyState.TogglerPressed)
            {
                Open = !Open;
                if (Open)
                {
                    mouse_was_captured = MouseHandler.MouseCaptured;
                    MouseHandler.ReleaseMouse();
                    RecentSpot = RecentCommands.Count;
                }
                else
                {
                    Typing.Text = "";
                    TypingText = "";
                    TypingCursor = 0;
                    if (mouse_was_captured)
                    {
                        MouseHandler.CaptureMouse();
                    }
                }
            }
            if (Open)
            {
                extralines = 0;
                LineBack = 0;
                // flicker the cursor
                keymark_delta += MainGame.Delta;
                if (keymark_delta > 0.5f)
                {
                    keymark_add = !keymark_add;
                    keymark_delta = 0f;
                }
                // handle backspaces
                if (KeyState.InitBS > 0)
                {
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
                // handle input text
                KeyState.KeyboardString = KeyState.KeyboardString.Replace("\t", "    ");
                if (KeyState.KeyboardString.Length > 0)
                {
                    if (TypingText.Length == TypingCursor)
                    {
                        TypingText += Utilities.CleanStringInput(KeyState.KeyboardString);
                    }
                    else
                    {
                        if (KeyState.KeyboardString.Contains('\n'))
                        {
                            string[] lines = KeyState.KeyboardString.Split(new char[] { '\n' }, 2);
                            TypingText = TypingText.Insert(TypingCursor, Utilities.CleanStringInput(lines[0])) + "\n" + Utilities.CleanStringInput(lines[1]);
                        }
                        else
                        {
                            TypingText = TypingText.Insert(TypingCursor, Utilities.CleanStringInput(KeyState.KeyboardString));
                        }
                    }
                    TypingCursor += KeyState.KeyboardString.Length;
                    while (TypingText.Contains('\n'))
                    {
                        int index = TypingText.IndexOf('\n');
                        string input = TypingText.Substring(0, index);
                        if (index + 1 < TypingText.Length)
                        {
                            TypingText = TypingText.Substring(index + 1);
                            TypingCursor = TypingText.Length;
                        }
                        else
                        {
                            TypingText = "";
                            TypingCursor = 0;
                        }
                        WriteLine("] " + input);
                        RecentCommands.Add(input);
                        if (RecentCommands.Count > MaxRecentCommands)
                        {
                            RecentCommands.RemoveAt(0);
                        }
                        RecentSpot = RecentCommands.Count;
                        ClientCommands.ExecuteCommands(input);
                    }
                }
                // handle copying
                if (KeyState.CopyPressed)
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
                    keymark_add = true;
                    keymark_delta = 0f;
                }
                // handle scrolling up/down in the console
                if (KeyState.Pages != 0)
                {
                    ScrolledLine -= (int)(KeyState.Pages * ((float)MainGame.ScreenHeight / 2 / ConsoleText.set.font.Height - 3));
                }
                ScrolledLine -= MouseHandler.MouseScroll;
                if (ScrolledLine > 0)
                {
                    ScrolledLine = 0;
                }
                if (ScrolledLine < -Lines + 5)
                {
                    ScrolledLine = -Lines + 5;
                }
                // handle scrolling through commands
                if (KeyState.Scrolls != 0)
                {
                    RecentSpot -= KeyState.Scrolls;
                    if (RecentSpot < 0)
                    {
                        RecentSpot = 0;
                        TypingText = RecentCommands[0];
                    }
                    else if (RecentSpot >= RecentCommands.Count)
                    {
                        RecentSpot = RecentCommands.Count;
                        TypingText = "";
                    }
                    else
                    {
                        TypingText = RecentCommands[RecentSpot];
                    }
                    TypingCursor = TypingText.Length;
                }
                // update the rendered text
                Typing.Text = ">" + TypingText;
            }
            else // !Open
            {
                if (extralines > 0)
                {
                    LineBack -= MainGame.Delta;
                    if (LineBack <= 0)
                    {
                        extralines--;
                        LineBack = 3f;
                    }
                }
            }
        }

        /// <summary>
        /// Renders the console, called every tick.
        /// </summary>
        public static void Draw()
        {
            // Render the console texture
            Typing.Position.Y = ((MainGame.ScreenHeight / 2) - ConsoleText.set.font.Internal_Font.Height) - 5;
            ConsoleText.Position.Y = (-(Lines + 2) * ConsoleText.set.font.Internal_Font.Height) - 5 - ScrolledLine * (int)ConsoleText.set.font.Height;
            ScrollText.Position.Y = ((MainGame.ScreenHeight / 2) - ConsoleText.set.font.Internal_Font.Height * 2) - 5;
            if (Open)
            {
                ConsoleText.Position.Y += MainGame.ScreenHeight / 2;
                // Standard console box
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                Texture.Console.Bind();
                Shader.Generic.Bind();
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 0);
                GL.Vertex2(0, 0);
                GL.TexCoord2(1, 0);
                GL.Vertex2(MainGame.ScreenWidth, 0);
                GL.TexCoord2(1, 1);
                GL.Vertex2(MainGame.ScreenWidth, MainGame.ScreenHeight / 2);
                GL.TexCoord2(0, 1);
                GL.Vertex2(0, MainGame.ScreenHeight / 2);
                GL.End();

                // Scrollbar
                Texture.White.Bind();
                Shader.ColorMultShader.Bind();
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(Color.White);
                GL.TexCoord2(0, 0);
                GL.Vertex2(0, 0);
                GL.TexCoord2(1, 0);
                GL.Vertex2(2, 0);
                GL.TexCoord2(1, 1);
                GL.Vertex2(2, MainGame.ScreenHeight / 2);
                GL.TexCoord2(0, 1);
                GL.Vertex2(0, MainGame.ScreenHeight / 2);
                GL.Color4(Color.Red);
                float Y = MainGame.ScreenHeight / 2;
                float percentone = -(float)ScrolledLine / (float)Lines;
                float percenttwo = -((float)ScrolledLine - (float)MainGame.ScreenHeight / ConsoleText.set.font.Height) / (float)Lines;
                GL.TexCoord2(0, 0);
                GL.Vertex2(0, Y - Y * percenttwo);
                GL.TexCoord2(1, 0);
                GL.Vertex2(2, Y - Y * percenttwo);
                GL.TexCoord2(1, 1);
                GL.Vertex2(2, Y - Y * percentone);
                GL.TexCoord2(0, 1);
                GL.Vertex2(0, Y - Y * percentone);
                GL.End();

                // Bottom line
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                Texture.White.Bind();
                Shader.ColorMultShader.Bind();
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(Color.Cyan);
                GL.TexCoord2(0, 0);
                GL.Vertex2(0, (MainGame.ScreenHeight / 2) - 1);
                GL.TexCoord2(1, 0);
                GL.Vertex2(MainGame.ScreenWidth, (MainGame.ScreenHeight / 2) - 1);
                GL.TexCoord2(1, 1);
                GL.Vertex2(MainGame.ScreenWidth, MainGame.ScreenHeight / 2);
                GL.TexCoord2(0, 1);
                GL.Vertex2(0, MainGame.ScreenHeight / 2);
                GL.End();

                // Typing text
                //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                FontSet.DrawColoredText(Typing);
                // Cursor
                if (keymark_add)
                {
                    float XAdd = FontSet.MeasureFancyText(Typing.Text.Substring(0, TypingCursor + 1), Typing.set) - 1;
                    if (Typing.Text.Length > TypingCursor + 1 && Typing.Text[TypingCursor] == '^'
                        && TextStyle.IsColorSymbol(Typing.Text[TypingCursor + 1]))
                    {
                        XAdd -= Typing.set.font.MeasureString(Typing.Text[TypingCursor].ToString());
                    }
                    Typing.set.font.DrawStringFull("|", Typing.Position.X + XAdd, Typing.Position.Y, Color.White);
                }
                // Render the console text
                FontSet.DrawColoredText(ConsoleText, (int)(MainGame.ScreenHeight / 2 - ConsoleText.set.font.Height * 3));
                if (ScrolledLine != 0)
                {
                    FontSet.DrawColoredText(ScrollText);
                }
            }
            else
            {
                ConsoleText.Position.Y += (int)(ConsoleText.set.font.Height * (2 + extralines));
                FontSet.DrawColoredText(ConsoleText, (int)(MainGame.ScreenHeight / 2 - ConsoleText.set.font.Height * 3), 1, true);
                ConsoleText.Position.Y -= (int)(ConsoleText.set.font.Height * (2 + extralines));
            }
        }
    }
}
