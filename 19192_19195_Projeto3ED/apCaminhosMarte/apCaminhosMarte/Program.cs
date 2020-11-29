using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    // Nome: Pedro Go Iqueda RA: 19195
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMapa());
        }
    }
}
