using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mcmtestOpenTK.Shared
{
    class AccountFileSaver
    {
        public static string[] GetAccountData()
        {
            try
            {
                return FileHandler.encoding.GetString(FileHandler.UnGZip(
                    File.ReadAllBytes(Environment.CurrentDirectory + "/account.dat"))).Split('\n');
            }
            catch
            {
                return new string[] { "", "", "" };
            }
        }

        public static void SaveAccountData(string name, string pass, string save)
        {
            try
            {
                File.WriteAllBytes(Environment.CurrentDirectory + "/account.dat",
                    FileHandler.GZip(FileHandler.encoding.GetBytes(name + "\n" + pass + "\n" + save)));
            }
            catch
            {
                return;
            }
        }
    }
}
