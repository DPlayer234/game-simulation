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
    public class Door : MonoBehaviour, IInteractable
    {
        /// <summary> The minimum angle for an opened door </summary>
        private const float MinimumOpenAngle = 270.0f;

        /// <summary> The maximum angle for a closed door </summary>
        private const float MaximumCloseAngle = 359.9f;

        /// <summary> The transform of the door hinge </summary>
        [SerializeField]
        private Transform hinge;

        /// <summary> The speed in angles per second that the door will move </summary>
        [SerializeField]
        [Range(0.0f, 360.0f)]
        private float movementSpeed = 90f;

        /// <summary> The coroutine handling the door movement </summary>
        private Coroutine movementTask;

        /// <summary> Indicates, whether the door was opened </summary>
        public bool IsOpened { get; private set; }

        /// <summary>
        ///     Called to interact with the door.
        /// </summary>
        [ContextMenu("Interact")]
        public void Interact()
        {
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

            while (this.hinge.localEulerAngles.y > Door.MinimumOpenAngle)
            {
                this.hinge.localEulerAngles = new Vector3(
                    0.0f,
                    Mathf.Max(Door.MinimumOpenAngle, this.hinge.localEulerAngles.y - Time.fixedDeltaTime * this.movementSpeed),
                    0.0f);

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

            while (this.hinge.localEulerAngles.y < Door.MaximumCloseAngle)
            {
                this.hinge.localEulerAngles = new Vector3(
                    0.0f,
                    Mathf.Min(Door.MaximumCloseAngle, this.hinge.localEulerAngles.y + Time.fixedDeltaTime * this.movementSpeed),
                    0.0f);

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
            if (this.hinge == null)
            {
                Debug.LogWarning("There was no hinge set, using own Transform.");
                this.hinge = this.transform;
            }

            this.hinge.localEulerAngles = new Vector3(0.0f, Door.MaximumCloseAngle, 0.0f);

            this.IsOpened = false;
        }
    }
}
