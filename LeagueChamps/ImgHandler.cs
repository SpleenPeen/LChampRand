using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace LeagueChamps
{
    internal static class ImgHandler
    {
        private static string filePath = $@"{Directory.GetCurrentDirectory()}\imgs\";

        public static string FilePath { get { return filePath; } }

        public static void SetImage(Image img, Champion champ)
        {
            if (!File.Exists(@$"imgs\{champ.Name}.png"))
                return;
            img.Source = BitmapFrame.Create(new Uri($@"{filePath}{champ.Name}.png", UriKind.Absolute), BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
        }

        public static void CopyImage(string srcFilepath, string champName)
        {
            File.Copy(srcFilepath, filePath + champName + ".png");
        }

        public static void RenameImage(string name, string newName, Image img)
        {
            if (!File.Exists(filePath + name + ".png"))
                return;
            File.Copy(filePath + name + ".png", filePath + newName + ".png");
            img.Source = BitmapFrame.Create(new Uri($@"{filePath}{newName}.png", UriKind.Absolute), BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
            File.Delete(filePath + name + ".png");
        }

        public static void RemoveImage(string name)
        {
            if (!File.Exists(filePath + name + ".png"))
                return;
            File.Delete(filePath + name + ".png");
        }
    }
}
