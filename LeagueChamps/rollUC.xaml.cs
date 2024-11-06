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
            disableBut.IsEnabled = false; //disable remove button
            rolledChamp = -1; //set rolled champ to -1

            //sort champion indexes into lists for each role
            sortedChamps = new List<int>[5];
            for (int i = 0; i < 5; i++)
                sortedChamps[i] = new List<int>();

            for (int i = 0; i < ChampController.champs.Count; i++)
            {
                if (!ChampController.champs[i].Enabled)
                    continue;
                for (int x = 0; x < ChampController.champs[i].Roles.Length; x++)
                {
                    if (ChampController.champs[i].Roles[x])
                        sortedChamps[x].Add(i);
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
            disableBut.IsEnabled = true;

            //cast sender object as button
            Button pressed = (Button)sender;

            //if pressed button is in role buttons role that lane
            if (roleButtons.Contains(pressed))
            {
                int role = roleButtons.IndexOf(pressed);
                var excludedList = sortedChamps[role].FindAll(index => index != rolledChamp);
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

                exludedChamps = exludedChamps.FindAll(champ => champ.Enabled);

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
                if (sortedChamps[roleButtons.IndexOf(but)].Count() == 0)
                {
                    but.IsEnabled = false;
                }
            }

            //if all roles disabled, disable all button aswell
            if (roleButtons.TrueForAll(but => but.IsEnabled == false))
                allBut.IsEnabled = false;
        }

        private void Disable(object sender, RoutedEventArgs e)
        {
            MessageBoxResult conf = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);

            if (conf == MessageBoxResult.No)
                return;

            //update sorted role lists
            for (int i = 0; i < sortedChamps.Length; i++)
            {
                if (!ChampController.champs[rolledChamp].Roles[i])
                    continue;

                //get where the index to remove was and remove the entry
                var indexToRemove = sortedChamps[i].Find(num => num == rolledChamp);
                sortedChamps[i].Remove(rolledChamp);

                //decrease entries after the one removed by one
                for (int x = indexToRemove; x < sortedChamps[i].Count(); x++)
                {
                    sortedChamps[i][x] -= 1;
                }
            }

            rollImg.Source = null;

            DisableEmptyRoleButs();

            //remove champion
            ChampController.champs[rolledChamp].Enabled = false;
            ChampController.SaveData();

            //disable remove button
            disableBut.IsEnabled = false;

            //reset name label
            name.Content = defLabel;

            //set rolled champ to -1
            rolledChamp = -1;
        }
    }
}
