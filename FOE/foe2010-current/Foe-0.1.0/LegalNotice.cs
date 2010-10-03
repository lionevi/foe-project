using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;

namespace Foe
{
    public partial class LegalNotice : Form
    {
        private DialogResult _result = DialogResult.Cancel;

        private ResourceManager m_ResourceManager = new ResourceManager("Foe.FoeResource", System.Reflection.Assembly.GetExecutingAssembly());

        public LegalNotice()
        {
            InitializeComponent();
            wbLegalNotice.DocumentText = m_ResourceManager.GetString("Copyright");
        }

        private void btnAgree_Click(object sender, EventArgs e)
        {
            _result = DialogResult.OK;
            Close();
        }

        private void btnDisagree_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LegalNotice_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = _result;
        }
    }
}
