﻿namespace ARSlingshot
{
    using UnityEngine;
    using Photon.Pun;
    using UnityEngine.UIElements;

    /// <summary>
    /// Launches projectiles from a touch point with the specified <see cref="initialSpeed"/>.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ProjectileLauncher : PressInputBase
    {
        [SerializeField]
        private Rigidbody projectilePrefab;

        [SerializeField]
        private float initialSpeed = 25;

        private bool planeMode;

        private bool _joinedRoom = false;

        private GlobalManager _globalManager;

        protected override void Awake()
        {
            base.Awake();
            _globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
        }

        private void Start()
        {
            this.enabled = (PlayerPrefs.GetInt("PlayerType") == 0) ? true : false;
        }

        protected override void OnPressBegan(Vector3 position)
        {
            //if (this.projectilePrefab == null || !NetworkLauncher.Singleton.HasJoinedRoom)
            if (this.projectilePrefab == null || !FindObjectOfType<SharedSpaceManager>().HasFoundOrigin || _globalManager.noOfPlanes < 1)
                return;

            // Ensure user is not doing anything else.
            var uiButtons = FindObjectOfType<UIButtons>();
            if (uiButtons != null && (uiButtons.IsPointOverUI(position) || !uiButtons.IsIdle))
                return;

            // We send our current player number as data so that the projectile can pick its material based on the player that owns it.
            var initialData = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };

            // Cast a ray from the touch point to the world. We use the camera position as the origin and the ray direction as the
            // velocity direction.
            var ray = this.GetComponent<Camera>().ScreenPointToRay(position);
            Quaternion camRot = this.GetComponent<Camera>().transform.rotation;
            var projectile = PhotonNetwork.Instantiate(this.projectilePrefab.name, ray.origin, camRot, data: initialData);

            UpdatePlaneCount();

            // By default, the projectile is kinematic in the prefab. This is because it should not be affected by physics
            // on clients other than the one owning it. Hence we disable kinematic mode and let the physics engine take over here.
            // It might make sense to have all game physics run on the server for a more complex scenario. You could transfer
            // ownership here to the server.
            var rigidbody = projectile.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            initialSpeed = 8;
            rigidbody.velocity = ray.direction * initialSpeed;
        }

        private void UpdatePlaneCount()
        {
            _globalManager.noOfPlanes -= 1;
            _globalManager.noOfPlanesUI.text = "Planes : " + _globalManager.noOfPlanes;
            GameObject.Find("GlobalManager").GetPhotonView().RPC("UpdatePlaneCount", RpcTarget.OthersBuffered, false);
        }
    }
}
