namespace ARSlingshot
{
    using UnityEngine;

    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (SphereCollider))]
    public class PelletShot : MonoBehaviour
    {
        private Rigidbody _rigidBody = null;
        private SphereCollider _sphereCollider = null;
        
        // private bool _isAirborne = false;
        private float _speed = 0;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        void OnEnable()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _rigidBody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Remove any forces from the Pellet's Rigidbody
        /// </summary>
        public void removeAllForces() {
            if (_rigidBody == null) return;
            //Debug.LogError("[PelletShot][removeAllForces]");
            _rigidBody.velocity = Vector3.zero;
        }

        /// <summary>
        /// Perform a shot with the provided speedPercent at the Pellet's current rotation
        /// </summary>
        /// <param name="speedPercent">A speed to perform the pellet shot with</param>
        public void ShootWithSpeedAtCurrentRotation(float speedPercent) {
            if (_rigidBody == null) return;
            //Debug.LogError("[PelletShot][ShootWithSpeedAtCurrentRotation] transform.forward" + transform.forward);
            //Debug.LogError("[PelletShot][ShootWithSpeedAtCurrentRotation] transform.position" + transform.position);
            //Debug.LogError("[PelletShot][ShootWithSpeedAtCurrentRotation] transform.position" + transform.localPosition);

            // _isAirborne = true;
            _speed = 50f * speedPercent;

            //Vector3 force = transform.forward * _speed;
            //_rigidBody.AddForce(force, ForceMode.Impulse);

            //// DEBUG: To view direction on PC Scene
            //Debug.DrawLine(transform.position, transform.position + (transform.forward * 10), Color.red, 20);
            _rigidBody.isKinematic = false;
            _rigidBody.velocity = transform.forward * _speed;
            //Debug.Log("forceShoot" + this.GetComponent<Rigidbody>().velocity);
        }

        //private void Update()
        //{
        //    //Debug.Log("force" + this.GetComponent<Rigidbody>().velocity);
        //}
    }
}