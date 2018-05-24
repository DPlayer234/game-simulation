namespace DPlay.Playable.Trigger
{
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     This will trigger if any matching <seealso cref="Triggerer"/>s step ontop of it.
    /// </summary>
    public class TriggerPlate : Trigger
    {
        /// <summary>
        ///     The minimum Y-component of a contact point to be considered on top.
        /// </summary>
        private const float NormalOnTopYMinimum = 0.25f;

        /// <summary>
        ///     Called by Unity when a collision starts.
        /// </summary>
        /// <param name="collision">The collision</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (this.IsOnTop(collision))
            {
                this.TriggerEnter(collision.collider);
            }
        }

        // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
        // Doesn't work?
        // ... This is not important for this assignment.
        // I will look into it later.

        /// <summary>
        ///     Called by Unity when a collision ends.
        /// </summary>
        /// <param name="collision">The collision</param>
        private void OnCollisionExit(Collision collision)
        {
            if (this.IsOnTop(collision))
            {
                this.TriggerExit(collision.collider);
            }
        }

        /// <summary>
        ///     Returns whether the collision happened on top of this GameObject.
        /// </summary>
        /// <param name="collision">The collision to check</param>
        /// <returns>Whether the collision happened on top</returns>
        private bool IsOnTop(Collision collision)
        {
            foreach (ContactPoint point in collision.contacts)
            {
                if (point.normal.y < -TriggerPlate.NormalOnTopYMinimum)
                {
                    return true;
                }
            }

            return false;
        }
    }
}