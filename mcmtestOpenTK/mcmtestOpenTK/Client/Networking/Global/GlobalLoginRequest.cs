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

namespace mcmtestOpenTK.Client.Networking.Global
{
    public class GlobalLoginRequest : GlobalNetwork
    {
        /// <summary>
        /// Sends a message to the Global Server, to begin logging in.
        /// </summary>
        /// <param name="Announce">Whether to announce the result in the console</param>
        /// <param name="username">The username to login with</param>
        /// <param name="password">The password to login with</param>
        /// <returns>The GlobalNetwork object created for this time request</returns>
        public static GlobalLoginRequest RequestLogin(bool Announce, string username, string password)
        {
            GlobalLoginRequest glr = new GlobalLoginRequest();
            glr.Username = username;
            glr.Password = password;
            NetworkingObjects.Add(glr);
            glr.Send();
            return glr;
        }

        /// <summary>
        /// Whether the login succeeded.
        /// </summary>
        public bool Success = false;

        /// <summary>
        /// Whether the success/failure state of the login should be announced.
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

        Socket socket;

        string Error = null;

        Object Locker = new Object();

        public override void Send()
        {
            socket = NetworkUtil.CreateSocket();
            Thread thr = new Thread(new ThreadStart(ConnectMe));
            thr.Start();
        }
        public byte[] correctblocksize(byte[] input, int size)
        {
            byte[] toret = new byte[size];
            int len = 0;
            while (input.Length + len <= size)
            {
                input.CopyTo(toret, len);
                len += input.Length;
            }
            if (len != size)
            {
                Array.Copy(input, 0, toret, len, size - len);
            }
            return toret;
        }
        void ConnectMe()
        {
            try
            {
                socket.Connect(GlobalNetwork.GlobalAddress, GlobalNetwork.GlobalPort);
                ECDiffieHellmanCng DHC = new ECDiffieHellmanCng();
                DHC.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                DHC.HashAlgorithm = CngAlgorithm.Sha256;
                byte[] PubKey = DHC.PublicKey.ToByteArray();
                byte[] SendMe = new byte[5 + PubKey.Length];
                SendMe[0] = 42;
                BitConverter.GetBytes(PubKey.Length).CopyTo(SendMe, 1);
                PubKey.CopyTo(SendMe, 5);
                socket.Send(SendMe); // PACKET SEND 1: client public key -> server
                int len = 0;
                byte[] GotBack = new byte[4];
                while (len < 4)
                {
                    byte[] tbyte = new byte[4 - len];
                    int blen = socket.Receive(tbyte, 4 - len, SocketFlags.None);
                    tbyte.CopyTo(GotBack, len);
                    len += blen;
                }
                int FullDataLength = BitConverter.ToInt32(GotBack, 0);
                if (FullDataLength > 10 * 1024 || FullDataLength < 1)
                {
                    Error = "Received invalid data length '" + FullDataLength + "'";
                    return;
                }
                len = 0;
                GotBack = new byte[FullDataLength];
                while (len < FullDataLength)
                {
                    byte[] tbyte = new byte[FullDataLength - len];
                    int blen = socket.Receive(tbyte, FullDataLength - len, SocketFlags.None);
                    tbyte.CopyTo(GotBack, len);
                    len += blen;
                } // PACKET RECEIVE 1: server public key -> client
                byte[] Derived = DHC.DeriveKeyMaterial(CngKey.Import(GotBack, CngKeyBlobFormat.EccPublicBlob));
                Aes AES = Aes.Create();
                AES.Mode = CipherMode.CBC;
                byte[] IV = correctblocksize(Derived, 16);
                ICryptoTransform ICT = AES.CreateEncryptor(Derived, IV);
                byte[] LoginInfo = FileHandler.encoding.GetBytes("LOGIN\n" + Username + "\n" + Password);
                DataStream DS = new DataStream();
                CryptoStream CS = new CryptoStream(DS, ICT, CryptoStreamMode.Write);
                CS.Write(LoginInfo, 0, LoginInfo.Length);
                CS.Close();
                LoginInfo = DS.ToArray();
                DS.Close();
                SendMe = new byte[4 + LoginInfo.Length];
                BitConverter.GetBytes(LoginInfo.Length).CopyTo(SendMe, 0);
                LoginInfo.CopyTo(SendMe, 4);
                socket.Send(SendMe); // PACKET SEND 2: login request data -> server
                len = 0;
                GotBack = new byte[4];
                while (len < 4)
                {
                    byte[] tbyte = new byte[4 - len];
                    int blen = socket.Receive(tbyte, 4 - len, SocketFlags.None);
                    tbyte.CopyTo(GotBack, len);
                    len += blen;
                }
                FullDataLength = BitConverter.ToInt32(GotBack, 0);
                if (FullDataLength > 10 * 1024 || FullDataLength < 1)
                {
                    Error = "Received invalid data length '" + FullDataLength + "'";
                    return;
                }
                len = 0;
                GotBack = new byte[FullDataLength];
                while (len < FullDataLength)
                {
                    byte[] tbyte = new byte[FullDataLength - len];
                    int blen = socket.Receive(tbyte, FullDataLength - len, SocketFlags.None);
                    tbyte.CopyTo(GotBack, len);
                    len += blen;
                } // PACKET RECEIVE 2: result -> client
                ICT = AES.CreateDecryptor(Derived, IV);
                DS = new DataStream(GotBack);
                CS = new CryptoStream(DS, ICT, CryptoStreamMode.Read);
                DataStream DS2 = new DataStream();
                CS.CopyTo(DS2);
                CS.Close();
                DS.Close();
                GotBack = DS2.ToArray();
                DS2.Close();
                Error = FileHandler.encoding.GetString(GotBack);
            }
            catch (Exception ex)
            {
                lock (Locker)
                {
                    Error = ex.Message;
                }
            }
        }

        public override void TickMe()
        {
            lock (Locker)
            {
                if (Error != null)
                {
                    TimeRan = GlobalNetwork.MaxRunTime;
                    KillQuietly = true;
                    UIConsole.WriteLine(TextStyle.Color_Error + "Login failed with message: " + TextStyle.Color_Separate + Error);
                    return;
                }
            }
            if (Success)
            {
                if (ShouldAnnounce)
                {
                    UIConsole.WriteLine(TextStyle.Color_Importantinfo + "Successfully logged in as " +
                        TextStyle.Color_Separate + Username + TextStyle.Color_Importantinfo + "!");
                }
                ready = true;
            }
        }

        public override void Kill()
        {
            try
            {
                socket.Close();
            }
            catch (Exception)
            {
                // Irrelevant, we already want to scrap this entirely.
            }
        }
    }
}
