using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace netwar
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool done = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while (!done)
            {
                Startup form = new Startup();
                Application.Run(form);

                if (form.StartGame)
                    done = new Game().Play();
                else
                    done = true;
            }
        }
    }
}
