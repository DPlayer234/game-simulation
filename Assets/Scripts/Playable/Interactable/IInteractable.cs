namespace DPlay.Playable.Interactable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     An interface for any <seealso cref="GameObject"/> to be interacted with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        ///     Gets whether the <seealso cref="IInteractable"/> can currently be interacted with.
        /// </summary>
        bool Interactable { get; }

        /// <summary>
        ///     To be called when interacted with
        /// </summary>
        void Interact();
    }
}
