using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneButtonFuncionalities : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            //Goes to first scene
            SceneManager.LoadScene(0);
    }

    public void StartButtonPressed(){
        //Goes to redbull falling game
        SceneManager.LoadScene(2);//Scene 2 is the time mode. The levels mode is unfinished and not played, while it is in the build with scene number 3.
    }
}
