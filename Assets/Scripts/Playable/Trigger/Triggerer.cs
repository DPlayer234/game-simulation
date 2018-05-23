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
    }
}
