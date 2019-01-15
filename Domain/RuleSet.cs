using System.Collections.Generic;

namespace Domain
{
    public class RuleSet
    {
        public int RuleSetId { get; set; }
        
        //public int GameId { get; set; }
        //public Game Game { get; set; }
        
        public static bool CanTouch { get; set; } = true;

        //public Player Turn { get; set; } = null;
        
        public static void SetTouchTrue()
        {
            CanTouch = true;
        }
        public static void SetTouchFalse()
        {
            CanTouch = false;
        }

    }
}