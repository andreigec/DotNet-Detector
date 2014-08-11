using System;
using System.Windows.Forms;
using ANDREICSLIB;
using DotNetDetector.ServiceReference1;

namespace DotNetDetector
{
    public partial class Form1 : Form
    {
        #region licensing
        private const string AppTitle = "DotNetDetector";
        private const double AppVersion = 0.1;
        private const String HelpString = "";

        private readonly String OtherText =
            @"©" + DateTime.Now.Year +
            @" Andrei Gec (http://www.andreigec.net)

Licensed under GNU LGPL (http://www.gnu.org/)

Zip Assets © SharpZipLib (http://www.sharpdevelop.net/OpenSource/SharpZipLib/)
";

        public Licensing.DownloadedSolutionDetails GetDetails()
        {
            try
            {
                var sr = new ServicesClient();
                var ti = sr.GetTitleInfo(AppTitle);
                if (ti == null)
                    return null;
                return ToDownloadedSolutionDetails(ti);

            }
            catch (Exception)
            {
            }
            return null;
        }

        public static Licensing.DownloadedSolutionDetails ToDownloadedSolutionDetails(TitleInfoServiceModel tism)
        {
            return new Licensing.DownloadedSolutionDetails()
            {
                ZipFileLocation = tism.LatestTitleDownloadPath,
                ChangeLog = tism.LatestTitleChangelog,
                Version = tism.LatestTitleVersion
            };
        }
        #endregion

        public Form1()
        {
            InitializeComponent();

            Licensing.CreateLicense(this, menuStrip1,
                new Licensing.SolutionDetails(GetDetails, HelpString, AppTitle, AppVersion, OtherText));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Controller.Refresh(this);
        }

        public void AddVersion(NetVersion v)
        {
            var lvi = new ListViewItem(v.Version);
            lvi.SubItems.Add(v.SubVersion);
            lvi.SubItems.Add(v.ServicePack);
            versions.Items.Add(lvi);
        }

    }
}
