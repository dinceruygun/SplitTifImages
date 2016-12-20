using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplitTifImages
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        NCZFolder nczSourceFolder;
        NCZFolder nczTargetFolder;
        NCZFiles nczFiles;
        string searchFileName = "*.tif";
        bool processStatus = false;

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            lblFolder.Text = ShowFolder();

            if (!(string.IsNullOrEmpty(lblFolder.Text)))
                nczSourceFolder = new NCZFolder(lblFolder.Text);
            else
                lblFolder.Text = "-";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblTargetFolder.Text = ShowFolder();

            if (!(string.IsNullOrEmpty(lblTargetFolder.Text)))
                nczTargetFolder = new NCZFolder(lblTargetFolder.Text);
            else
                lblTargetFolder.Text = "-";
        }


        private string ShowFolder()
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    return folderDialog.SelectedPath;
                }
            }

            return "";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            if (processStatus == false)
            {
                processStatus = true;
                StartProcess();
                btnStart.Text = "Durdur";
            }
            else
            {
                processStatus = false;
                btnStart.Text = "Başlat";
            }

            
        }


        private void StartProcess()
        {
            try
            {
                EnableDisableUI(false);

                int completeFileCount = 0;

                lblStatus.Text = "Kaynak klasör okunuyor...";
                Application.DoEvents();

                if (nczSourceFolder == null)
                {
                    MessageBox.Show("Kaynak klasör seçmelisiniz!", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (nczTargetFolder == null)
                {
                    MessageBox.Show("Hedef klasör seçmelisiniz!", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }


                nczFiles = nczSourceFolder.LoadFiles(searchFileName);

                if (nczFiles?.Count == 0)
                {
                    MessageBox.Show("Kaynak klasör içerisinde dönüştürülecek dosya bulunamadı!", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }


                progressBar1.Value = 0;
                progressBar1.Maximum = nczFiles.Count;

                foreach (var file in nczFiles)
                {
                    if (processStatus == false) {
                        btnStart.Text = "Başlat";
                        processStatus = false;
                        return;
                    }


                    string targetPath = nczTargetFolder.FullPath + file.FilePath.Replace(nczSourceFolder.FullPath, "");
                    System.IO.FileInfo fif = new System.IO.FileInfo(targetPath);


                    NCZImage.SplitImage(file.FilePath, fif.Directory.FullName);




                    completeFileCount++;

                    lblStatus.Text = $"{completeFileCount.ToString()} / {nczFiles.Count.ToString()}";
                    progressBar1.Value = completeFileCount;

                    Application.DoEvents();
                }

                btnStart.Text = "Başlat";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnStart.Text = "Başlat";
                processStatus = false;
                EnableDisableUI(true);
            }
        }



        




        private void EnableDisableUI(bool status)
        {
            btnOpenFolder.Enabled = status;
            button1.Enabled = status;
        }
    }
}
