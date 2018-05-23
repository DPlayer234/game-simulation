namespace DPlay.Playable.Interactor
{
    using UnityEngine;

    /// <summary>
    ///     This allows the player to interact with things.
    /// </summary>
    public class PlayerInteractor : Interactor
    {
        /// <summary>
        ///     Called once every frame by Unity
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                this.TryInteract();
            }
        }
    }
}
