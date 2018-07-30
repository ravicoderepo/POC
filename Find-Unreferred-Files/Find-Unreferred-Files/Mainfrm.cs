using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace POC.TOOLS
{
    public partial class Mainfrm : Form
    {
        public Mainfrm()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            pbLoader.Visible = true;

            if (string.IsNullOrEmpty(txtProjectpath.Text))
            {
                MessageBox.Show("Select Project Path!");
            }
            else
            {
                List<string> listofFiles = DirSearch(txtProjectpath.Text, txtExtensions.Text, txtFindIn.Text, pbLoader);
                int iSNo = 1;
                var filesFound = listofFiles.Select(x => new {  SNo = iSNo ++, FilePath = x }).ToList();

                dtFilesGrid.DataSource = filesFound;

                lblCount.Text = filesFound.Count().ToString() + " found.";

                pbLoader.Visible = false;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtProjectpath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        public static List<string> DirSearch(string sDir, string findInExtn, string findExtn , ProgressBar progress )
        {
            StringBuilder sb = new StringBuilder();
            List<string> fileList = new List<string>();
            IEnumerable<string> listFileOnSearch = new List<string>();
            IEnumerable<string> listFileSearch = new List<string>();
            try
            {               
                listFileOnSearch = GetFiles(sDir, findExtn.Split(';'), SearchOption.AllDirectories);
                listFileSearch = GetFiles(sDir, findInExtn.Split(';'), SearchOption.AllDirectories);

                string readText = string.Empty;
                string searchTxt = string.Empty;

                

                foreach (var searchOnFile in listFileOnSearch)
                {
                    readText = File.ReadAllText(searchOnFile);

                    foreach (var searchInFile in listFileSearch)
                    {
                        if (readText.IndexOf("/" + searchInFile.Split('\\')[searchInFile.Split('\\').Length - 1], StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            fileList.Add(searchInFile);
                        }
                    }

                    progress.Value += 5;
                }
                progress.Value = 100;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listFileSearch.Where(wh => !fileList.Contains(wh.ToString())).ToList();
        }

        public static List<string> GetFiles(string path,
                       string[] searchPatterns,
                       SearchOption searchOption = SearchOption.AllDirectories)
        {
            return searchPatterns.AsParallel()
                   .SelectMany(searchPattern =>
                          Directory.EnumerateFiles(path, searchPattern, searchOption)).ToList();
        }

        private void Mainfrm_Load(object sender, EventArgs e)
        {
            pbLoader.Minimum = 0;
            pbLoader.Maximum = 200;
        }
    }
}
