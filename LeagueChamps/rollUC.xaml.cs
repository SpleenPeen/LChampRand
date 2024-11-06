using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LeagueChamps
{
    /// <summary>
    /// Interaction logic for rollUC.xaml
    /// </summary>
    public partial class rollUC : UserControl
    {
        string? defLabel;
        Random rng = new Random();
        List<int>[] sortedChamps;
        List<Button> roleButtons;
        int rolledChamp;

        public rollUC()
        {
            InitializeComponent();
            defLabel = name.Content.ToString(); //store what the default name label entry was
            removeBut.IsEnabled = false; //disable remove button
            rolledChamp = -1; //set rolled champ to -1

            //sort champion indexes into lists for each role
            sortedChamps = new List<int>[5];
            for (int i = 0; i < 5; i++)
                sortedChamps[i] = new List<int>();

            for (int i = 0; i < ChampController.champs.Count; i++)
            {
                foreach (Role role in ChampController.champs[i].Roles)
                {
                    sortedChamps[(int)role].Add(i);
                }
            }

            //get role buttons
            roleButtons = rollGrid.Children.OfType<Button>().ToList();

            for (int i = 0; i < roleButtons.Count(); i++)
            {
                try
                {
                    var role = Enum.Parse<Role>(roleButtons[i].Content.ToString());
                }
                catch
                {
                    roleButtons.RemoveAt(i);
                    i--;
                }
            }

            DisableEmptyRoleButs();
        }

        private void Roll(object sender, RoutedEventArgs e)
        {
            //new index variable
            int newIndex = -1;

            //enable remove button
            removeBut.IsEnabled = true;

            //cast sender object as button
            Button pressed = (Button)sender;

            //if pressed button is in role buttons role that lane
            if (roleButtons.Contains(pressed))
            {
                Role role = Enum.Parse<Role>(pressed.Content.ToString());
                var excludedList = sortedChamps[(int)role].FindAll(index => index != rolledChamp);
                if (excludedList.Count == 0)
                    return;
                newIndex = excludedList[rng.Next(excludedList.Count)];
            }
            else
            {
                //otherwise roll from all champions 
                List<Champion> exludedChamps;
                if (rolledChamp != -1)
                    exludedChamps = ChampController.champs.FindAll(champ => champ != ChampController.champs[rolledChamp]);
                else
                    exludedChamps = ChampController.champs;

                if (exludedChamps.Count == 0)
                    return;
                newIndex = ChampController.champs.IndexOf(exludedChamps[rng.Next(exludedChamps.Count())]);
            }

            SelectChamp(newIndex);
        }

        private void SelectChamp(int newIndex)
        {
            rolledChamp = newIndex;
            name.Content = ChampController.champs[rolledChamp].Name;
            ImgHandler.SetImage(rollImg, ChampController.champs[rolledChamp]);
        }

        private void DisableEmptyRoleButs()
        {
            //disable all role buttons without any champs
            foreach (Button but in roleButtons)
            {
                if (sortedChamps[(int)Enum.Parse<Role>(but.Content.ToString())].Count() == 0)
                {
                    but.IsEnabled = false;
                }
            }

            //if all roles disabled, disable all button aswell
            if (roleButtons.TrueForAll(but => but.IsEnabled == false))
                allBut.IsEnabled = false;
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            MessageBoxResult conf = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);

            if (conf == MessageBoxResult.No)
                return;

            //update sorted role lists
            foreach (Role role in ChampController.champs[rolledChamp].Roles)
            {
                //get where the index to remove was and remove the entry
                var indexToRemove = sortedChamps[(int)role].Find(num => num == rolledChamp);
                sortedChamps[(int)role].Remove(rolledChamp);
                //decrease entries after the one removed by one
                for (int i = indexToRemove; i < sortedChamps[(int)role].Count(); i++)
                {
                    sortedChamps[(int)role][i] -= 1;
                }
            }

            DisableEmptyRoleButs();

            //remove champion
            ChampController.champs.RemoveAt(rolledChamp);
            ChampController.SaveData();

            //disable remove button
            removeBut.IsEnabled = false;

            //reset name label
            name.Content = defLabel;

            //set rolled champ to -1
            rolledChamp = -1;
        }
    }
}
