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

        /// <summary> The Event to trigger </summary>
        private UnityEvent TriggerEvent;

        /// <summary>
        ///     Called by Unity when another collider enters this one.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            Triggerer triggerer = other.collider.GetComponent<Triggerer>();

            if (triggerer != null && triggerer.TriggerTag == this.TriggerTag)
            {
                this.TriggerEvent.Invoke();
            }
        }
    }
}
