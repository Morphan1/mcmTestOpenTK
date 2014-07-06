using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using mcmtestOpenTK.Client.CommonHandlers;
using System.Diagnostics;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using System.Threading;
using mcmtestOpenTK.Client.GraphicsHandlers;
using System.Drawing.Imaging;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        /// <summary>
        /// Global entry point, should never be directly called!
        /// </summary>
        /// <param name="args">Command line input args</param>
        public static void Client_Main(List<string> args)
        {
            // Fix the system console.
            SysConsole.FixTitle();
            // Create the data saving thread
            SysConsole.Output(OutputType.INIT, "Starting up save-irrelevant-data thread...");
            Thread datathr = new Thread(new ThreadStart(SaveIrrelevantData));
            datathr.Name = "SaveIrrelevantData";
            Program.ThreadsToClose.Add(datathr);
            datathr.Start();
            // Create the window and establish basic event info / settings
            PrimaryGameWindow = new GameWindow(ScreenWidth, ScreenHeight);
            PrimaryGameWindow.Title = WindowTitle;
            PrimaryGameWindow.Closed += new EventHandler<EventArgs>(PrimaryGameWindow_Closed);
            PrimaryGameWindow.Load += new EventHandler<EventArgs>(PrimaryGameWindow_Load);
            PrimaryGameWindow.UpdateFrame += new EventHandler<FrameEventArgs>(PrimaryGameWindow_UpdateFrame);
            PrimaryGameWindow.RenderFrame += new EventHandler<FrameEventArgs>(PrimaryGameWindow_RenderFrame);
            PrimaryGameWindow.KeyPress += new EventHandler<KeyPressEventArgs>(KeyHandler.PrimaryGameWindow_KeyPress);
            PrimaryGameWindow.KeyDown += new EventHandler<KeyboardKeyEventArgs>(KeyHandler.PrimaryGameWindow_KeyDown);
            PrimaryGameWindow.KeyUp += new EventHandler<KeyboardKeyEventArgs>(KeyHandler.PrimaryGameWindow_KeyUp);
            // Begin running the game.
            SysConsole.Output(OutputType.INIT, "Starting up main game window...");
            PrimaryGameWindow.Run(Target_cFPS, Target_gFPS);
            SysConsole.Output(OutputType.CLIENTINFO, "Game done running!");
        }

        static void SaveIrrelevantData()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    Bitmap shot = null;
                    lock (ScreenshotLock)
                    {
                        if (Screenshots.Count > 0)
                        {
                            shot = Screenshots.Dequeue();
                        }
                    }
                    if (shot != null)
                    {
                        List<string> files = FileHandler.AllFiles("screenshots");
                        int shotnum = 0;
                        string name = "screenshot" + Utilities.Pad(shotnum.ToString(), '0', 4);
                        while (FileHandler.Exists("screenshots/" + name + ".png"))
                        {
                            shotnum++;
                            name = "screenshot" + Utilities.Pad(shotnum.ToString(), '0', 4);
                        }
                        DataStream ds = new DataStream();
                        shot.Save(ds, ImageFormat.Png);
                        FileHandler.WriteBytes("screenshots/" + name + ".png", ds.ToArray());
                        shot.Dispose();
                    }
                    if (ConfigStr.Length > 0)
                    {
                        FileHandler.WriteText("clientconfig.cfg", ConfigStr);
                        ConfigStr = "";
                    }
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    SysConsole.Output(OutputType.ERROR, "Error handling irrelevant save data: " + ex.ToString());
                }
            }
        }
    }
}
