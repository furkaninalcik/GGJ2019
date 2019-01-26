using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScorePasser : MonoBehaviour {

    public static int lastScore;
    bool hasUpdatedScore;

    private void Awake(){

        SceneManager.sceneUnloaded += ExitEndScene;

        if (SceneManager.GetActiveScene().name.Contains("AR"))
        {
            lastScore = 0;
            hasUpdatedScore = false;
            DontDestroyOnLoad(transform.gameObject);
        }
        
    }    

    private void Update(){

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(lastScore);
        }

        if (!hasUpdatedScore){
            if (3 <= GameManagerScript_AR_timemode.missedCans){
                lastScore = GameManagerScript_AR_timemode.totalScore;
                hasUpdatedScore = true;                
            }
        }
    }

    void ExitEndScene<Scene>(Scene scene)
    {
        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            Debug.Log("destroyed");
            Destroy(this);
        }
    }

}
