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

        // Use this for initialization
        void Start()
        {
            noOfPlanes = 5;
            noOfPellets = 10;
            hoopScore = 0;

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
            return new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        }

        void Update()
        {
            //timer += Time.deltaTime;
            //// spawn animals
            //if (timer > spawnPeriod)
            //{
            //    timer = 0;
            //    //float width = Screen.width;
            //    //float height = Screen.height;
            //    for (int i = 0; i < numberSpawnedEachPeriod; i++)
            //    {
            //        float horizontalPos = Random.Range(horizonMin, horizonMax);// 250.0f, 750.0f);
            //        float verticalPos = Random.Range(verticalMin, verticalMax);// 500.0f, 1400.0f);
            //        float yRotation = Random.Range(1.0f, 360.0f);
            //        int idx = Random.Range(0, 5);
            //        Instantiate(objsToSpawn[idx],
            //            Camera.main.ScreenToWorldPoint(new Vector3(horizontalPos, verticalPos, originInScreenCoords.z)),
            //            Quaternion.AngleAxis(yRotation, Vector3.up));
            //    }
            //}

            if(noOfPellets == 0)
            {
                if (this.ammunitionToSpawnPrefab == null || !FindObjectOfType<SharedSpaceManager>().HasFoundOrigin)
                    return;

                Vector3 ammoPos = randomPosGenerator();
                PhotonNetwork.Instantiate(this.ammunitionToSpawnPrefab.name, ammoPos, Quaternion.identity);
            }

        }
    }
}
