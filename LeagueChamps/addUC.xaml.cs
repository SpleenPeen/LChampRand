using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LeagueChamps
{
    public partial class addUC : UserControl
    {
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

            ChampController.AddChampion(name, list.ToArray());
        }

        private void DragImg(object sender, DragEventArgs e)
        {
            MessageBox.Show(e.Data.ToString());
        }
    }
}
