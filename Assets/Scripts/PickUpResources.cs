namespace ARSlingshot
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;
    using Photon.Pun;

    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    public class PickUpResources : PressInputBase
    {
        private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        private ARRaycastManager m_RaycastManager;
        private bool pressed;

        private GlobalManager _globalManager;
        private UIButtons _uiButtons;
        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to place an object.
        /// </summary>
        public bool CanPlace { get; set; }

        protected override void Awake()
        {
            base.Awake();
            this.m_RaycastManager = this.GetComponent<ARRaycastManager>();
            _globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
            _uiButtons = GameObject.Find("UIManager").GetComponent<UIButtons>();
        }

        private void Update()
        {
            if (Pointer.current == null || this.pressed == false || !this.CanPlace)
                return;

            var touchPosition = Pointer.current.position.ReadValue();

            // Ensure we are not over any UI element.
            _uiButtons = FindObjectOfType<UIButtons>();
            if (_uiButtons != null && (_uiButtons.IsPointOverUI(touchPosition)))
                return;


            // Raycast against layer "GroundPlane" using normal Raycasting for our artifical ground plane.
            // For AR Foundation planes (if enabled), we use AR Raycasting.
            var ray = Camera.main.ScreenPointToRay(touchPosition);
            int playerType = PlayerPrefs.GetInt("PlayerType");
            GameObject hitObject;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Airplane")) && playerType == 0)
            {
                hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<Collider>().CompareTag("Airplane"))
                {
                    pickUpPlane(hitObject);
                }
            }
            else if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Ammo")) && playerType == 1)
            {
                hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<Collider>().CompareTag("Ammo"))
                {
                    pickUpAmmo(hitObject);
                }
            }
        }


        private void pickUpPlane(GameObject hitObject)
        {
            // update score
            if (_globalManager.noOfPlanes >= 5 || hitObject.transform.GetChild(0).gameObject.activeSelf)
                return;
            
            _globalManager.noOfPlanes++;
            _globalManager.noOfPlanesUI.text = "Planes: " + _globalManager.noOfPlanes;
            GameObject.Find("GlobalManager").GetPhotonView().RPC("UpdatePlaneCount", RpcTarget.OthersBuffered, true);

            // Destroy gameObject from the scene and mark it for garbage collection
            PhotonNetwork.Destroy(hitObject);
            
        }

            private void pickUpAmmo(GameObject hitObject)
        {
            // update score
            _uiButtons.debugTextUI.text = "Updating score";
            _globalManager.noOfPellets+=10;
            _globalManager.noOfPelletsUI.text = "Ammo: " + _globalManager.noOfPellets;
            GameObject.Find("GlobalManager").GetPhotonView().RPC("UpdateAmmoCount", RpcTarget.OthersBuffered);

            _globalManager.isAmmoSpawned = false;
            PhotonNetwork.Destroy(_globalManager.spawnedAmmoGameObject);
            // Destroy gameObject from the scene and mark it for garbage collection
            PhotonNetwork.Destroy(hitObject);
        }

        protected override void OnPress(Vector3 position) => this.pressed = true;

        protected override void OnPressCancel() => this.pressed = false;
    }
}
