namespace DPlay.Playable.Trigger
{
    using UnityEngine;

    /// <summary>
    ///     This will trigger matching <see cref="Trigger"/>s
    /// </summary>
    public class Triggerer : MonoBehaviour
    {
        /// <summary>
        ///     The trigger tag.
        ///     It has to match with that of the <see cref="Trigger"/>
        /// </summary>
        public string TriggerTag = string.Empty;
    }
}
