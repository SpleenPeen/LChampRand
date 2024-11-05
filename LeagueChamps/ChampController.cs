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

        public static void AddChampion(string name, Role[] roles)
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
                if (roles.Length == 0)
                {
                    throw new Exception("Must have at least one role");
                }

                //make sure all role entries are unique
                if (roles.Length > 1)
                {
                    List<Role> uniqRoles = new List<Role>();

                    foreach (Role role in roles)
                    {
                        //if already contains all roles break out of loop
                        if (uniqRoles.Count == 5)
                            break;

                        //loop through unique roles and add role if not present
                        bool add = true;
                        foreach (Role uniqRole in uniqRoles)
                        {
                            if (uniqRole == role)
                            {
                                add = false;
                                break;
                            }
                        }
                        if (add)
                        {
                            uniqRoles.Add(role);
                        }
                    }

                    //replace roles with unique roles
                    roles = uniqRoles.ToArray();
                }

                //add champion to list and save updated list to file
                champs.Add(new Champion { Name = name, Roles = roles });
                SortChamps();
                SaveData();
                MessageBox.Show("Added successfully");
                
            }
            catch (Exception ex)
            {
                //display error message
                MessageBox.Show(ex.Message);
            }
        }
    }
}
