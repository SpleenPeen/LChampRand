using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class Champion
    {
        public string Name { get; set; }
        public Role[] Roles { get; set; } 
    }
}
