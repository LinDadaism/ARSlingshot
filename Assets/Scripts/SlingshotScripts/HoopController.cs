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
            }
        }
    }
}
