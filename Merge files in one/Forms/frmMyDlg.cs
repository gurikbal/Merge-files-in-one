using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NppPluginNET;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Merge_files_in_one
{
    public partial class frmMyDlg : Form
    {
        public static string outputString;
        internal const string PluginName = "Merge files in one";
        static string iniFilePath = null;
        static bool someSetting = false;

        public frmMyDlg()
        {
            InitializeComponent();

            StringBuilder sbIniFilePathkk = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePathkk);
            iniFilePath = sbIniFilePathkk.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");


            StringBuilder filename = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFILENAME, 0, filename);

            label1.Text = filename.ToString();
            
            textBox1.Text = GetCurrentDocumentText();
            Win32.WritePrivateProfileString(PluginName, "NPPN_BUFFERACTIVATED_N", "True", iniFilePath);
            Win32.WritePrivateProfileString(PluginName, "filename", filename.ToString(), iniFilePath);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var countm = textBox1.Lines.Length;
                var countnn = textBox2.Lines.Length;
                if (countm > countnn)
                {
                    for (int i = 0; i < countnn; i++)
                    {
                        textBox1.AppendText(Environment.NewLine);
                    }
                }
                else
                {
                    for (int i = 0; i < countnn; i++)
                    {
                        // MessageBox.Show(i.ToString());
                        textBox1.AppendText(Environment.NewLine);
                    }
                    // MessageBox.Show(textBox1.Lines.Length.ToString() + "yyyy" + textBox1.Lines.Length.ToString());
                }
                var count = textBox1.Lines.Length;
                var countn = textBox2.Lines.Length;             
                var kmj = textBox1.Lines.Length;
                if ((count > countn) || (count == countn)) {kmj = countn;} else {kmj = count;};
                    int x = -1;
                    string linex;
                    string[] lines = textBox1.Lines;
                    string[] line = textBox2.Lines;
                    for (int i = 0; i < kmj; i++)
                      {
                          x += 1;
                          if (checkBox1.Checked == true)
                          {
                              linex = Regex.Replace(line[x], "$", " 3f5456cfsd661lld33dGuid9CA0F324 " + lines[x]);
                              lines[x] = linex;
                              textBox3.Lines = lines;
                          }
                          else
                          {
                              linex = Regex.Replace(lines[x], "$", " 3f5456cfsd661lld33dGuid9CA0F324 " + line[x]);
                              lines[x] = linex;
                              textBox3.Lines = lines;
                          }
                      }
                    outputString = textBox3.Text;
                    textBox3.Clear();
                if (string.IsNullOrEmpty(outputString)) return;
                Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_FILE_NEW);

                Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_SETTEXT, 0, outputString.TrimEnd());
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error. " + exp.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        public static void NPPN_BUFFERACTIVATED_N()
        {
            someSetting = (Win32.GetPrivateProfileInt(PluginName, "nppn_bufferactivated_n", 0, iniFilePath) != 0);
            if (someSetting.ToString() == "False")
            {
                return;
            }
            StringBuilder cur_filename = new StringBuilder(Win32.MAX_PATH);
            StringBuilder sbTempm = new StringBuilder(Win32.MAX_PATH);
            Win32.GetPrivateProfileString(PluginName, "filename", "", sbTempm, Win32.MAX_PATH, iniFilePath);
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFILENAME, 0, cur_filename);
            if (cur_filename.ToString() == sbTempm.ToString())
            {
                return;
            }
            else
            {
                Win32.WritePrivateProfileString(PluginName, "filename", cur_filename.ToString(), iniFilePath);
            }
        }
        private string GetCurrentDocumentText()
        {
            IntPtr curScintilla = PluginBase.GetCurrentScintilla();
            return GetDocumentText(curScintilla);
        }

        private string GetDocumentText(IntPtr curScintilla)
        {
            int length = (int)Win32.SendMessage(curScintilla, SciMsg.SCI_GETLENGTH, 0, 0) + 1;
            StringBuilder sb = new StringBuilder(length);
            Win32.SendMessage(curScintilla, SciMsg.SCI_GETTEXT, length, sb);
            return sb.ToString();
        }

        private void Form_Activated(object sender, EventArgs e)
        {
			StringBuilder sbTemp = new StringBuilder(Win32.MAX_PATH);
            StringBuilder cur_filenamen = new StringBuilder(Win32.MAX_PATH);

                Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFILENAME, 0, cur_filenamen);

                Win32.GetPrivateProfileString(PluginName, "filename", "", sbTemp, Win32.MAX_PATH, iniFilePath);
                if (sbTemp.ToString() == cur_filenamen.ToString())
                {
                    label3.Text = sbTemp.ToString() + " is selected \n swicth to second file now";
                    return;
                }
                else
                {
                    label3.Visible = false;
                    button1.Visible = true;
                    label2.Text = cur_filenamen.ToString();
                    textBox2.Text = GetCurrentDocumentText();
                    
                }

        }

        private void form_closing(object sender, FormClosingEventArgs e)
        {

            Win32.WritePrivateProfileString(PluginName, "nppn_bufferactivated_n", "False", iniFilePath);
            
        }
    }
}
