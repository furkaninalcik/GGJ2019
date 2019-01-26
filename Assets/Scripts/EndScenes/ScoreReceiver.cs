using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreReceiver : MonoBehaviour {

    public Text scoreText;
    static int lastScore = ScorePasser.lastScore;

    private void Start()
    {
        scoreText.text = "Your score : " + lastScore.ToString();
    }

}
