namespace ARSlingshot
{
    using Photon.Pun;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// This class is responsible for controlling the Hoop prefab. The prefab gets instantiated by <see cref="SharedSpaceManager"/>
    /// when the tracking image is recognized.
    /// </summary>
    public class HoopController : MonoBehaviourPun
    {
        /// <summary>
        /// Updates the hoop existence on other clients so that they cannot repetitively spawn.
        /// </summary>
        /// <param name="exists">If the hoop exists.</param>
        [PunRPC]
        public void UpdateHoop(bool exists)
        {
            var sharedSpaceManager = FindObjectOfType<SharedSpaceManager>();
            if (sharedSpaceManager != null)
            {
                sharedSpaceManager.hoopSpawned = exists;
                Debug.Log("other player hoop updated");
            }
        }

        // use trigger as collider to allow airplane fly through the hoop
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Airplane"))
            {
                // update the score
                GlobalManager _globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
                _globalManager.hoopScore += _globalManager.hoopScoreIncrement;
                _globalManager.hoopScoreUI.text = "Hoop Score : " + _globalManager.hoopScore;
                this.gameObject.GetPhotonView().RPC("UpdateHoopScore", RpcTarget.OthersBuffered);
            }
        }

        [PunRPC]
        public void UpdateHoopScore()
        {
            GlobalManager _globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
            _globalManager.hoopScore += _globalManager.hoopScoreIncrement;
            _globalManager.hoopScoreUI.text = "Hoop Score : " + _globalManager.hoopScore;
        }
    }
}
