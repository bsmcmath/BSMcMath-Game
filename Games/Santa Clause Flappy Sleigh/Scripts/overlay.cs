using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace santaFlappy
{
    public class overlay : MonoBehaviour
    {
        public TextMeshProUGUI caption, score;

        public void reset()
        {
            gameObject.SetActive(true);
            caption.text = "Press Space to Start";
            score.text = "";
        }
    }
}