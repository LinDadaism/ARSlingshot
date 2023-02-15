namespace ARSlingshot
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ScoreUI : MonoBehaviour
    {
        GlobalManager globalObj;
        Text scoreText;

        // Use this for initialization
        void Start()
        {
            globalObj = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
            scoreText = gameObject.GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            scoreText.text = "Score: " + globalObj.score.ToString();
        }
    }
}
