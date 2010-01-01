using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Project24
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool done = false;

            if (!FindResources())
            {
                MessageBox.Show("Resources not found, exiting");
                return;
            }

            while (!done)
            {
                if (false)
                {
                    new Game(new ConnectionInfo()).Run();
                    done = true;
                }
                else
                {
                    LoginForm login = new LoginForm();
                    login.Setup();
                    Application.Run(login);
                    if (login.play)
                    {
                        // play
                        ConnectionInfo info = new ConnectionInfo();

                        info.Avatar = login.GetAvatarName();
                        info.Username = login.UserName;
                        info.SelfServ = login.selfServ;
                        new Game(info).Run();
                    }
                    else
                        done = true;
                }
            }
        }

        static bool CheckPath( string path )
        {
            string resVer = "Resources/ResVer.txt";
            FileInfo file = new FileInfo(Path.Combine(path, resVer));
            if (file.Exists)
            {
                ResourceManager.AddPath(Path.Combine(path, "Resources")); 
                return true;
            }
            return false;
        }

        static bool FindResources ()
        {
            if (CheckPath("./"))
                return true;

            if (CheckPath("../"))
                return true;

            if (CheckPath("../../"))
                return true;

            if (CheckPath("../../../"))
                return true;

            return false;
        }
    }
}
