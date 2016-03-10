using System;
using System.Windows.Forms;
using ANDREICSLIB;
using ANDREICSLIB.Licensing;
using DotNetDetector.ServiceReference1;

namespace DotNetDetector
{
    public partial class Form1 : Form
    {
        #region licensing
        private const String HelpString = "";

        private readonly String OtherText =
            @"©" + DateTime.Now.Year +
            @" Andrei Gec (http://www.andreigec.net)

Licensed under GNU LGPL (http://www.gnu.org/)

Zip Assets © SharpZipLib (http://www.sharpdevelop.net/OpenSource/SharpZipLib/)
";

        #endregion

        public Form1()
        {
            InitializeComponent();

            Licensing.LicensingForm(this, menuStrip1, HelpString, OtherText);
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
