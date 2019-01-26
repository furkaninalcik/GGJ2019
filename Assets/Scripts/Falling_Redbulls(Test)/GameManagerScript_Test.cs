using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript_Test : MonoBehaviour
{
    //Redbull cans will be instaanciated at StartRef and will be moving towards ann be destroyed at bullicon afer their collider is hit
    public GameObject StartRef;
    public RawImage bullIcon;

    //Each list contains prefabs (models with or without animations) related to each type of redbull can. 
    //Lists are passed into falling objects, hence any number of prefabs can be passed and used during game play.
    //Each element of each list (or at least first elements) should have unique tags, which are used in this class and fallingobject class.
    public List<GameObject> modelsForNormalCan = new List<GameObject>();
    public List<GameObject> modelsForLigthCan = new List<GameObject>();

    public Slider scoreBar;

    //Every falling object created in this class is stored in a list and list order passed in as id for making each falling object unique
    //listOrder is incremented each time a falling object is called, hence is initialized as -1.
    int listOrder = -1;

    //The game ends in timeCount seconds and scene changes a few seconds later.
    float timeCount = 50f;
    public Text timerText;

    //Every falling object is added to mFallingObjectListi for checking every falling object.
    List<FallingObject> mFallingObjectList = new List<FallingObject>();

    //mFallingObject is the variable name for FallingObject class which is declared once in the code, hence all falling objects has the same name which makes using a list order as id a necessity.
    FallingObject mFallingObject;

    //dislayedScoreGameObject is assigned a game object prefab with a 3dTextMesh as child.
    //The prafab is instanciated in floatingText class which is called (in this case) in falling objced class, yet needed to be passed into falling objects from this class because floating object doeas not exist in scene and variables cannot be assigned in editor.
    public GameObject displayedScoreGameObject;

    //A list that contains the tags of first elements of each list that contains models which is used as a practical (may be not) solution for distinguishing hits of redbull cans and hits of other objects (if there is any).
    List<string> tagsOfFirstComponents = new List<string>();

    //Each list will contain 30 randomly generated floats which are the invoke times for instantiations of Redbull can models.
    //The duration of geme (timercount) requires approximately 23 instantiations, 30 in case most of the floats normal cans
    List<float> instantiateOrder1 = new List<float>();
    List<float> instantiateOrder2 = new List<float>();

    //Funcions named CreateNormalCan and CreateLigth is invoked if the time they will be invoked does not exceed the time count
    //All floats in lists are summed and passed to invoke functions to determine when will the functions be invoked
    //Tags of first elements of lists that contains models are added to tagsOfFirstComponents list manually.
    void Start(){
        int i = 0;
        while (true){            
            instantiateOrder1.Add(Random.Range(1f, 2.5f));
            instantiateOrder2.Add(Random.Range(3f, 5.5F));
            if(SumUntil(instantiateOrder2, i) <= timeCount){ Invoke("CreateLigthCan", SumUntil(instantiateOrder2, i)); }
            if(SumUntil(instantiateOrder1, i) <= timeCount){ Invoke("CreateNormalCan", SumUntil(instantiateOrder1, i)); }
            else{ break; }
            i++;
        }

        timerText.text = timeCount.ToString();

        tagsOfFirstComponents.Add(modelsForNormalCan[0].tag);
        tagsOfFirstComponents.Add(modelsForLigthCan[0].tag);
    }

    float SumUntil(List<float> fList, int i){
        float sum = 0f;
        if (i < 0){
            Debug.Log("index cannot be smaller than zero");
            return 0f;
        }
        else{
            while (i >= 0){
                sum += fList[i];
                i--;
            }
            return sum;
        }
    }

    //Six variables is passed to falling objects. Fourth variable is a string to describe the type of the redbull can. It determines the velocities, scores etc. in the class.
    //This is used not to deal with too many variables in the main class. ( And not to pass too many variables)
    void CreateNormalCan(){
        if (0.00f <= timeCount){
            listOrder += 1;
            mFallingObject = new FallingObject(StartRef, bullIcon.gameObject, modelsForNormalCan, "normal", displayedScoreGameObject, listOrder);
            mFallingObjectList.Add(mFallingObject);
        }
    }
    void CreateLigthCan(){
        if (0.00f <= timeCount){
            listOrder += 1;
            mFallingObject = new FallingObject(StartRef, bullIcon.gameObject, modelsForLigthCan, "light", displayedScoreGameObject, listOrder);
            mFallingObjectList.Add(mFallingObject);
        }
    }

    void Update(){
        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)){

                if (hit.transform != null){

                    //This block first checks whether the hit has a particular tag assigned in editor and finds the game object that is hit.
                    //Tag is the tag of the first component in models list.
                    if (tagsOfFirstComponents.Contains(hit.transform.gameObject.tag)){
                       for (int i = 0; i<mFallingObjectList.Count; i++){
                            foreach(string tag in tagsOfFirstComponents){
                                if (hit.transform.gameObject.name == tag + i){
                                    mFallingObjectList[i].isHit = true;
                                    mFallingObjectList[i].ChangeModel(hit);
                                    mFallingObjectList[i].DisplayScore(hit);
                                }
                            }                            
                       } 
                    }
                    else{
                        Debug.Log("No action is defined for game objects of this tag or this game object is untagged.");
                    }
                }
            }
        }

        for (int i = 0; i<mFallingObjectList.Count; i++){
            mFallingObjectList[i].move();
            if (!mFallingObjectList[i].hasUpdatedScore && mFallingObjectList[i].hasReached){
                mFallingObjectList[i].hasUpdatedScore = true;
                if (scoreBar.value <= 50){
                    if(mFallingObjectList[i].objectClass == "normal") { scoreBar.value += 1; }
                    if(mFallingObjectList[i].objectClass == "red") { scoreBar.value += 2; }                    
                }
            }
        }

        if(scoreBar.value == 50) { timeCount = 0.00f; }
        if( 0.00f > timeCount) { TimeHasEnded(); }
        timeCount -= Time.deltaTime;
        if ((int)timeCount >= 0) { timerText.text = ((int)timeCount).ToString(); }
        else { timerText.text = "0"; }    
    }

    void TimeHasEnded(){
        //Invoke("ChangeScene", 4.00f);
    }
    void ChangeScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
