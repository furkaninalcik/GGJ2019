using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FallingObject{//6 variables

    GameObject startReference, targetReference;//where the model will be first instanciated, where will be the model moving towards after clicked,  first model, second model with wings
    GameObject go;//local game object to assign position
    Vector3 tempPosition;// used to get position of key locations 
    public bool isHit = false, hasReached = false, hasUpdatedScore = false, isDestroyed = false, isMissed = false;
    int id;//list order
    float fallingVelocity, flyingVelocity;
    int scoreValue;
    GameObject displayedScoreGameObject;
    FloatingText mFloatingText;
    List<GameObject> listOfModels = new List<GameObject>();
    int orderOfModels = 0;
    public string objectClass;

    public FallingObject(GameObject _startReference, GameObject _targetReference, List<GameObject> _listOfModels, string _objectClass, GameObject _displayedScoreGameObject, int _id)
    {
        startReference = _startReference;
        targetReference = _targetReference;
        listOfModels = _listOfModels;
        objectClass = _objectClass;
        displayedScoreGameObject = _displayedScoreGameObject;
        id = _id;

        if (objectClass == "normal") { fallingVelocity = 100.00f; flyingVelocity = 200.00f; scoreValue = 10; }
        else if (objectClass == "light") { fallingVelocity = 150.00f; flyingVelocity = 300.00f; scoreValue = 20; }

        if (SceneManager.GetActiveScene().name.Contains("AR")){
            if (objectClass == "normal") { fallingVelocity = 90.00f; flyingVelocity = 270.00f; scoreValue = 10; }
            else if (objectClass == "light") { fallingVelocity = 200.00f; flyingVelocity = 270.00f; scoreValue = 20; }
        }

        InstanciateGameObject();       
    }

    public FallingObject(GameObject _startReference, GameObject _targetReference, List<GameObject> _listOfModels, string _objectClass, GameObject _displayedScoreGameObject, int _id, int _velocityIncrement)
    {
        startReference = _startReference;
        targetReference = _targetReference;
        listOfModels = _listOfModels;
        objectClass = _objectClass;
        displayedScoreGameObject = _displayedScoreGameObject;
        id = _id;

        if (objectClass == "normal") { fallingVelocity = 100.00f; flyingVelocity = 200.00f; scoreValue = 10; }
        else if (objectClass == "light") { fallingVelocity = 150.00f; flyingVelocity = 300.00f; scoreValue = 20; }

        if (SceneManager.GetActiveScene().name.Contains("AR"))
        {
            if (objectClass == "normal") { fallingVelocity = 90.00f + _velocityIncrement*5; flyingVelocity = 270.00f; scoreValue = 10; }
            else if (objectClass == "light") { fallingVelocity = 200.00f + _velocityIncrement*5; flyingVelocity = 270.00f; scoreValue = 20; }
        }

        InstanciateGameObject();
    }

    void InstanciateGameObject(){
        go = GameObject.Instantiate(listOfModels[0], startReference.transform.position, Quaternion.identity);
        tempPosition = startReference.transform.position;
        go.name = listOfModels[0].tag + id.ToString();
    }

    public void move()
    {
        if (!isHit && !hasReached && !isDestroyed){
            tempPosition.y += fallingVelocity*(-1) * Time.deltaTime;
            go.transform.position = tempPosition;

            if (go.transform.position.y < -6.00f){
                isMissed = true;
                //Debug.Log("missed");
                DestroyFallingObject();
            }
        }
        else if(!hasReached && !isDestroyed){
            var diffofx = targetReference.transform.position.x - tempPosition.x;
            var diffofy = targetReference.transform.position.y - tempPosition.y;
            var diffofz = targetReference.transform.position.z - tempPosition.z;

            var distance = Mathf.Sqrt(diffofx*diffofx + diffofy*diffofy + diffofz*diffofz);

            tempPosition.x += diffofx*flyingVelocity*Time.deltaTime / distance;
            tempPosition.y += diffofy*flyingVelocity*Time.deltaTime / distance;
            tempPosition.z += diffofz*flyingVelocity*Time.deltaTime / distance;
            go.transform.rotation = Quaternion.Euler(0, -90 - Mathf.Rad2Deg * (Mathf.Atan2(diffofz, diffofx)), 0);
            go.transform.position = tempPosition;

            if ( Mathf.Abs(diffofx) < flyingVelocity * Time.deltaTime/2 && Mathf.Abs(diffofy) < flyingVelocity * Time.deltaTime/2){
                ObjectReachedTarget();
            }
        }
    }

    public void ChangeModel(RaycastHit touchedObject){
        GameObject.Destroy(touchedObject.transform.gameObject);
        var diffofx = targetReference.transform.position.x - tempPosition.x;
        var diffofz = targetReference.transform.position.z - tempPosition.z;
        go = GameObject.Instantiate(listOfModels[++orderOfModels], touchedObject.transform.position, Quaternion.Euler(0, -90 - Mathf.Rad2Deg * (Mathf.Atan2(diffofz,diffofx)), 0));
    }

    public void DisplayScore(RaycastHit touchedObject){
        displayedScoreGameObject.GetComponentInChildren<TextMesh>().text = "+" + scoreValue.ToString();
        mFloatingText = new FloatingText(displayedScoreGameObject, touchedObject);
    }

    void ObjectReachedTarget(){
        hasReached = true;
        DestroyFallingObject();
    }

    void DestroyFallingObject(){
        isDestroyed = true;
        GameObject.Destroy(go);
    }
}
