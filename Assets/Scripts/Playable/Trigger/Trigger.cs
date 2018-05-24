namespace DPlay.Playable.Trigger
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    ///     This is the base class for anything that interacts with <seealso cref="Triggerer"/>.
    /// </summary>
    public abstract class Trigger : MonoBehaviour
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

        /// <summary>
        ///     Called to trigger the enter event based on a collider.
        /// </summary>
        /// <param name="other">Any component of the other GameObject</param>
        protected void TriggerEnter(Component other)
        {
            foreach (Triggerer triggerer in other.GetComponents<Triggerer>())
            {
                this.InvokeIfMatchingTriggerer(triggerer, this.EnterEvent);
            }
        }

        /// <summary>
        ///     Called to trigger the exit event based on a collider.
        /// </summary>
        /// <param name="other">Any component of the other GameObject</param>
        protected void TriggerExit(Component other)
        {
            foreach (Triggerer triggerer in other.GetComponents<Triggerer>())
            {
                this.InvokeIfMatchingTriggerer(triggerer, this.ExitEvent);
            }
        }
    }
}
