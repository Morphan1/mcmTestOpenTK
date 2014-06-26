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

namespace mcmtestOpenTK.Client.Networking.OneOffs
{
    public abstract class GlobalSecureRequest : NetPing
    {
        /// <summary>
        /// The networking socket.
        /// </summary>
        public Socket socket;

        /// <summary>
        /// Any received error message.
        /// </summary>
        public string Error = null;

        /// <summary>
        /// Used for cross-thread locking of important data (results).
        /// </summary>
        public Object Locker = new Object();

        /// <summary>
        /// Adjusts random byte array input to match a certain length, in a lazy manner.
        /// </summary>
        /// <param name="input">The base byte array</param>
        /// <param name="size">How long it needs to be</param>
        /// <returns>The resized byte array</returns>
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

        /// <summary>
        /// Should be sufficiently implemented.
        /// </summary>
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

        /// <summary>
        /// Sends a block of bytes, formatted as the secure request system expects, optionally adding an ID number to the start.
        /// </summary>
        /// <param name="block">The bytes to send</param>
        public void SendByteBlock(byte[] block, int preid = -1)
        {
            byte[] SendMe = new byte[(preid >= 0 ? 5: 4) + block.Length];
            if (preid >= 0)
            {
                SendMe[0] = (byte)preid;
            }
            BitConverter.GetBytes(block.Length).CopyTo(SendMe, (preid >= 0 ? 1 : 0));
            block.CopyTo(SendMe, (preid >= 0 ? 5 : 4));
            socket.Send(SendMe);
        }

        /// <summary>
        /// Receives the next block from the server.
        /// </summary>
        /// <returns>The bytes received</returns>
        public byte[] ReceiveBlock()
        {
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
                throw new Exception("Received invalid data length '" + FullDataLength + "'");
            }
            len = 0;
            GotBack = new byte[FullDataLength];
            while (len < FullDataLength)
            {
                byte[] tbyte = new byte[FullDataLength - len];
                int blen = socket.Receive(tbyte, FullDataLength - len, SocketFlags.None);
                tbyte.CopyTo(GotBack, len);
                len += blen;
            }
            return GotBack;
        }

        /// <summary>
        /// Encrypts a byte set for transmission.
        /// </summary>
        /// <param name="AES">The AES object to use for encryption</param>
        /// <param name="Key">The key to encrypt with</param>
        /// <param name="data">The data to encrypt</param>
        /// <returns>The encrypted data</returns>
        public byte[] Encrypt(Aes AES, byte[] Key, byte[] data)
        {
            byte[] IV = correctblocksize(Key, 16);
            ICryptoTransform ICT = AES.CreateEncryptor(Key, IV);
            DataStream DS = new DataStream();
            CryptoStream CS = new CryptoStream(DS, ICT, CryptoStreamMode.Write);
            CS.Write(data, 0, data.Length);
            CS.Close();
            byte[] toret = DS.ToArray();
            DS.Close();
            return toret;
        }

        /// <summary>
        /// Decrypts a transmitted byte set.
        /// </summary>
        /// <param name="AES">The AES object to use for decryption</param>
        /// <param name="Key">The key to decrypt with</param>
        /// <param name="data">The data to decrypt</param>
        /// <returns>The decrypted data</returns>
        public byte[] Decrypt(Aes AES, byte[] Key, byte[] data)
        {
            byte[] IV = correctblocksize(Key, 16);
            ICryptoTransform ICT = AES.CreateDecryptor(Key, IV);
            DataStream DS = new DataStream(data);
            CryptoStream CS = new CryptoStream(DS, ICT, CryptoStreamMode.Read);
            DataStream DS2 = new DataStream();
            CS.CopyTo(DS2);
            CS.Close();
            DS.Close();
            byte[] toret = DS2.ToArray();
            DS2.Close();
            return toret;
        }

        /// <summary>
        /// Connects through the secure server and sends the specified initial data.
        /// </summary>
        /// <param name="datatosend">The data to initially send</param>
        /// <returns>the result from the server</returns>
        public string SecureConnectBase(string datatosend)
        {
            try
            {
                socket.Connect(NetPing.GlobalAddress, NetPing.GlobalPort);
                ECDiffieHellmanCng DHC = new ECDiffieHellmanCng();
                DHC.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                DHC.HashAlgorithm = CngAlgorithm.Sha256;
                byte[] PubKey = DHC.PublicKey.ToByteArray();
                SendByteBlock(PubKey, 42); // PACKET SEND 1: client public key -> server
                byte[] GotBack = ReceiveBlock(); // PACKET RECEIVE 1: server public key -> client
                byte[] Derived = DHC.DeriveKeyMaterial(CngKey.Import(GotBack, CngKeyBlobFormat.EccPublicBlob));
                Aes AES = Aes.Create();
                AES.Mode = CipherMode.CBC;
                byte[] LoginInfo = Encrypt(AES, Derived, FileHandler.encoding.GetBytes(datatosend));
                SendByteBlock(LoginInfo); // PACKET SEND 2: login request data -> server
                GotBack = Decrypt(AES, Derived, ReceiveBlock()); // PACKET RECEIVE 2: result -> client
                return FileHandler.encoding.GetString(GotBack);
            }
            catch (Exception ex)
            {
                lock (Locker)
                {
                    Error = ex.Message;
                    return "";
                }
            }
        }
    }
}
