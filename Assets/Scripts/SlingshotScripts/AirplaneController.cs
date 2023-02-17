namespace ARSlingshot
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AirplaneController : MonoBehaviour
    {
        private HoopController targetHoop;

        void OnCollisionEnter(Collision collision)
        {
            // the Collision contains a lot of info, but it’s the colliding
            // object we’re most interested in.
            Collider collider = collision.collider;

            // if hit by a pellet, fall down straightly
            if (collider.CompareTag("Pellet"))
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                // gameObject.transform.Translate(0, -9.8f, 0);
            }
            // if hit the target hoop, increase score
            //else if (collider.CompareTag("Hoop"))
            //{
            //    this.targetHoop = GameObject.FindGameObjectWithTag("Hoop").GetComponent<HoopController>();
            //    this.targetHoop.UpdateScore();

            //    // allow airplane to move through the hoop
            //    this.targetHoop.GetComponent<Collider>().enabled = false;
            //}
        }
    }
}
