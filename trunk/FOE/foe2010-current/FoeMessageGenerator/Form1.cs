using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Foe.Common;
using Foe.Server;

namespace FoeMessageGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // generate a sample message
            FoeMessage msg = new FoeMessage();
            msg.Add(new FoeMessageItem("RequestType", "RssFeed"));
            msg.Add(new FoeMessageItem("FeedId", "VOACHINESE"));
            tbxSource.Text = msg.ToXml();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] compressed = CompressionManager.Compress(Encoding.UTF8.GetBytes(tbxSource.Text));
            tbxTarget.Text = Convert.ToBase64String(compressed);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] compressed = Convert.FromBase64String(tbxTarget.Text);
            tbxSource.Text = Encoding.UTF8.GetString(CompressionManager.Decompress(compressed));
        }
    }
}
