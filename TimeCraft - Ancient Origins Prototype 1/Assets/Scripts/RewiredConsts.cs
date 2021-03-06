// <auto-generated>
// Rewired Constants
// This list was generated on 5/8/2018 3:24:45 PM
// The list applies to only the Rewired Input Manager from which it was generated.
// If you use a different Rewired Input Manager, you will have to generate a new list.
// If you make changes to the exported items in the Rewired Input Manager, you will
// need to regenerate this list.
// </auto-generated>

namespace RewiredConsts {
    public static partial class Action {
        // Default
        // Movement
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Handles player movement", friendlyName = "Moves Horizontaly")]
        public const int Horizontal = 1;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Handles player movement", friendlyName = "Moves Vertically")]
        public const int Vertical = 2;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Handles player movement", friendlyName = "Turns horizontally")]
        public const int HorizontalTurn = 5;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Handles player movement", friendlyName = "Turns Vertically")]
        public const int VerticalTurn = 7;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Handles player movement", friendlyName = "Sprint")]
        public const int Sprint = 8;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Handles player movement", friendlyName = "Walk")]
        public const int Walk = 9;
        // Combat
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Handles combat actions", friendlyName = "Fire1")]
        public const int Fire1 = 4;
    }
    public static partial class Category {
        public const int Default = 0;
    }
    public static partial class Layout {
        public static partial class Joystick {
            public const int Default = 0;
        }
        public static partial class Keyboard {
            public const int Default = 0;
        }
        public static partial class Mouse {
            public const int Default = 0;
        }
        public static partial class CustomController {
            public const int Default = 0;
        }
    }
    public static partial class Player {
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "System")]
        public const int System = 9999999;
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "Player0")]
        public const int Player0 = 0;
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "Player0")]
        public const int Player1 = 1;
    }
    public static partial class CustomController {
    }
}
