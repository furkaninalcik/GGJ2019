using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {
    
    public GameObject House, Knight;
    public GameObject HouseImage, GoblinImage, KnightImage;
    public GameObject GoblinModel;

    public GameObject ARCameraRef, CameraPosition;
    public Camera arCam;

    Vector3 GoblinPosition;
    bool GoblinPositionFirstAssigned = false;
    Goblin mGoblin;
    List<Goblin> mGoblinList = new List<Goblin>();
    
    private void Start(){
        InvokeRepeating("GetGoblin", 1.0f, 0.5f);
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


    private void Update(){

        //////////////////////////////Starts Here///////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        //ARCameraRef.transform.Translate(CameraPosition.transform.forward * Time.deltaTime);
        Vector3 gazeVector = CameraPosition.transform.forward.normalized * 0.05f; // 0.05 is near clipping plane (image plane) distance
        Vector3 upVector = CameraPosition.transform.up.normalized;
        Vector3 rightVector = CameraPosition.transform.right.normalized;

        Rect r = NearPlaneDimensions(arCam);

        // values to draw a vector from camera position to qr plane through bull icon that is on the image plane

        float u = (r.yMax - r.yMin) * 0.01f;
        float v = (r.xMax - r.xMin) * 0.01f;
        // values to draw a vector from camera position to qr plane through bull icon that is on the image plane

        Vector3 dir = gazeVector + upVector * u + rightVector * (-v);

        //Vector3 dir = new Vector3(gazeVector.x + u, gazeVector.y + v, gazeVector.z ); 

        //dir = new Vector3(gazeVector.x + u, gazeVector.y + v, gazeVector.z );  ;
        Vector3 e = CameraPosition.transform.position;

        float ratio = (-e.y) / dir.y;
        Vector3 dirScaled = new Vector3(dir.x * ratio, dir.y * ratio, dir.z * ratio);
        //Vector3 newPosition = new Vector3(e.x + dirScaled.x, e.y + dirScaled.y, e.z + dirScaled.z);  //e + dirScaled;
        //ARCameraRef.transform.position = new Vector3(e.x + dirScaled.x, e.y + dirScaled.y, e.z + dirScaled.z);  //e + dirScaled;;
        ARCameraRef.transform.position += Vector3.ClampMagnitude((e+dirScaled)-(ARCameraRef.transform.position), 0.05f);

        //////////////////////////////Ends Here/////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////


        foreach (Goblin g in mGoblinList){
            g.MoveGoblin();
            if (DefaultTrackableEventHandler.knightIsFound){
                /*var tempKnightPosition = Knight.transform.position;
                tempKnightPosition.y = 0.0f;
                Knight.transform.position = tempKnightPosition;*/
                if (!(g.isDestroyed || g.hasReached) && IsCloseWithRange(Knight.transform.position, g.go.transform.position, 1.00f)){
                    g.KnightSlayedGoblin();
                }
            }
        }
    }

    void GetGoblin(){
        if (DefaultTrackableEventHandler.goblinIsFound){
            GoblinPosition = GoblinImage.transform.position;
            GoblinPositionFirstAssigned = true;
        }
        if (GoblinPositionFirstAssigned)
        {
            mGoblin = new Goblin(GoblinPosition, GoblinModel, House, GoblinImage);
            mGoblinList.Add(mGoblin);
        }
    }

    bool IsCloseWithRange(Vector3 pos1, Vector3 pos2, float range)
    {
        if (Mathf.Abs(pos1.x - pos2.x) < range && Mathf.Abs(pos1.z - pos2.z) < range)
            return true;
        else
            return false;
    }
}
