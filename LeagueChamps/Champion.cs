namespace LeagueChamps
{
    public enum Role
    {
        Sup,
        Bot,
        Mid,
        Jgl,
        Top
    }

    public class OldChampion
    {
        public string Name { get; set; }
        public Role[] Roles { get; set; } 
    }

    public class Champion
    {
        private string name = "";
        private bool[] roles = new bool[5];
        private bool enabled = true;

        public bool Enabled { get; set; }
        public string Name { get; set; }
        public bool[] Roles { get; set; }
    }
}
