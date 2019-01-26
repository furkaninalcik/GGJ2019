using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText {

    GameObject displayedTextGameObject;
    RaycastHit touchedObject;
    GameObject go;//local game object to assign position
    GameObject parent;// parent of instanciated text
    public GameObject empty;

    // if position is not determined
    //currently does nothing
    public FloatingText(GameObject _displayedTextGameObject){
        displayedTextGameObject = _displayedTextGameObject;
    }

    // position is rlative to touched object determined and textmesh has no parent
    public FloatingText(GameObject _displayedTextGameObject, RaycastHit _touchedObject){
        displayedTextGameObject = _displayedTextGameObject;
        touchedObject = _touchedObject;
        InstanciateGameObjectOnTarget(touchedObject);
    }

    // if parent determined
    public FloatingText(GameObject _displayedTextGameObject, RaycastHit _touchedObject, GameObject _parent){
        displayedTextGameObject = _displayedTextGameObject;
        touchedObject = _touchedObject;
        parent = _parent;
        InstanciateGameObjectInParent(touchedObject, parent);
    }

    void InstanciateGameObjectOnTarget(RaycastHit touchedObject){
        go = GameObject.Instantiate(displayedTextGameObject, touchedObject.transform.position, Quaternion.identity);
        DestroyTextMesh();

    }

    void InstanciateGameObjectInParent(RaycastHit touchedObject, GameObject parent){
        go = GameObject.Instantiate(displayedTextGameObject.gameObject, touchedObject.transform.position, Quaternion.identity, parent.transform);
        DestroyTextMesh();
    }

    public void DestroyTextMesh()
    {
        GameObject.Destroy(go, 3.20f);
    }
}
