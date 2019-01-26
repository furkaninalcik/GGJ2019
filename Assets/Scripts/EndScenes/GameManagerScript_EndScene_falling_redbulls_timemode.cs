using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerScript_EndScene_falling_redbulls_timemode : MonoBehaviour {

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            //returns to main menu
            SceneManager.LoadScene(0);
    }
}
