using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trialtransform : MonoBehaviour {

    public GameObject sp1, sp2, sp3, sp4, sp5;//2 4
    Vector3 tempPosition;
    bool hasreached = false;

    private void Start()
    {
        tempPosition = sp2.transform.position;
    }

    void Update () {

        if (!hasreached)
        {
            var diffofx = sp4.transform.position.x - tempPosition.x;
            var diffofy = sp4.transform.position.y - tempPosition.y;
            //var diffofz = sp4.transform.position.z - tempPosition.z;

            var distance = Mathf.Sqrt(diffofx * diffofx + diffofy * diffofy /*+ diffofz * diffofz*/);

            tempPosition.x += diffofx * 1.00f * Time.deltaTime / distance;
            tempPosition.y += diffofy * 1.00f * Time.deltaTime / distance;
            //tempPosition.z += diffofz * 1.00f * Time.deltaTime / distance;

            sp2.transform.position = tempPosition;

            if (diffofx < 0.05f && diffofy < 0.05f /*&& diffofz < 0.05f*/)
            {
                hasreached = true;
            }
        }

        if (Input.GetMouseButtonDown(0))//2 4
        {
            Debug.Log("0" + sp1.transform.position);
            Debug.Log("1" + sp2.transform.position);
            Debug.Log("2" + sp3.transform.position);
            Debug.Log("3" + sp4.transform.position);
            Debug.Log("4" + sp5.transform.position);
        }
	}
}
