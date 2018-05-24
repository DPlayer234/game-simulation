namespace DPlay.Playable.Trigger
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    ///     This is a volume that triggers when a matching <seealso cref="Triggerer"/> enters or exits.
    /// </summary>
    public class TriggerVolume : Trigger
    {
        /// <summary>
        ///     Called by Unity when another collider enters this one.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            this.TriggerEnter(other);
        }

        /// <summary>
        ///     Called by Unity when another collider leaves this one.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            this.TriggerExit(other);
        }
    }
}
