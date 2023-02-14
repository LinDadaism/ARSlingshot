namespace MyFirstARGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GlobalManager : MonoBehaviour
    {
        public GameObject[] objsToSpawn = new GameObject[5];
        public GameObject waterBucketToSpawn;
        public float timer;
        public float spawnPeriod; // how frequently obj is spawned e.g. every 5 seconds
        public int numberSpawnedEachPeriod;
        public float bucketSpawnPeriod;
        public int numBucketSpawnedEachPeriod;
        public Vector3 originInScreenCoords;
        // public int level; // prototype always default to 1
        public int score;
        public int waterLevel;
        public int numProPack;
        public string logSelected;
        public float horizonMin, horizonMax, verticalMin, verticalMax;
        public int gameState; // 0-ongoing, 1-win, 2-loss

        // Use this for initialization
        void Start()
        {
            //Time.timeScale = 1;

            //score = 0;
            //waterLevel = 100;
            //numProPack = 0;
            //timer = 0;

            //spawnPeriod = 10.0f;
            //numberSpawnedEachPeriod = 1;
            //bucketSpawnPeriod = 20.0f;
            //numBucketSpawnedEachPeriod = 2;

            //originInScreenCoords = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
            //horizonMin = 600.0f;
            //horizonMax = 1200.0f;
            //verticalMin = 50.0f;
            //verticalMax = 1000.0f;
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

            //// spawn water supply
            //if (timer > bucketSpawnPeriod)
            //{
            //    timer = 0;
            //    //float width = Screen.width;
            //    //float height = Screen.height;
            //    for (int i = 0; i < numBucketSpawnedEachPeriod; i++)
            //    {
            //        float horizontalPos = Random.Range(horizonMin + 50, horizonMax = 50);
            //        float verticalPos = Random.Range(verticalMin + 50, verticalMax - 50);
            //        Instantiate(waterBucketToSpawn,
            //            Camera.main.ScreenToWorldPoint(new Vector3(horizontalPos, verticalPos, originInScreenCoords.z)),
            //            Quaternion.identity);
            //    }
            //}
        }
    }
}
