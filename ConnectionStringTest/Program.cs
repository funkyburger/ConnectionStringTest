using ConnectionStringTest.UI;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var module = new ApplicationModule();
            var kernel = new StandardKernel(module);

            DiContainer.Wire(kernel);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(DiContainer.Resolve<MainForm>());
        }
    }
}
