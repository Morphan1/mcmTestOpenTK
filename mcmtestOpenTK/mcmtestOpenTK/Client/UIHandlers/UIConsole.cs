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
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

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
        public static int MaxWidth = 600;

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
        /// What line has been scrolled to:
        /// 0 = farthest down, -LINES = highest up.
        /// The selected line will be rendered at the bottom of the screen.
        /// </summary>
        public static int ScrolledLine = 0;

        static bool ready = false;

        static string pre_waiting = "";

        /// <summary>
        /// Prepares the console.
        /// </summary>
        public static void InitConsole()
        {
            ready = true;
            ConsoleText = new PieceOfText(Utilities.CopyText("\n", Lines), new Point(5, (-(Lines + 2) * GLFont.Standard.Internal_Font.Height) - 5));
            Typing = new PieceOfText("", new Point(5, ((MainGame.ScreenHeight / 2) - GLFont.Standard.Internal_Font.Height) - 5));
            ScrollText = new PieceOfText("^1" + Utilities.CopyText("/\\ ", 100), new Point(5, ((MainGame.ScreenHeight / 2) - GLFont.Standard.Internal_Font.Height * 2) - 5));
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
                if (GLFont.MeasureFancyText(text.Substring(linestart, i - linestart), ConsoleText) > MaxWidth)
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
            // Update open/close state
            if (KeyHandler.TogglerPressed)
            {
                Open = !Open;
                if (Open)
                {
                    ConsoleText.Position.Y += MainGame.ScreenHeight / 2;
                    mouse_was_captured = MouseHandler.MouseCaptured;
                    MouseHandler.ReleaseMouse();
                }
                else
                {
                    ConsoleText.Position.Y -= MainGame.ScreenHeight / 2;
                    Typing.Text = "";
                    if (mouse_was_captured)
                    {
                        MouseHandler.CaptureMouse();
                    }
                }
            }
            if (Open)
            {
                // flicker the cursor
                keymark_delta += MainGame.Delta;
                if (keymark_delta > 0.5f)
                {
                    keymark_add = !keymark_add;
                    keymark_delta = 0f;
                }
                // handle backspaces
                if (KeyHandler.InitBS > 0)
                {
                    if (TypingText.Length > KeyHandler.InitBS)
                    {
                        TypingText = TypingText.Substring(0, TypingText.Length - KeyHandler.InitBS);
                    }
                    else
                    {
                        TypingText = "";
                    }
                }
                // handle input text
                if (KeyHandler.KeyboardString.Length > 0)
                {
                    TypingText += KeyHandler.KeyboardString;
                    while (TypingText.Contains('\n'))
                    {
                        int index = TypingText.IndexOf('\n');
                        string input = TypingText.Substring(0, index);
                        if (index + 1 < TypingText.Length)
                        {
                            TypingText = TypingText.Substring(index + 1);
                        }
                        else
                        {
                            TypingText = "";
                        }
                        WriteLine("] " + input);
                        Commands.ExecuteCommands(input);
                    }
                }
                // Update the rendered text
                Typing.Text = ">" + TypingText + (keymark_add ? "|" : "");
                // Handle copying
                if (KeyHandler.CopyPressed)
                {
                    if (TypingText.Length > 0)
                    {
                        System.Windows.Forms.Clipboard.SetText(TypingText);
                    }
                }
                // handle scrolling
                if (KeyHandler.Pages < 0)
                {
                    ScrolledLine -= (int)(KeyHandler.Pages * ((float)(MainGame.ScreenHeight / 2) / GLFont.Standard.Height)) + 3;
                }
                if (KeyHandler.Pages > 0)
                {
                    ScrolledLine -= (int)(KeyHandler.Pages * ((float)(MainGame.ScreenHeight / 2) / GLFont.Standard.Height)) - 3;
                }
                if (KeyHandler.Scrolls != 0)
                {
                    ScrolledLine -= KeyHandler.Scrolls;
                }
                if (ScrolledLine > 0)
                {
                    ScrolledLine = 0;
                }
                if (ScrolledLine < -Lines + 5)
                {
                    ScrolledLine = -Lines + 5;
                }
            }
            KeyHandler.Clear();
        }

        /// <summary>
        /// Renders the console, called every tick.
        /// </summary>
        public static void Draw()
        {
            // Render the console texture
            if (Open)
            {
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
                float percenttwo = -((float)ScrolledLine - (float)MainGame.ScreenHeight / GLFont.Standard.Height) / (float)Lines;
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

                //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GLFont.DrawColoredText(Typing);
            }

            // Render the console text
            ConsoleText.Position.Y -= ScrolledLine * (int)GLFont.Standard.Height;
            GLFont.DrawColoredText(ConsoleText, (int)(MainGame.ScreenHeight / 2 - GLFont.Standard.Height * 3));
            ConsoleText.Position.Y += ScrolledLine * (int)GLFont.Standard.Height;
            if (ScrolledLine != 0)
            {
                GLFont.DrawColoredText(ScrollText);
            }
        }
    }
}
