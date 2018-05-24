namespace DPlay.Playable.Interactable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     This is used by doors.
    /// </summary>
    [DisallowMultipleComponent]
    public class SlideDoor : MonoBehaviour, IInteractable
    {
        /// <summary> The minimum offset for a closed door </summary>
        [SerializeField]
        private float minimumOffset = 0.0f;

        /// <summary> The maximum offset for an opened door </summary>
        [SerializeField]
        private float maximumOffset = 0.9f;

        /// <summary> The speed in units per second that the door will move </summary>
        [SerializeField]
        private float movementSpeed = 2.0f;

        /// <summary> Whether the slide door can be interacted with </summary>
        [SerializeField]
        private bool interactable = true;

        /// <summary> The coroutine handling the door movement </summary>
        private Coroutine movementTask;
        
        /// <summary> The original local position </summary>
        private Vector3 basePosition;

        /// <summary> Indicates, whether the door was opened </summary>
        public bool IsOpened { get; private set; }

        /// <summary>
        ///     From <seealso cref="IInteractable"/>.
        ///     Returns whether the door is explicitly interactable with.
        /// </summary>
        public bool Interactable
        {
            get
            {
                return this.interactable;
            }
        }

        /// <summary>
        ///     Called to interact with the door.
        /// </summary>
        [ContextMenu("Interact")]
        public void Interact()
        {
            if (!this.Interactable) return;

            if (this.IsOpened)
            {
                this.Close();
            }
            else
            {
                this.Open();
            }
        }

        /// <summary>
        ///     Opens the door
        /// </summary>
        [ContextMenu("Open")]
        public void Open()
        {
            this.CancelMovementTask();
            this.movementTask = this.StartCoroutine(this.OpenTask());
        }

        /// <summary>
        ///     Closes the door
        /// </summary>
        [ContextMenu("Close")]
        public void Close()
        {
            this.CancelMovementTask();
            this.movementTask = this.StartCoroutine(this.CloseTask());
        }

        /// <summary>
        ///     Coroutine for opening the door.
        /// </summary>
        /// <returns>An enumerator</returns>
        private IEnumerator OpenTask()
        {
            this.IsOpened = true;

            while (this.transform.localPosition.z < this.maximumOffset)
            {
                this.transform.localPosition = new Vector3(
                    this.basePosition.x,
                    this.basePosition.y,
                    this.basePosition.z + Mathf.Min(this.maximumOffset, this.transform.localPosition.z + Time.fixedDeltaTime * this.movementSpeed));

                yield return new WaitForFixedUpdate();
            }
        }

        /// <summary>
        ///     Coroutine for closing the door.
        /// </summary>
        /// <returns>An enumator</returns>
        private IEnumerator CloseTask()
        {
            this.IsOpened = false;

            while (this.transform.localPosition.z > this.minimumOffset)
            {
                this.transform.localPosition = new Vector3(
                    this.basePosition.x,
                    this.basePosition.y,
                    this.basePosition.z + Mathf.Max(this.minimumOffset, this.transform.localPosition.z - Time.fixedDeltaTime * this.movementSpeed));

                yield return new WaitForFixedUpdate();
            }
        }

        /// <summary>
        ///     Cancels the running movement coroutine.
        /// </summary>
        private void CancelMovementTask()
        {
            if (this.movementTask != null)
            {
                this.StopCoroutine(this.movementTask);
            }
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="Door"/> class.
        /// </summary>
        private void Awake()
        {
            this.basePosition = this.transform.localPosition;

            this.IsOpened = false;
        }
    }
}
