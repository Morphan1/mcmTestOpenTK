using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommonHandlers
{
    public class ClientCVar
    {
        /// <summary>
        /// The CVar System the client will use.
        /// </summary>
        public static CVarSystem system;

        // Game CVars
        public static CVar g_noclip;

        // Text CVars
        public static CVar t_fastrender, t_sidetextfastrender, t_allowobfu, t_allowrandom, t_allowjello, t_betteremphasis, t_bettershadow;

        //Graphics/Renderer CVars
        public static CVar r_vsync, r_fov, r_screenwidth, r_screenheight, r_fullscreen, r_thirdperson,
            r_showwireframe, r_render3d, r_whitewireframe, r_crosshairscale;

        /// <summary>
        /// Prepares the CVar system, generating default CVars.
        /// </summary>
        public static void Init(Outputter output)
        {
            system = new CVarSystem(output);

            // Game CVars
            g_noclip = Register("g_noclip", "false", CVarFlag.Boolean | CVarFlag.ServerControl); // Whether the player is in 'noclip' mode.
            // Text CVars
            // TODO: IMPLEMENT BELOW CVAR
            t_fastrender = Register("t_fastrender", "false", CVarFlag.Boolean); // Whether to only render text when needed.
            // TODO: IMPLEMENT BELOW CVAR
            t_sidetextfastrender = Register("t_sidetextfastrender", "false", CVarFlag.Boolean); // Whether to only render side-text when needed.
            t_allowobfu = Register("t_allowobfu", "true", CVarFlag.Boolean); // Whether to allow '^k' (Obfuscated) text.
            t_allowrandom = Register("t_allowrandom", "true", CVarFlag.Boolean); // Whether to allow '^R' (Random) text.
            t_allowjello = Register("t_allowjello", "true", CVarFlag.Boolean); // Whether to allow '^J' (Jello) text.
            t_betteremphasis = Register("t_betteremphasis", "true", CVarFlag.Boolean); // Whether to draw HD text '^e' (Emphasis) (2 pixels out instead of 1)
            t_bettershadow = Register("t_bettershadow", "true", CVarFlag.Boolean); // Whether to draw HD text '^d' (Drop-Shadow) (2 pixels out instead of 1)
            // Graphics/Renderer CVars
            r_vsync = Register("r_vsync", "0", CVarFlag.Numeric | CVarFlag.Delayed); // What VSync mode to use. 0 = Off, 1 = On, 2 = Adaptive.
            r_fov = Register("r_fov", "45", CVarFlag.Numeric); // What field-of-vision range to use.
            r_screenwidth = Register("r_screenwidth", MainGame.ScreenWidth.ToString(), CVarFlag.Numeric | CVarFlag.Delayed); // The X-width (size) of the window on-screen.
            r_screenheight = Register("r_screenheight", MainGame.ScreenHeight.ToString(), CVarFlag.Numeric | CVarFlag.Delayed); // The Y-height (size) of the window on-screen.
            r_fullscreen = Register("r_fullscreen", "false", CVarFlag.Boolean | CVarFlag.Delayed); // Whether to make the render window occupy the entire screen.
            r_thirdperson = Register("r_thirdperson", "false", CVarFlag.Boolean); // Whether to use a third-person perspective.
            r_showwireframe = Register("r_showwireframe", "false", CVarFlag.Boolean); // Whether to show a wireframe of the 3D world.
            r_render3d = Register("r_render3d", "true", CVarFlag.Boolean); // Whether to render the 3D world - generally combined with showwireframe.
            r_whitewireframe = Register("r_whitewireframe", "true", CVarFlag.Boolean); // Whether wireframes are rendered pitched white.
            r_crosshairscale = Register("r_crosshairscale", "1", CVarFlag.Numeric); // How big the crosshair is
            // TODO: More graphics CVars
            // TODO: OpenGL info
            // TODO: Other CVars
        }

        static CVar Register(string name, string value, CVarFlag flags)
        {
            return system.Register(name, value, flags);
        }
    }
}
