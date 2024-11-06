using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;
using System.Xml.Linq;

namespace LeagueChamps
{
    public partial class editUC : UserControl
    {
        private int listStartIndex;

        public editUC()
        {
            InitializeComponent();

            listStartIndex = editPanel.Children.Count;

            for (int i = 0; i < ChampController.champs.Count; i++)
            {
                StackPanel curPan = new StackPanel();
                curPan.Orientation = Orientation.Horizontal;

                //enabled checkbox
                CheckBox enabled = new CheckBox();
                enabled.IsChecked = ChampController.champs[i].Enabled;

                //image
                Border brdr = new Border();
                Image img = new Image();
                ImgHandler.SetImage(img, ChampController.champs[i]);
                img.Width = 40;
                img.Height = 40;
                brdr.Child = img;
                brdr.BorderThickness = new Thickness(1, 1, 1, 1);
                brdr.BorderBrush = new SolidColorBrush(Color.FromRgb(208, 208, 208));

                //name
                TextBox name = new TextBox();
                name.Height = 20;
                name.Width = 100;
                name.Text = ChampController.champs[i].Name;
                name.Margin = new Thickness(10, 0, 0, 0);

                //roles
                CheckBox[] roles = new CheckBox[5];
                for (int x = 0; x < roles.Length; x++)
                {
                    roles[x] = new CheckBox();
                    roles[x].VerticalAlignment = VerticalAlignment.Center;
                    roles[x].Margin = new Thickness(10, 0, 0, 0);
                    roles[x].IsChecked = ChampController.champs[i].Roles[x];
                }
                roles[0].Margin = new Thickness(20, 0, 0, 0);

                //set role names
                roles[0].Content = "Top";
                roles[1].Content = "Jun";
                roles[2].Content = "Mid";
                roles[3].Content = "Bot";
                roles[4].Content = "Sup";

                //add control buttons
                StackPanel ctrlButs = new StackPanel();
                Button editBut = new Button();
                editBut.Width = 50;
                editBut.Height = 18;
                editBut.Content = "Save";
                editBut.Click += new RoutedEventHandler(Save_Onclick);

                Button removeBut = new Button();
                removeBut.Width = 50;
                removeBut.Height = 18;
                removeBut.Content = "Remove";
                removeBut.Margin = new Thickness(0, 4, 0, 0);
                removeBut.Click += new RoutedEventHandler(Remove_Onclick);

                ctrlButs.Children.Add(editBut);
                ctrlButs.Children.Add(removeBut);
                ctrlButs.Margin = new Thickness(10, 0, 0, 0);

                //add all elements to current stack panel
                curPan.Margin = new Thickness(10, 10, 0, 0);
                curPan.Children.Add(enabled);
                curPan.Children.Add(brdr);
                curPan.Children.Add(name);
                foreach (CheckBox box in roles)
                {
                    curPan.Children.Add(box);
                }
                curPan.Children.Add(ctrlButs);

                //add cur panel to main panel
                editPanel.Children.Add(curPan);
            }
        }

        private void Save_Onclick(object sender, EventArgs e)
        {
            //save button pressed
            Button curBut = sender as Button;

            //get the stack panel the button is a part of 
            StackPanel butPan = VisualTreeHelper.GetParent(curBut) as StackPanel;
            StackPanel curPan = VisualTreeHelper.GetParent(butPan) as StackPanel;

            //change the name and roles of the champ the panel is responsible for
            int curChampInd = editPanel.Children.IndexOf(curPan) - listStartIndex;

            //get important children
            TextBox curTxtBox = curPan.Children.OfType<TextBox>().First();
            CheckBox[] curChecks = curPan.Children.OfType<CheckBox>().ToArray();
            Image curImg = (Image)curPan.Children.OfType<Border>().First().Child;

            //save name and rename img
            if (ChampController.champs.Any(champ => champ.Name == curTxtBox.Text && ChampController.champs.IndexOf(champ) != curChampInd)) //abort if name is same as another champ
            {
                MessageBox.Show("Name already matches another champion, aborted changes");
                return;
            }

            ImgHandler.RenameImage(ChampController.champs[curChampInd].Name, curTxtBox.Text, curImg);
            ChampController.champs[curChampInd].Name = curTxtBox.Text;

            //save roles
            ChampController.champs[curChampInd].Enabled = (bool)curChecks[0].IsChecked;
            for (int i = 1; i < curChecks.Length; i++)
            {
                ChampController.champs[curChampInd].Roles[i-1] = (bool)curChecks[i].IsChecked;
            }

            //apply changes
            var champ = ChampController.champs[curChampInd];
            ChampController.SortChamps();
            ChampController.SaveData();

            //swap the panel order
            var newSpot = ChampController.champs.IndexOf(champ)+listStartIndex;
            editPanel.Children.Remove(curPan);
            editPanel.Children.Insert(newSpot, curPan);

            MessageBox.Show("Saved Data");
        }

        private void Remove_Onclick(object sender, EventArgs e)
        {
            MessageBoxResult conf = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);

            if (conf == MessageBoxResult.No)
                return;

            Button curBut = sender as Button;

            StackPanel butPan = VisualTreeHelper.GetParent(curBut) as StackPanel;
            StackPanel curPan = VisualTreeHelper.GetParent(butPan) as StackPanel;

            string champName = curPan.Children.OfType<TextBox>().First().Text;
            ChampController.champs.RemoveAt(editPanel.Children.IndexOf(curPan)-listStartIndex);
            ChampController.SaveData();
            editPanel.Children.Remove(curPan);
            ImgHandler.RemoveImage(champName);
        }
    }
}
