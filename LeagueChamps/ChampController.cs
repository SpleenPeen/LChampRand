using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using System.IO;

namespace LeagueChamps
{
    public static class ChampController
    {
        public static List<Champion> champs = new List<Champion>();
        public static string filePath = Directory.GetCurrentDirectory() + @"\champs.json";

        public static void LoadChamps()
        {
            List<Champion> loaded;
            try
            {
                loaded = JsonSerializer.Deserialize<List<Champion>>(File.ReadAllText(filePath));
            }
            catch
            {
                return;
            }

            champs = loaded;
            if(SortChamps())
                SaveData();
        }

        public static void SaveData()
        {
            string json = JsonSerializer.Serialize(champs);
            File.WriteAllText(filePath, json);
        }

        public static bool SortChamps() 
        {
            bool neededSorting = false;
            for (int i = 0; i < champs.Count-1; i++)
            {
                for (int x = i+1; x < champs.Count; x++)
                {
                    if (string.Compare(champs[i].Name, champs[x].Name) > 0)
                    {
                        var temp = champs[i];
                        champs[i] = champs[x];
                        champs[x] = temp;
                        neededSorting = true;
                    }
                }
            }
            return neededSorting;
        }

        public static bool AddChampion(string name, bool[] roles)
        {
            try
            {
                //throw an error if empty
                if (name == "" || name == null)
                    throw new Exception("Name can't be left empty");

                //throw an error if trying to add an existing champion
                foreach (Champion champ in champs)
                {
                    if (champ.Name == name)
                    {
                        throw new Exception("This champion already exists");
                    }
                }

                //throw an error if no roles were given
                if (roles.Length != 5)
                {
                    throw new Exception("Must have 5 roles");
                }

                //add champion to list and save updated list to file
                champs.Add(new Champion { Enabled = true, Name = name, Roles = roles });
                SortChamps();
                SaveData();
                //MessageBox.Show("Added successfully");
                return true;
            }
            catch (Exception ex)
            {
                //display error message
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
