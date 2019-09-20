using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using NppPluginNET;

namespace Merge_files_in_one
{
    class Main
    {
        #region " Fields "
        internal const string PluginName = "Merge files in one";
        static string iniFilePath = null;
        static frmMyDlg frmMyDlg = null;
        static bool someSetting = false;
        #endregion

        #region " StartUp/CleanUp "
        internal static void CommandMenuInit()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");

            PluginBase.SetCommand(0, "Merge files in one", all_file_text, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(3, "About", about, new ShortcutKey(false, false, false, Keys.None));
        }
        internal static void SetToolBarIcon()
        {

        }
        
        internal static void PluginCleanUp()
        {
            Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
        }
        #endregion

        #region " Menu functions "
        internal static void about()
        {
            var ss = "\n        ****** Merge files in one ******  \n                    build by G. Singh  \n                20-09-2019 build 1.0.0.0  ";
            MessageBox.Show(ss);
        }
        internal static void all_file_text()
        {
            frmMyDlg = new frmMyDlg();
            frmMyDlg.Show();
            frmMyDlg = null;
        }
        #endregion
    }
}