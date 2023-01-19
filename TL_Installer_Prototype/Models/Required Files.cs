using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Models
{
    internal static class Required_Files
    {
        private static string temmieLauncher;
        private static string nw_js;

        public static string TemmieLauncher { get => temmieLauncher; set => temmieLauncher = value; }
        public static string Nw_js { get => nw_js; set => nw_js = value; }
    }
}
