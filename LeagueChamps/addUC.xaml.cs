using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LeagueChamps
{
    public partial class addUC : UserControl
    {
        string imgFilepath;

        public addUC()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = "";
            List<Role> list = new List<Role>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(addGrid); i++)
            {
                var child = VisualTreeHelper.GetChild(addGrid, i);
                if (child is TextBox)
                {
                    var txtBox = (TextBox)child;
                    name = txtBox.Text;
                    continue;
                }

                if(child is CheckBox)
                {
                    var check = (CheckBox)child;
                    if (check.IsChecked != true)
                        continue;

                    list.Add(Enum.Parse<Role>(check.Content.ToString()));
                }
            }

            if(ChampController.AddChampion(name, list.ToArray()))
            {
                if (imgFilepath == null)
                    return;
                ImgHandler.CopyImage(imgFilepath, name);
                imgFilepath = null;
                addImg.Source = null;
            }
        }

        private void DragImg(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filepath = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (filepath.Length > 0)
                {
                    imgFilepath = filepath[0];
                    BitmapImage img = new BitmapImage(new Uri(filepath[0], UriKind.Absolute));
                    addImg.Source = img;
                }
            }
        }
    }
}
