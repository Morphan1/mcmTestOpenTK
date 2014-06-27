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

        public abstract void SwitchTo();
    }

    public enum ScreenMode
    {
        Logos = 1,
        Login = 2,
        MainMenu = 3,
        Servers = 4,
        // Menus
        Loading = 5,
        Game = 6,
        // In-game options
        MAX = 7,
    }
}
