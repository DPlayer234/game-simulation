namespace DPlay.Playable.NPC
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     Any exception caused by an NPC
    /// </summary>
    public class NPCException : System.Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NPCException"/> class.
        /// </summary>
        /// <param name="message">The exception message</param>
        public NPCException(string message) : base(message) { }
    }
}