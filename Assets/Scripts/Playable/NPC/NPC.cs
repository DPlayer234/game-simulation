namespace DPlay.Playable.NPC
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     This script is used to make an NPC.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class NPC : MonoBehaviour
    {
        /// <summary> Distance in units which is considered close </summary>
        private const float CloseDistance = 0.5f;

        /// <summary> Used in the calculation for rotation </summary>
        private const float RotationSpeedBase = 0.01f;

        /// <summary> Used in the calculation for velocity </summary>
        private const float MovementSpeedBase = 0.2f;

        /// <summary> The root transform containing all random waypoints </summary>
        [SerializeField]
        private Transform waypointRoot;

        /// <summary> The NPCs view distance for following the player </summary>
        [SerializeField]
        private float viewDistance = 4f;

        /// <summary> The speed in units per second that the NPC can move </summary>
        [SerializeField]
        private float movementSpeed = 3f;

        /// <summary> The waypoint to walk towards </summary>
        private Transform targetWaypoint;

        /// <summary> The player this NPC is trying to follow </summary>
        private Transform followingPlayer;

        /// <summary> This NPC's <seealso cref="Rigidbody"/> </summary>
        new private Rigidbody rigidbody;

        /// <summary>
        ///     Returns the actual target transform.
        ///     May be null.
        /// </summary>
        private Transform Target
        {
            get
            {
                return this.followingPlayer ?? this.targetWaypoint;
            }
        }

        /// <summary>
        ///     Returns the normalized direction towards the target.
        ///     Will throw a <seealso cref="System.NullReferenceException"/> if <seealso cref="Target"/> is null.
        /// </summary>
        private Vector3 TargetDirection
        {
            get
            {
                return (this.Target.position - this.transform.position).normalized;
            }
        }

        /// <summary>
        ///     Chooses a random new waypoint
        /// </summary>
        public void ChooseNewWaypoint()
        {
            if (this.waypointRoot == null) return;
            if (this.waypointRoot.childCount < 1) return;

            this.targetWaypoint = this.waypointRoot.GetChild(Random.Range(0, this.waypointRoot.childCount));
        }

        // These next two methods are not a property because the Setter needs to be selectable in the editor
        // for a UnityEvent.

        /// <summary>
        ///     Gets the waypoint root
        /// </summary>
        /// <returns>The waypoint root</returns>
        public Transform GetWaypointRoot()
        {
            return this.waypointRoot;
        }

        /// <summary>
        ///     Sets the waypoint root.
        /// </summary>
        /// <param name="root">The new waypoint root</param>
        public void SetWaypointRoot(Transform root)
        {
            this.waypointRoot = root;
            this.ChooseNewWaypoint();
        }

        /// <summary>
        ///     Sets the player's transform to follow.
        /// </summary>
        /// <param name="player"></param>
        public void FollowPlayer(Transform player)
        {
            this.followingPlayer = player;
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="NPC"/> class.
        /// </summary>
        private void Awake()
        {
            if (!this.FindComponent(out this.rigidbody))
                throw new NPCException("There is no Rigidbody attached to the NPC.");

            this.UpdateTarget();
        }

        /// <summary>
        ///     Called by Unity every fixed update time.
        /// </summary>
        private void FixedUpdate()
        {
            this.UpdateTarget();
            this.UpdateRotation();
            this.UpdateVelocity();
        }

        /// <summary>
        ///     Updates the follow target
        /// </summary>
        private void UpdateTarget()
        {
            // Look for a waypoint
            if (this.targetWaypoint == null || this.IsCloseToTransform(this.targetWaypoint))
            {
                this.ChooseNewWaypoint();
            }

            // Look for a player
            this.FollowPlayer(this.GetVisiblePlayer());
        }

        /// <summary>
        ///     Updates the rotation to slowly rotate to face the <seealso cref="Target"/>
        /// </summary>
        private void UpdateRotation()
        {
            if (this.Target == null) return;

            Vector3 forward = Vector3.Lerp(
                this.TargetDirection,
                this.transform.forward,
                Mathf.Pow(NPC.RotationSpeedBase, Time.fixedDeltaTime));

            // Should only rotate around the Y axis
            forward.y = 0.0f;
            forward.Normalize();
            this.transform.forward = forward;
        }

        /// <summary>
        ///     Updates the velocity to approach the <seealso cref="Target"/>
        /// </summary>
        private void UpdateVelocity()
        {
            if (this.Target == null) return;
            
            Vector3 velocity = this.rigidbody.velocity;
            Vector3 targetVelocity = this.TargetDirection * this.movementSpeed;
            targetVelocity.y = velocity.y;

            velocity = Vector3.Lerp(
                targetVelocity,
                velocity,
                Mathf.Pow(NPC.MovementSpeedBase, Time.fixedDeltaTime));

            this.rigidbody.velocity = velocity;
        }

        /// <summary>
        ///     Returns the transform of a visible player
        /// </summary>
        /// <returns></returns>
        private Transform GetVisiblePlayer()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            RaycastHit raycastHit;

            foreach (GameObject player in players)
            {
                if (this.IsCloseToTransform(player.transform, this.viewDistance)
                    && Physics.Raycast(this.transform.position, player.transform.position - this.transform.position, out raycastHit, this.viewDistance)
                    && raycastHit.collider.gameObject == player)
                {
                    return player.transform;
                }
            }

            return null;
        }

        /// <summary>
        ///     Returns whether the NPC is close to a given transform.
        /// </summary>
        /// <param name="transform">The transform to check against</param>
        /// <param name="closeDistance">The distance which is considered close. Defaults to <seealso cref="CloseDistance"/></param>
        /// <returns>Whether the NPC is close by</returns>
        private bool IsCloseToTransform(Transform transform, float closeDistance = NPC.CloseDistance)
        {
            // Only top-down distance
            float distance = Vector2.Distance(
                new Vector2(this.transform.position.x, this.transform.position.z),
                new Vector2(transform.position.x, transform.position.z));

            return distance < closeDistance;
        }
    }
}