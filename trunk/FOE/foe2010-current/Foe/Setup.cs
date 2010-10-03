using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Foe
{
    public partial class Setup : Form
    {
        private DialogResult _dialogResult = DialogResult.Cancel;

        public Setup()
        {
            InitializeComponent();
        }

        private void Setup_Shown(object sender, EventArgs e)
        {
            btnReady.Focus();
        }

        private void btnReady_Click(object sender, EventArgs e)
        {
            _dialogResult = DialogResult.OK;
            Close();
        }

        private void btnNotReady_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Setup_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = _dialogResult;
        }
    }
}
