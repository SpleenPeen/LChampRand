using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Form = System.Windows.Forms;
using System.Windows.Media;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace LeagueChamps
{
    public partial class MainWindow : Window
    {
        Brush butCol;
        Brush selButCol;
        Form.NotifyIcon notifIcon;
        bool close;

        public MainWindow()
        {
            InitializeComponent();

            //load champs
            ChampController.LoadChamps();

            //save colours and reset edit buts colour to unselected
            butCol = rollBut.Background;
            selButCol = editBut.Background;
            editBut.Background = butCol;

            //create notif tray icon
            notifIcon = new Form.NotifyIcon();
            notifIcon.Icon = new System.Drawing.Icon("humzh ico.ico");
            notifIcon.Text = "League Champ Randomiser";
            notifIcon.Visible = true;
            notifIcon.DoubleClick += OnNotifClick;

            close = false;
            notifIcon.ContextMenuStrip = new Form.ContextMenuStrip();
            notifIcon.ContextMenuStrip.Items.Add("Close", null, OnCloseClick);
        }

        private void SwitchScreen(UserControl cntrl, Button but)
        {
            if (but.Background == selButCol)
                return;

            CC.Content = cntrl;

            addBut.Background = butCol;
            editBut.Background = butCol;
            rollBut.Background = butCol;

            but.Background = selButCol;
        }

        private void editBut_Click(object sender, RoutedEventArgs e)
        {
            SwitchScreen(new editUC(), editBut);
        }

        private void addBut_Click(object sender, RoutedEventArgs e)
        {
            SwitchScreen(new addUC(), addBut);
        }

        private void rollBut_Click(object sender, RoutedEventArgs e)
        {
            SwitchScreen(new rollUC(), rollBut);
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            close = true;
            this.Close();
        }

        private void OnNotifClick(object sender, EventArgs e)
        {
            this.Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!close)
                e.Cancel = true;
            this.Hide();
        }
    }
}
