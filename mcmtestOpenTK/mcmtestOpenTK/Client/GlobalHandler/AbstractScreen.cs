using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public abstract class AbstractScreen
    {
        public readonly ScreenMode Mode;

        public bool Initted = false;

        public AbstractScreen(ScreenMode mode)
        {
            Mode = mode;
        }

        public abstract void Init();

        public abstract void Tick();

        public abstract void Draw2D();

        public abstract void Draw3D();
    }

    public enum ScreenMode
    {
        Logos = 1,
        MainMenu = 2,
        // Menus
        Loading = 3,
        Game = 4,
        // In-game options
        MAX = 5,
    }
}
