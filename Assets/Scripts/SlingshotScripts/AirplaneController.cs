namespace ARSlingshot
{
    using Photon.Pun;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AirplaneController : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            // the Collision contains a lot of info, but it’s the colliding
            // object we’re most interested in.
            Collider collider = collision.collider;

            // if hit by a pellet, fall down straightly
            if (collider.CompareTag("Pellet"))
            {
                // update score
                GlobalManager _globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
                //_globalManager.noOfPlanes--;
                //_globalManager.noOfPlanesUI.text = "Planes: " + _globalManager.noOfPlanes;
                //GameObject.Find("GlobalManager").GetPhotonView().RPC("UpdatePlaneCount", RpcTarget.OthersBuffered, false);

                _globalManager.planesShot++;
                _globalManager.planesShotUI.text = "Planes Shot: " + _globalManager.planesShot;
                GameObject.Find("GlobalManager").GetPhotonView().RPC("UpdateShotCount", RpcTarget.OthersBuffered);

                // highlight the object selected
                this.transform.GetChild(0).gameObject.SetActive(true);

                // Destroy gameObject from the scene and mark it for garbage collection
                //PhotonNetwork.Destroy(this.gameObject);
                
                // legacy: plane fall straight down after hit
                // gameObject.GetComponent<Rigidbody>().isKinematic = false;
                // gameObject.transform.Translate(0, -9.8f, 0);
            }
        }
    }
}
