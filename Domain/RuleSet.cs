using System.Collections.Generic;

namespace Domain
{
    public static class RuleSet
    {
        public static int RuleSetId { get; set; }
        
        
        public static bool CanTouch { get; set; } = true;

        //public Player Turn { get; set; } = null;
        
        public static int CarrierAmount { get; set; }
        public static int BattleshipAmount{ get; set; }
        public static int SubmarineAmount { get; set; }
        public static int CruiserAmount { get; set; }
        public static int PatrolAmount { get; set; }

        public static int BoardWidth { get; set; } = 10;
        public static int BoardHeight { get; set; } = 10;
        
        
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