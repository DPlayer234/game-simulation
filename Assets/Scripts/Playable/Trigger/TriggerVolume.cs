namespace DPlay.Playable.Trigger
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    ///     This is a volume that triggers when a matching <seealso cref="Triggerer"/> enters
    /// </summary>
    public class TriggerVolume : MonoBehaviour
    {
        /// <summary>
        ///     The trigger tag.
        ///     It has to match with that of the <see cref="Triggerer"/>
        /// </summary>
        public string TriggerTag = string.Empty;

        /// <summary> The Event to trigger upon entering </summary>
        public UnityEvent EnterEvent;

        /// <summary> The Event to trigger upon exiting </summary>
        public UnityEvent ExitEvent;

        /// <summary>
        ///     Invokes a specific event if there is a triggerer with a matching tag.
        /// </summary>
        /// <param name="unityEvent"></param>
        public void InvokeIfMatchingTriggerer(Triggerer triggerer, UnityEvent unityEvent)
        {
            if (triggerer.TriggerTag == this.TriggerTag)
            {
                unityEvent.Invoke();
            }
        }
    }
}
