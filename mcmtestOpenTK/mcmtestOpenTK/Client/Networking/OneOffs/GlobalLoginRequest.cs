using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Shared;
using System.Security.Cryptography;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.Networking.OneOffs
{
    public class GlobalLoginRequest : GlobalSecureRequest
    {
        /// <summary>
        /// Sends a message to the Global Server, to begin logging in.
        /// </summary>
        /// <param name="Announce">Whether to announce the result in the console</param>
        /// <param name="username">The username to login with</param>
        /// <param name="password">The password to login with</param>
        /// <returns>The NetPing object created for this login request</returns>
        public static GlobalLoginRequest RequestLogin(bool Announce, string username, string password)
        {
            GlobalLoginRequest glr = new GlobalLoginRequest();
            glr.Username = username;
            glr.Password = password;
            glr.ShouldAnnounce = Announce;
            NetworkingObjects.Add(glr);
            glr.Send();
            return glr;
        }

        /// <summary>
        /// Whether the success of the login should be announced.
        /// </summary>
        public bool ShouldAnnounce = false;

        /// <summary>
        /// The username to login with.
        /// </summary>
        public string Username;

        /// <summary>
        /// The password to login with.
        /// </summary>
        public string Password;

        public override void Send()
        {
            socket = NetworkUtil.CreateSocket();
            Thread thr = new Thread(new ThreadStart(ConnectMe));
            thr.Start();
        }
        void ConnectMe()
        {
            string result = SecureConnectBase("LOGIN\n" + Username + "\n" + Password);
            lock (Locker)
            {
                if (Error != null)
                {
                    return;
                }
                Error = result;
            }
        }

        public override void TickMe()
        {
            lock (Locker)
            {
                if (Error != null)
                {
                    TimeRan = NetPing.MaxRunTime;
                    KillQuietly = true;
                    if (Error.StartsWith("REFUSED:"))
                    {
                        string[] errorsplit = Error.Split(new char[] { ':' }, 2);
                        string[] subdata = errorsplit[1].Split(new char[] { '/' }, 2);
                        string mes = LanguageHandler.GetMessage("login.refused." + subdata[0],
                            TextStyle.Color_Error, new List<Variable> { new Variable("error_data", subdata.Length == 2 ? subdata[1] : "") });
                        UIConsole.WriteLine(TextStyle.Color_Error + "Login was refused with message: " + mes);
                        Fail(mes);
                        MainGame.Username = Username;
                        MainGame.Password = "";
                        MainGame.Session = "";
                    }
                    else if (Error.StartsWith("ACCEPT:"))
                    {
                        string[] sessplit = Error.Split(new char[] { ':' }, 2);
                        string[] subdata = sessplit[1].Split(new char[] { '/' }, 2);
                        if (ShouldAnnounce)
                        {
                            UIConsole.WriteLine(TextStyle.Color_Importantinfo + "Login was accepted with message: " +
                                LanguageHandler.GetMessage("login.accepted.success", TextStyle.Color_Importantinfo,
                                new List<Variable> { new Variable("username", Username) }));
                            Pass();
                        }
                        MainGame.Username = subdata[0];
                        MainGame.Password = Password;
                        MainGame.Session = subdata[1];
                        NetworkBase.Identify();
                    }
                    else
                    {
                        UIConsole.WriteLine(TextStyle.Color_Error + "Login failed with message: " + TextStyle.Color_Separate + Error);
                        Fail(Error);
                        MainGame.Username = Username;
                        MainGame.Password = "";
                        MainGame.Session = "";
                    }
                    ready = true;
                    return;
                }
            }
        }

        void Fail(string message)
        {
            if (MainGame.Screen.Mode == ScreenMode.Login)
            {
                ((Screen_Login)MainGame.Screen).Menus.ShowNotice("Failed to log in:\n" + message);
            }
        }
        void Pass()
        {
            if (MainGame.Screen.Mode == ScreenMode.Login)
            {
                MainGame.SetScreen(ScreenMode.MainMenu);
            }
        }
    }
}
