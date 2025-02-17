namespace ARSlingshot
{
    using Photon.Pun;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;
    using ElRaccoone.Tweens;
    using ElRaccoone.Tweens.Core;

    public class Slingshot : PressInputBase
    {

        private Camera _cam;

        private float _slingshotOnscreenYPos = 0;
        public float yDistToOffscreen = 0.35f;

        private float _slingshotOnscreenXRot = 0;
        public float xOffscreenRot = 30f;

        private Vector2? _touchDownPos = null;

        private float _pullbackPercent = 0;
        public float pullbackPercent { get { return _pullbackPercent; } }

        private bool _isShooting = false;
        public bool isShooting { get { return _isShooting; } }

        private Vector3 _pelletLocalOrigin;
        public Vector3 pelletLocalOrigin { get { return _pelletLocalOrigin; } }

        public Transform pelletTransform;
        public PelletShot pelletShot;

        public Transform shakeTransform;
        public SkinnedMeshRenderer slingshotRenderer;
        public SpriteRenderer reticleRenderer;

        private Vector3 _originalSlingshotRendLocalPos;
        private float _slingshotLocalYOffset;
        public float slingshotLocalYOffsetMultiplier = -0.13f;

        private Tween<float> _leftRightTween = null;
        private float _leftRightPercentOffset = 0;

        public float distAheadOffset = 0.5f;

        private GlobalManager _globalManager;

        /// <summary>
        /// Unity Awake() Function, called before Start()
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _cam = GetComponentInParent<Camera>();
            _globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();

            _pelletLocalOrigin = pelletTransform.localPosition;
            _slingshotOnscreenYPos = this.transform.localPosition.y;
            _slingshotOnscreenXRot = this.transform.eulerAngles.x;

            _originalSlingshotRendLocalPos = slingshotRenderer.transform.localPosition;

            slingshotRenderer.enabled = false;
            reticleRenderer.enabled = false;
            pelletTransform.gameObject.SetActive(false);
        }

        /// <summary>
        /// Unity Start() Function, called before the first frame update
        /// </summary>
        void Start()
        {
            this.gameObject.SetActive((PlayerPrefs.GetInt("PlayerType") == 1) ? true : false);
            if(this.enabled)
                FindObjectOfType<SharedSpaceManager>().ScannedImage += Singleton_ScannedImage;
                //NetworkLauncher.Singleton.JoinedRoom += Singleton_JoinedRoom;

        }

        private void Singleton_ScannedImage(SharedSpaceManager sender)
        //private void Singleton_JoinedRoom(NetworkLauncher sender)
        {
            pelletShot = CreatePelletShot(pelletTransform);
            this.SlingshotSetup();
            this.UpdatePullbackVisuals();
            this.PositionSlingshotInFrontOfCamera();
        }

        // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        private float _screenAspect = 1f;
        private float _distAhead = 1.84f;

        /// <summary>
        /// Calculates and sets the correct position of the Slingshot 
        /// in front of the camera
        /// </summary>
        private void PositionSlingshotInFrontOfCamera()
        {
            _screenAspect = (float)Screen.height / (float)Screen.width;
            float perc = _screenAspect / _cam.fieldOfView;

            this._distAhead = Mathf.Atan(perc * 100f) - distAheadOffset;

            _slingshotLocalYOffset = _screenAspect * slingshotLocalYOffsetMultiplier + 0.25f;

            Vector3 localPos = this.transform.localPosition;
            localPos.z = this._distAhead * _screenAspect;
            _slingshotOnscreenYPos = this._originalSlingshotRendLocalPos.y + _slingshotLocalYOffset;
            this.transform.localPosition = localPos;
        }

        // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        /// <summary>
        /// Creates a PelletShot to be shot into the world
        /// </summary>
        /// <returns>
        /// A new Pellet shot to send into the world
        /// </returns>
        /// <param name="origTransform">The original transform of the Pellet we are duplicating
        static PelletShot CreatePelletShot(Transform origTransform)
        {
            GameObject spawnedPellet = PhotonNetwork.Instantiate("Pellet", origTransform.position, origTransform.rotation);
            spawnedPellet.transform.localPosition = new Vector3(0,0,0);

            spawnedPellet.transform.parent = null;
            spawnedPellet.transform.name = "WORLD PELLET";
            spawnedPellet.transform.gameObject.SetActive(false);
            spawnedPellet.transform.gameObject.layer = 7; // "pellet"
            spawnedPellet.transform.position = new Vector3(999, 999, 999);

            spawnedPellet.transform.gameObject.SetActive(false);

            Debug.LogError("[Slingshot][CreatePelletShot]spawnedName1" + spawnedPellet.transform.gameObject.name);
            Debug.LogError("[Slingshot][CreatePelletShot]spawnedName2" + spawnedPellet.name);

            return spawnedPellet.transform.gameObject.AddComponent<PelletShot>();

        }

        //   #####  ####### ####### ####### ### #     #  #####   #####  
        //  #     # #          #       #     #  ##    # #     # #     # 
        //  #       #          #       #     #  # #   # #       #       
        //   #####  #####      #       #     #  #  #  # #  ####  #####  
        //        # #          #       #     #  #   # # #     #       # 
        //  #     # #          #       #     #  #    ## #     # #     # 
        //   #####  #######    #       #    ### #     #  #####   #####  

        /// <summary>
        /// Setting up slingshot initial position
        /// </summary>

        public void SlingshotSetup()
        {
            float destY = _slingshotOnscreenYPos;

            float destLocalXRot = _slingshotOnscreenXRot;
            slingshotRenderer.enabled = true;
            reticleRenderer.enabled = true;
            pelletTransform.gameObject.SetActive(true);

            // ANIMATE
            float animDuration = 0.6f;
            EaseType ease = EaseType.BackOut;

            // Animate the slingshot on/off-screen
            this.transform
                .TweenLocalPositionY(destY, animDuration)
                .SetEase(ease);

            // Animate the rotation too...
            ease = EaseType.ExpoOut;
            this.transform.TweenLocalRotationX(destLocalXRot, animDuration)
            // this.transform.TweenLocalRotation(destRot, animDuration)
                .SetEase(ease);

            if(_globalManager.noOfPlanes == 0)
                pelletTransform.gameObject.SetActive(false);
        }

        // --------------------------------------------

        //  ### #     # ######  #     # ####### 
        //   #  ##    # #     # #     #    #    
        //   #  # #   # #     # #     #    #    
        //   #  #  #  # ######  #     #    #    
        //   #  #   # # #       #     #    #    
        //   #  #    ## #       #     #    #    
        //  ### #     # #        #####     #    

        /// <summary>
        /// Static method to return the slingshot pullback percent from the 
        /// touch y-difference
        /// </summary>
        /// <param name="yDelta">The difference from the touch down position
        private static float PullbackPercentWithYDelta(float yDelta)
        {
            return Mathf.Max(0, Mathf.Min(1f, yDelta * 0.002f));
        }

        // --------------------------------------------

        /// <summary>
        /// TouchDown callback from SlingshotTouchResponder
        /// </summary>
        /// <param name="pos">Latest touch position
        /// 

        public void SlingshotUITouchDown(Vector2 pos)
        {
            if (_isShooting) return;
            _touchDownPos = pos;
        }

        /// <summary>
        /// TouchMoved callback from SlingshotTouchResponder
        /// </summary>
        /// <param name="pos">Latest touch position
        public void SlingshotUITouchMoved(Vector2 pos)
        {
            if (_touchDownPos == null) return;

            Vector2 delta = (Vector2)_touchDownPos - pos;

            float percentOfScreen = delta.x / (Screen.width * 0.3f);
            float deltaX = Mathf.Clamp(-percentOfScreen, -1f, 1f);

            // float deltaX = Mathf.Clamp(-delta.x * 0.005f, -1f, 1f);
            this.SetLeftRightAimingPercent(deltaX);

            // Holding the SLING
            this.SetPullbackPercent(Slingshot.PullbackPercentWithYDelta(delta.y));
        }
        /// <summary>
        /// TouchEnded callback from SlingshotTouchResponder
        /// </summary>
        /// <param name="pos">Latest touch position
        public void SlingshotUITouchEnded(Vector2 pos)
        {
            if (_touchDownPos == null) return;
            
            this.PerformShot();

            _touchDownPos = null;
        }

        // --------------------------------------------

        //  #     # ### ####### #     # 
        //  #     #  #  #       #  #  # 
        //  #     #  #  #       #  #  # 
        //  #     #  #  #####   #  #  # 
        //   #   #   #  #       #  #  # 
        //    # #    #  #       #  #  # 
        //     #    ### #######  ## ##  

        /// <summary>
        /// Update the blendshapes on the slingshot mesh using _pullbackPercent.
        /// Also update the position of the pellet
        /// </summary>
        private void UpdatePullbackVisuals()
        {
            // Pullback
            this.slingshotRenderer.SetBlendShapeWeight(0,
                Mathf.Max(0, _pullbackPercent) * 150f);
            // Follow-through
            this.slingshotRenderer.SetBlendShapeWeight(1,
                -Mathf.Min(0, _pullbackPercent) * 150f);

            // Update the position of the pellet as well
            this.pelletTransform.localPosition = _pelletLocalOrigin
                + new Vector3(0, 0, -_pullbackPercent * 13.2f);
        }

        /// <summary>
        /// This function should be called to set the pullbackPercent 
        /// of the slingshot
        /// </summary>
        /// <param name="percent">Pullback percent of the slingshot
        private void SetPullbackPercent(float percent)
        {
            _pullbackPercent = percent;

            UpdatePullbackVisuals();

            if (this.pelletTransform.gameObject.activeInHierarchy) return;
            this.pelletTransform.gameObject.SetActive(false);
            //Debug.LogError("[Slingshot]Pellet set to false - This should never be printed");
        }

        /// <summary>
        /// The slingshot can be aimed left and right, this function does that!
        /// </summary>
        /// <param name="percent">A value of -1 aims 100% to the left, +0.5 aims 50% to the right
        private void SetLeftRightAimingPercent(float percent)
        {

            Vector3 eulerRot = this.transform.localRotation.eulerAngles;
            if (_leftRightTween != null)
            {
                _leftRightTween.Cancel();
                _leftRightTween = null;
                _leftRightPercentOffset = (eulerRot.y > 180f ? eulerRot.y - 360f : eulerRot.y) * 0.1f;
                // Debug.Log("_leftRightPercentOffset: " + _leftRightPercentOffset + ", eulerRot: " + eulerRot);
            }

            float smoothedPercent = _leftRightPercentOffset +
                                    (Mathf.Sign(-percent) *
                                    0.5f * (1f - Mathf.Cos(percent * Mathf.PI)));

            eulerRot.y = Mathf.Clamp(smoothedPercent, -1f, 1f) * 6f;
            this.transform.localRotation = Quaternion.Euler(eulerRot);
        }

        /// <summary>
        /// Shoot the slingshot
        /// </summary>
        private void PerformShot()
        {
            _isShooting = true;
            AnimateShot();

            this.UpdateSlingshotShake(0);
        }

        // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        //     #    #     # ### #     # 
        //    # #   ##    #  #  ##   ## 
        //   #   #  # #   #  #  # # # # 
        //  #     # #  #  #  #  #  #  # 
        //  ####### #   # #  #  #     # 
        //  #     # #    ##  #  #     # 
        //  #     # #     # ### #     # 

        /// <summary>
        /// The user let go of a pulled back slingshot, so take the shot!
        /// </summary>
        private void AnimateShot()
        {
            float shotSpeed = Mathf.Max(0.2f, this._pullbackPercent);
            // Debug.Log($"shotSpeed: {shotSpeed}");
            float originalPullback = this._pullbackPercent;

            float destination = -shotSpeed;

            this.TweenValueFloat(destination, duration: 0.1f,
                (float updateVal) =>
                {
                    _pullbackPercent = updateVal;
                    this.UpdatePullbackVisuals();
                }).SetFrom(this._pullbackPercent)
                .SetEaseLinear()
                .SetOnComplete(() =>
                {
                    if (_globalManager.noOfPellets != 0)
                    {
                        UpdatePelletCount();
                        //this.pelletTransform.localPosition = new Vector3(0, 0, 0);
                        // SHOOT THE WORLD PELLET
                        this.pelletShot.gameObject.SetActive(true);
                        this.pelletShot.removeAllForces();
                        this.pelletShot.transform.position = this.pelletTransform.position;
                        this.pelletShot.transform.rotation = this.pelletTransform.rotation;
                        this.pelletTransform.gameObject.SetActive(false);
                        this.pelletShot.ShootWithSpeedAtCurrentRotation(shotSpeed * 0.4f);
                    }
                    AnimateSlingToRest(shotSpeed);

                });
        }

        /// <summary>
        /// Animate the slingshot to the resting or idle position
        /// </summary>
        /// <param name="shotSpeed">Shot speed
        void AnimateSlingToRest(float shotSpeed)
        {
            float destination = 0f;
            float duration = shotSpeed * 0.5f;

            this.TweenValueFloat(destination, duration,
                (float updateVal) =>
                {
                    _pullbackPercent = updateVal;
                    this.UpdatePullbackVisuals();
                }).SetFrom(this._pullbackPercent)
                .SetEaseElasticOut()
                .SetOnComplete(SlingToRestAnimationComplete);
        }

        /// <summary>
        /// Animate the left/right aiming back to rest.
        /// Also reset any variables concerned with shooting and animation
        /// allowing the user to take another shot
        /// </summary>
        private void SlingToRestAnimationComplete()
        {
            // Ease the Left/Right rotation back
            Vector3 eulerRot = this.transform.localRotation.eulerAngles;
            eulerRot.y = 0;
            // Ease the Left/Right rotation back
            _leftRightTween = this.transform.TweenLocalRotationY(0, duration: 1.6f)
                .SetEaseExpoInOut()
                .SetOnComplete(() =>
                {
                    // Debug.Log("ANIM COMPLETE");
                    _leftRightTween = null;
                    _leftRightPercentOffset = 0;
                });

            this._pullbackPercent = 0;
            this.UpdatePullbackVisuals();
            if(_globalManager.noOfPlanes != 0)
                this.pelletTransform.gameObject.SetActive(true);
            _isShooting = false;
        }

        /// <summary>
        /// Update the Slingshot shake, based on the pullbackPercent.
        /// It's hard to hold an elastic, the further it has been pulled.
        /// </summary>
        private void UpdateSlingshotShake(float magnitude)
        {
            float rotMod = 10f;
            shakeTransform.localRotation = Quaternion.Euler(new Vector3(
                Random.Range(-1f, 1f) * magnitude * rotMod,
                Random.Range(-1f, 1f) * magnitude * rotMod,
                Random.Range(-1f, 1f) * magnitude * rotMod
            ));
        }

        // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        private void UpdatePelletCount()
        {
            _globalManager.noOfPellets -= 1;
            _globalManager.noOfPelletsUI.text = "Ammo : " + _globalManager.noOfPellets;
        }

        // Update is called once per frame
        void Update()
        {
            if (this._pullbackPercent > 0) this.UpdateSlingshotShake(this._pullbackPercent * 0.01f);

            //// DEBUG: UNCOMMENT TO TEST ANIMATE SHOT ON PC
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    this._pullbackPercent = 1f;
            //    Application.targetFrameRate = 1;
            //    AnimateShot();
            //}

        }
    }
}