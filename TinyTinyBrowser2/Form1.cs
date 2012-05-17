using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TinyTinyBrowser2
{
    public partial class Form1 : Form
    {
        ArrayList tempHistory = new ArrayList();
        public Form1()
        {
            InitializeComponent();
            txtStatus.Text = "";
            tabControl1.SelectedTab.Text = "(Untitled)";
            tabControl1.TabPages[1].Text = "+Tab";
            googleToolStripMenuItem.Checked = true;
            getCurrentBrowser().ScriptErrorsSuppressed = true;
        }

        private void addNewTab()
        {
            TabPage tab = new TabPage();
            tab.BorderStyle = BorderStyle.Fixed3D;
            tabControl1.TabPages.Insert(tabControl1.TabCount - 1, tab);
            WebBrowser browser = new WebBrowser();
            browser.Navigate("about:blank");
            tab.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            tabControl1.SelectTab(tab);
            tabControl1.SelectedTab.Text = "(Untitled)";
            browser.ProgressChanged += new WebBrowserProgressChangedEventHandler(webBrowser1_ProgressChanged);
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            browser.Navigating += new WebBrowserNavigatingEventHandler(webBrowser1_Navigating);
        }
        private void closeTab()
        {
            if (tabControl1.TabCount != 2)
            {
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == tabControl1.TabPages.Count - 1)
            {
                addNewTab();
            }
            if (getCurrentBrowser().Url != null)
                txtNavigate.Text = getCurrentBrowser().Url.ToString();
            else
                txtNavigate.Text = "about:blank";
            getCurrentBrowser().ScriptErrorsSuppressed = true;
        }

        private void txtNavigate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                btnGo_Click(sender, e);

                tempHistory.Add(getCurrentBrowser().Url);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate(txtNavigate.Text);
        }

        private WebBrowser getCurrentBrowser()
        {
            return (WebBrowser)tabControl1.SelectedTab.Controls[0];
        }

        private void webBrowser1_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            progressBar.Maximum = (int)e.MaximumProgress;
            if ((int)e.CurrentProgress < 0)
                progressBar.Value = (int)e.CurrentProgress + 1;
            else if (e.CurrentProgress > progressBar.Maximum)
                progressBar.Maximum = (int)e.CurrentProgress;
            else
                progressBar.Value = (int)e.CurrentProgress;
            txtStatus.Text = getCurrentBrowser().StatusText;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            txtNavigate.Text = getCurrentBrowser().Url.ToString();
            tabControl1.SelectedTab.Text = getCurrentBrowser().DocumentTitle;
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {

        }

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void addNewTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addNewTab();
        }

        private void closeTabToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (tempHistory.Count > 1)
            {
                getCurrentBrowser().Navigate(tempHistory[tempHistory.Count-1].ToString());
                tempHistory.RemoveAt(tempHistory.Count - 1);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                btnSearch_Click(sender, e);

                tempHistory.Add(getCurrentBrowser().Url);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (googleToolStripMenuItem.Checked)
            {
                getCurrentBrowser().Navigate("http://www.google.com/search?q=" + txtSearch.Text);
            }
            else if (bingToolStripMenuItem.Checked)
            {
                getCurrentBrowser().Navigate("http://www.bing.com/search?q=" + txtSearch.Text);
            }
        }

        private void googleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bingToolStripMenuItem.Checked)
                bingToolStripMenuItem.Checked = false;
        }

        private void bingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (googleToolStripMenuItem.Checked)
                googleToolStripMenuItem.Checked = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Stop();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Refresh();
        }

    }
}
