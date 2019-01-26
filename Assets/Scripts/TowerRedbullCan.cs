using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRedbullCan{

    GameObject RedbullCanWithGravity;//prefab
    GameObject InstantiatePosition;
    GameObject ImageTarget;//objects will be children of image target one.
    int id;

    GameObject go;//instance to be used in class
    int levelOfObject;//equals to the number of vertical bottles under each object

    public TowerRedbullCan(GameObject _Model, GameObject _InstantiatePosition, GameObject _Parent, int _id){
        RedbullCanWithGravity = _Model;
        InstantiatePosition = _InstantiatePosition;
        ImageTarget = _Parent;
        id = _id;
        InstantiateObject();
    }

    void InstantiateObject(){
        go = GameObject.Instantiate(RedbullCanWithGravity, InstantiatePosition.transform.position, Quaternion.identity, ImageTarget.transform);
        go.transform.localScale = new Vector3(go.transform.localScale.x/2.0f, go.transform.localScale.y / 2.0f, go.transform.localScale.z / 2.0f); // because I doubled the scale of ImageTarget2 to "10"
    }

    public int GetLevel(){
        float currentYPosition= go.GetComponent<Collider>().bounds.center.y;
        //formerly go.transform.position.y
        float heightOfCan = GetHight();
        /*Debug.Log("hight" + GetHight());
        Debug.Log("yPos" + currentYPosition);
        Debug.Log("levelint" + (int)(currentYPosition / heightOfCan));
        Debug.Log("levelfloat" + (currentYPosition / heightOfCan));*/

        return (int)(currentYPosition / heightOfCan) + 1;
    }

    //Size of the object in the y direction
    public float GetHight(){
        //bounds.extends returns half the size. 
        return go.GetComponent<Collider>().bounds.size.y;// or bounds.max - bounds.min
    }

    public Vector3 GetRotation(){
        return go.transform.rotation.eulerAngles;        
    }

    public bool isStanding(){
        if(go.transform.rotation.eulerAngles == new Vector3(0f, 0f, 0f)){
            return true;
        }
        else{
            return false;
        }
    }

    public bool IsMoving(){
        return false;
    }
}
