using System;
using System.Collections.Generic;

namespace MenuSystem
{
    public class MenuItem
    {
        public string LongDescription { get; set; }

        public HashSet<string> Shortcuts{ get; set; } = new HashSet<string>();
        
        //parameter is shortcut chosen
        public Func<string, string> CommandToExecute { get; set;  }  

        public bool IsDefaultChoice { get; set; } = false;
        
        public MenuItemType MenuItemType { get; set; } = MenuItemType.Regular;

        public override string ToString()
        {
            return ") " + LongDescription;
        }

    }
}