using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin{

    Vector3 GoblinPosition;
    GameObject GoblinModel, Target;
    public GameObject go; //temporary game object
    GameObject GoblinImage; //Parent of go

    public bool hasReached = false, isDestroyed = false;
    Vector3 tempPosition;
    float velocity = 0.6f;

    public Goblin(Vector3 _GoblinPosition, GameObject _GoblinModel, GameObject _Target, GameObject _GoblinImage)
    {
        GoblinPosition = _GoblinPosition;
        GoblinPosition.y = 0.0f;
        GoblinModel = _GoblinModel;
        GoblinImage = _GoblinImage;
        Target = _Target;
        InstantiateGoblin();
    }

    void InstantiateGoblin()
    {
        go = GameObject.Instantiate(GoblinModel, GoblinPosition, Quaternion.identity, GoblinImage.transform);
        tempPosition = GoblinPosition;
    }

    public void MoveGoblin()
    {
        if(!hasReached && !isDestroyed)
        {
            float distX, distZ; //Distance between Goblin and House in x and z directions
            float distance; //Total distance between goblin and house
            distX = Target.transform.position.x - tempPosition.x;
            distZ = Target.transform.position.z - tempPosition.z;

            distance = Mathf.Sqrt(distX * distX + distZ * distZ);

            tempPosition.x += distX * velocity * Time.deltaTime / distance;
            tempPosition.z += distZ * velocity * Time.deltaTime / distance;

            go.transform.position = tempPosition;

            //Debug.Log(distX);
            //Debug.Log(distZ);

            if (distX < velocity * Time.deltaTime / 2 && distZ < velocity * Time.deltaTime / 2)
            {
                GoblinReachedHouse();
            }
        }
    }

    void GoblinReachedHouse(){
        GameObject.Destroy(go);
        hasReached = true;
        Debug.Log("goblin reached house");
    }

    public void KnightSlayedGoblin(){
        GameObject.Destroy(go);
        isDestroyed = true;
        Debug.Log("goblin slayed");
    }
}
