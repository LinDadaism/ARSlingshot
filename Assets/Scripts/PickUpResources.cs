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
        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to place an object.
        /// </summary>
        public bool CanPlace { get; set; }

        protected override void Awake()
        {
            base.Awake();
            this.m_RaycastManager = this.GetComponent<ARRaycastManager>();
            _globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
        }

        private void Update()
        {
            if (Pointer.current == null || this.pressed == false || !this.CanPlace)
                return;

            var touchPosition = Pointer.current.position.ReadValue();

            // Ensure we are not over any UI element.
            var uiButtons = FindObjectOfType<UIButtons>();
            if (uiButtons != null && (uiButtons.IsPointOverUI(touchPosition)))
                return;

            // Raycast against layer "GroundPlane" using normal Raycasting for our artifical ground plane.
            // For AR Foundation planes (if enabled), we use AR Raycasting.
            var ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Airplane")))
            {
                //Debug.Log("using Physics raycast");

                GameObject hitObject = hit.transform.gameObject;

                int playerType = PlayerPrefs.GetInt("PlayerType");
                if (hitObject.GetComponent<Collider>().CompareTag("Airplane") && playerType == 0)
                {
                    pickUpPlane(hitObject);
                }
                else if (hitObject.GetComponent<Collider>().CompareTag("Ammo") && playerType == 1)
                {
                    pickUpAmmo(hitObject);
                }
            }
        }


        private void pickUpPlane(GameObject hitObject)
        {
            // update score
            _globalManager.noOfPlanes++;
            _globalManager.noOfPlanesUI.text = "Planes: " + _globalManager.noOfPlanes;
            // Destroy gameObject from the scene and mark it for garbage collection
            Destroy(hitObject);
        }

        private void pickUpAmmo(GameObject hitObject)
        {
            // update score
            _globalManager.noOfPellets+=10;
            _globalManager.noOfPelletsUI.text = "Ammo: " + _globalManager.noOfPellets;
            // Destroy gameObject from the scene and mark it for garbage collection
            Destroy(hitObject);
        }

        protected override void OnPress(Vector3 position) => this.pressed = true;

        protected override void OnPressCancel() => this.pressed = false;
    }
}
