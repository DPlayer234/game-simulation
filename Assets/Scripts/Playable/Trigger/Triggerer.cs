namespace DPlay.Playable.Trigger
{
    using UnityEngine;

    /// <summary>
    ///     This will trigger matching <see cref="TriggerVolume"/>s
    /// </summary>
    public class Triggerer : MonoBehaviour
    {
        /// <summary>
        ///     The trigger tag.
        ///     It has to match with that of the <see cref="TriggerVolume"/>
        /// </summary>
        public string TriggerTag = string.Empty;

        /// <summary>
        ///     Called by Unity when another collider enters this one.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            TriggerVolume trigger = other.GetComponent<TriggerVolume>();

            trigger.InvokeIfMatchingTriggerer(this, trigger.EnterEvent);
        }

        /// <summary>
        ///     Called by Unity when another collider leaves this one.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            TriggerVolume trigger = other.GetComponent<TriggerVolume>();

            trigger.InvokeIfMatchingTriggerer(this, trigger.ExitEvent);
        }
    }
}
