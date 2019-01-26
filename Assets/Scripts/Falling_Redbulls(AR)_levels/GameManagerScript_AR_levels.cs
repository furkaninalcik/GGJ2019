
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript_AR_levels : MonoBehaviour
{

    //Redbull cans will be instaanciated at StartRef and will be moving towards ann be destroyed at ARCamera ref afer their collider is hit
    public GameObject StartRef, ARCameraRef;


    ///////////////
    //public GameObject Ref;
    public GameObject CameraPosition;
    public Camera arCam;
    ///////////////

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
    public static float timeCount = 50f;
    public Text timerText;
    public static int totalScore = 0;
    public Text totalScoreText;

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

    bool gameStarted = false;

    //UI Components for the score display panel
    //Canvas is deactive when the scene starts and activates on game end
    public GameObject ScoreCavas;
    public Text ScoreCanvasText;

    //Funcions named CreateNormalCan and CreateLigth is invoked if the time they will be invoked does not exceed the time count
    //All floats in lists are summed and passed to invoke functions to determine when will the functions be invoked
    //Tags of first elements of lists that contains models are added to tagsOfFirstComponents list manually.
    void Start(){

        ScoreCavas.SetActive(false);

        Invoke("SetInstantiates", 5.05f);

        timeCount = 50.00f;
        totalScore = 0;
        timerText.text = timeCount.ToString();
        totalScoreText.text = totalScore.ToString();

        tagsOfFirstComponents.Add(modelsForNormalCan[0].tag);
        tagsOfFirstComponents.Add(modelsForLigthCan[0].tag);
    }

    void SetInstantiates(){
        int i = 0;
        while (true){
            instantiateOrder1.Add(Random.Range(1f, 2.5f));
            instantiateOrder2.Add(Random.Range(3f, 5.5F));
            if (SumUntil(instantiateOrder2, i) <= timeCount) { Invoke("CreateLigthCan", SumUntil(instantiateOrder2, i)); }
            if (SumUntil(instantiateOrder1, i) <= timeCount) { Invoke("CreateNormalCan", SumUntil(instantiateOrder1, i)); }
            else { break; }
            i++;
        }
        gameStarted = true;
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
            mFallingObject = new FallingObject(StartRef, ARCameraRef, modelsForNormalCan, "normal", displayedScoreGameObject, listOrder);
            mFallingObjectList.Add(mFallingObject);
        }
    }
    void CreateLigthCan(){
        if (0.00f <= timeCount){
            listOrder += 1;
            mFallingObject = new FallingObject(StartRef, ARCameraRef, modelsForLigthCan, "light", displayedScoreGameObject, listOrder);
            mFallingObjectList.Add(mFallingObject);
        }
    }

    //////////////////////////////Starts Here///////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////
    Rect NearPlaneDimensions(Camera cam)
    {
        Rect r = new Rect();
        float a = cam.nearClipPlane;//get length
        float A = cam.fieldOfView * 0.5f;//get angle
        A = A * Mathf.Deg2Rad;//convert tor radians
        float h = (Mathf.Tan(A) * a);//calc height
        float w = (h / cam.pixelHeight) * cam.pixelWidth;//deduct width

        r.xMin = -w;
        r.xMax = w;
        r.yMin = -h;
        r.yMax = h;
        return r;
    }
    //////////////////////////////Ends Here/////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////

    void Update(){

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        //////////////////////////////Starts Here///////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        //ARCameraRef.transform.Translate(CameraPosition.transform.forward * Time.deltaTime);
        Vector3 gazeVector = CameraPosition.transform.forward.normalized * 0.05f; // 0.05 is near clipping plane (image plane) distance
        Vector3 upVector = CameraPosition.transform.up.normalized;
        Vector3 rightVector = CameraPosition.transform.right.normalized;
        
        Rect r = NearPlaneDimensions(arCam);
        
        // values to draw a vector from camera position to qr plane through bull icon that is on the image plane

        float u = (r.yMax - r.yMin) * 0.39f;
        float v = (r.xMax - r.xMin) * 0.35f - (scoreBar.value * 0.0004f); // 0.00035 was good for our target to catch the bull icon in Unity Editor but changed to .001 for build test
        // values to draw a vector from camera position to qr plane through bull icon that is on the image plane

        Vector3 dir = gazeVector + upVector * u + rightVector * (-v);

        //Vector3 dir = new Vector3(gazeVector.x + u, gazeVector.y + v, gazeVector.z ); 

        //dir = new Vector3(gazeVector.x + u, gazeVector.y + v, gazeVector.z );  ;
        Vector3 e = CameraPosition.transform.position;
        float ratio = (-e.z) / dir.z;
        Vector3 dirScaled = new Vector3(dir.x * ratio, dir.y * ratio, dir.z * ratio);
        ARCameraRef.transform.position = e + dirScaled;

        //////////////////////////////Ends Here/////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////


        if (Input.GetMouseButtonDown(0)){
            //Debug.Log(Input.mousePosition);
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
                    else
                    {
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
                    if(mFallingObjectList[i].objectClass == "normal") { totalScore += 10;  scoreBar.value += 1; }//ARCameraRef.transform.position is updated at two places in the code. This part to be deleted and other part to be recalculated in optimization.
                    if(mFallingObjectList[i].objectClass == "light") { totalScore += 20; scoreBar.value += 2; }  //To be deleted from var=tempPos                  
                    totalScoreText.text = totalScore.ToString();
                }
            }
        }

        if(scoreBar.value == 50) { timeCount = 0.00f; }
        else if (gameStarted) { timeCount -= Time.deltaTime; }

        if ( 0.00f > timeCount) { TimeHasEnded(); }
        if ((int)timeCount >= 0) { timerText.text = ((int)timeCount).ToString(); }
        else { timerText.text = "0"; }    
    }

    void TimeHasEnded(){
        Invoke("ChangeScene", 4.00f);
    }
    void ChangeScene(){
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Currently the score is dispayed on a . The does not change
        Debug.Log("Scene Change");
        ScoreCavas.SetActive(true);
        ScoreCanvasText.text = "Puanınız" + totalScore.ToString();
    }
}
