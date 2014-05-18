﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared.CommandSystem
{
    public abstract class Outputter
    {
        /// <summary>
        /// Writes a line of text to the console.
        /// </summary>
        /// <param name="text">The line of text</param>
        public abstract void WriteLine(string text);

        /// <summary>
        /// Writes text to the console.
        /// </summary>
        /// <param name="text">The text to write</param>
        public abstract void Write(string text);

        /// <summary>
        /// Used to output a failure message.
        /// </summary>
        /// <param name="tagged_text">The text to output, with tags included</param>
        public abstract void Bad(string tagged_text);

        /// <summary>
        /// Used to output a success message.
        /// </summary>
        /// <param name="tagged_text">The text to output, with tags included</param>
        public abstract void Good(string tagged_text);

        /// <summary>
        /// Used to properly handle an unknown command.
        /// </summary>
        /// <param name="basecommand">The command that wasn't recognized</param>
        /// <param name="arguments">The commands arguments</param>
        public abstract void UnknownCommand(string basecommand, string[] arguments);

        /// <summary>
        /// The CVar System used by this command engine.
        /// </summary>
        public CVarSystem CVarSys;

        /// <summary>
        /// Whether the game is still setting up currently.
        /// </summary>
        public bool Initializing = false;
    }
}
