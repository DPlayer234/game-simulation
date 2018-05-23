namespace DPlay.Playable.Interactor
{
    using DPlay.Playable.Interactable;
    using UnityEngine;

    /// <summary>
    ///     This allows things to interact with <seealso cref="IInteractable"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Interactor : MonoBehaviour
    {
        /// <summary> The transform used as a referrence when raycasting </summary>
        [SerializeField]
        protected Transform viewport;

        /// <summary> The range that something has to be within to be interactable. </summary>
        [SerializeField]
        protected float interactionRange = 1f;

        /// <summary>
        ///     Called by Unity to initialize the <see cref="Interactor"/>
        /// </summary>
        protected virtual void Awake()
        {
            if (this.viewport == null)
            {
                Debug.LogWarning("There was no viewport set, using own Transform.");
                this.viewport = this.transform;
            }
        }

        /// <summary>
        ///     Tries to interact with whatever is being pointed at.
        /// </summary>
        protected void TryInteract()
        {
            // Cast a ray
            RaycastHit raycastHit;

            if (Physics.Raycast(this.viewport.position, this.viewport.forward, out raycastHit, this.interactionRange))
            {
                IInteractable interactable = raycastHit.collider.GetComponent<IInteractable>();

                // If hit and interactable, interact.
                if (interactable != null)
                {
                    this.InteractWith(interactable);
                }
            }
        }

        /// <summary>
        ///     Interacts with the given interactable.
        /// </summary>
        /// <param name="interactable">The interactable to interact with</param>
        protected void InteractWith(IInteractable interactable)
        {
            interactable.Interact();
        }
    }
}
