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
        /// Holds the TextRender used for rendering.
        /// </summary>
        public static TextRenderer textrender;

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
        /// Prepares the console.
        /// </summary>
        public static void InitConsole()
        {
            ConsoleText = new PieceOfText(Utilities.CopyText("\n", Lines), new Point(0, (-(Lines + 2) * TextRenderer.DefaultFont.Height) - 5));
            Typing = new PieceOfText("", new Point(0, ((MainGame.ScreenHeight / 2) - TextRenderer.DefaultFont.Height) - 5));
            textrender = new TextRenderer(MainGame.ScreenWidth, MainGame.ScreenHeight / 2);
            textrender.AddText(ConsoleText);
            textrender.AddText(Typing);
            MaxWidth = MainGame.ScreenWidth - 10;
            WriteLine("Console loaded!");
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
                if (TextRenderer.MeasureFancyText(Texture.GenericGraphicsObject, text.Substring(linestart, i - linestart), ConsoleText) > MaxWidth)
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
            textrender.modified = true;
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
                KeyHandler.TogglerPressed = false;
                textrender.modified = true;
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
            if (!Open)
            {
                KeyHandler.Clear();
            }
            else
            {
                // Update the input line
                keymark_delta += MainGame.Delta;
                if (keymark_delta > 0.5f)
                {
                    keymark_add = !keymark_add;
                    textrender.modified = true;
                    keymark_delta = 0f;
                }
                if (KeyHandler.InitBS > 0)
                {
                    if (TypingText.Length > KeyHandler.InitBS)
                    {
                        TypingText = TypingText.Substring(0, TypingText.Length - KeyHandler.InitBS);
                        textrender.modified = true;
                    }
                    else
                    {
                        TypingText = "";
                        textrender.modified = true;
                    }
                    KeyHandler.InitBS = 0;
                }
                if (KeyHandler.KeyboardString.Length > 0)
                {
                    TypingText += KeyHandler.KeyboardString;
                    textrender.modified = true;
                    KeyHandler.KeyboardString = "";
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
                        Write("] " + input + "\n");
                        Commands.ExecuteCommands(input);
                    }
                }
                Typing.Text = ">" + TypingText + (keymark_add ? "|" : "");
                if (KeyHandler.CopyPressed)
                {
                    if (TypingText.Length > 0)
                    {
                        System.Windows.Forms.Clipboard.SetText(TypingText);
                    }
                    KeyHandler.CopyPressed = false;
                }
            }
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
                GL.BindTexture(TextureTarget.Texture2D, Texture.Console.Internal_Texture);
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
                // Bottom line
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.BindTexture(TextureTarget.Texture2D, Texture.White.Internal_Texture);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 0);
                GL.Vertex2(0, (MainGame.ScreenHeight / 2) - 1);
                GL.TexCoord2(1, 0);
                GL.Vertex2(MainGame.ScreenWidth, (MainGame.ScreenHeight / 2) - 1);
                GL.TexCoord2(1, 1);
                GL.Vertex2(MainGame.ScreenWidth, MainGame.ScreenHeight / 2);
                GL.TexCoord2(0, 1);
                GL.Vertex2(0, MainGame.ScreenHeight / 2);
                GL.End();
            }

            // Render the console text
            textrender.Draw();
        }
    }
}
