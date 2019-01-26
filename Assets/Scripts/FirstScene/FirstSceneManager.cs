using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenFirstGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Goes to the start menu of redbull falling game
        SceneManager.LoadScene(1);

    }

    public void OpenSecondGame()
    {
        //Goes to redbull tower game
        SceneManager.LoadScene(4);

    }

}
