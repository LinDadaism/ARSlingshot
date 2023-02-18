using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ARSlingshot
{
    public class GameOverScript : MonoBehaviour
    {
        public TextMeshProUGUI winnerUI;

        // Start is called before the first frame update
        void Start()
        {
            this.winnerUI.text = PlayerPrefs.GetString("Winner");
        }
    }
}
