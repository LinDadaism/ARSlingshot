namespace ARSlingshot
{
    using Photon.Pun;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    [System.Serializable]
    public class GlobalManager : MonoBehaviour
    {
        public GameObject ammunitionToSpawnPrefab;
        public Vector3 originInScreenCoords;

        public int noOfPlanes;
        public TextMeshProUGUI noOfPlanesUI;

        public int noOfPellets;
        public TextMeshProUGUI noOfPelletsUI;

        public int hoopScore;
        public TextMeshProUGUI hoopScoreUI;

        public int gameState;
        public int hoopScoreIncrement;

        // vars below for spawning pellet to test collision with airplane
        //public GameObject pelletToSpawn;
        //public float spawnPeriod = 1.0f; // how frequently obj is spawned e.g. every 5 seconds
        //public int numberSpawnedEachPeriod = 3;
        //private float timer = 0.0f;

        // Use this for initialization
        void Start()
        {
            noOfPlanes = 5;
            noOfPellets = 10;
            hoopScore = 0;
            hoopScoreIncrement = 10;

            this.noOfPlanesUI.text = "Planes: " + this.noOfPlanes;
            this.noOfPelletsUI.text = "Ammo: " + this.noOfPellets;
            this.hoopScoreUI.text = "Hoops: " + this.hoopScore;
            //originInScreenCoords = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
            //horizonMin = 600.0f;
            //horizonMax = 1200.0f;
            //verticalMin = 50.0f;
            //verticalMax = 1000.0f;
        }

        private Vector3 randomPosGenerator()
        {
            return new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2));
        }

        void Update()
        {
            /*---------start: test pellet airplane collision---------*/
            //timer += Time.deltaTime;
            //// spawn animals
            //if (timer > spawnPeriod)
            //{
            //    timer = 0;
            //    //float width = Screen.width;
            //    //float height = Screen.height;
            //    for (int i = 0; i < numberSpawnedEachPeriod; i++)
            //    {
            //        GameObject pellet = PhotonNetwork.Instantiate(pelletToSpawn.name,
            //            new Vector3(0.0f,0.0f,0.0f),
            //            Quaternion.identity);
            //        pellet.transform.Translate(0, 5, 0);
            //    }
            //}
            /*---------end: test pellet airplane collision---------*/

            if (noOfPellets == 0)
            {
                if (this.ammunitionToSpawnPrefab == null || !FindObjectOfType<SharedSpaceManager>().HasFoundOrigin)
                {
                    Debug.Log("cannot spawn ammo");
                    return;
                }

                Vector3 ammoPos = randomPosGenerator();
                PhotonNetwork.Instantiate(this.ammunitionToSpawnPrefab.name, ammoPos, Quaternion.identity);
            }

        }
    }
}
