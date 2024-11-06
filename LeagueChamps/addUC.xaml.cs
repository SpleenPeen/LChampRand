using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            bool[] roles = new bool[5];
            var boxes = addGrid.Children.OfType<CheckBox>().ToArray();

            name = addGrid.Children.OfType<TextBox>().First().Text;
            for (int i = 0; i < boxes.Length; i++)
            {
                roles[i] = (bool)boxes[i].IsChecked;
            }

            if(ChampController.AddChampion(name, roles))
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
