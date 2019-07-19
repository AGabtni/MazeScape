using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

    
	Vector3 dollyDir;
	public Vector3 dollyDirAdjusted;
	public float distance;





    //New script declarations : 
    [Header("Camera Properties")]
    private float DistanceAway;                     //how far the camera is from the player.

    public float minDistance = 1.0f;
    public float maxDistance = 2.0f;               //max camera distance

    public float DistanceUp = -2;                    //how high the camera is above the player
    public float smooth = 4.0f;                    //how smooth the camera moves into place
    public float rotateAround = 70f;            //the angle at which you will rotate the camera (on an axis)

    [Header("Player to follow")]
    public Transform target;                    //the target the camera follows

    [Header("Layer(s) to include")]
    public LayerMask CamOcclusion;                //the layers that will be affected by collision

    [Header("Map coordinate script")]
    //    public worldVectorMap wvm;
    RaycastHit hit;
    float cameraHeight = 55f;
    float cameraPan = 0f;
    float camRotateSpeed = 180f;
    Vector3 camPosition;
    Vector3 camMask;
    Vector3 followMask;

    private float HorizontalAxis;
    private float VerticalAxis;



    // Use this for initialization
    void Awake () {
		dollyDir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}
	
	// Update is called once per frame
	void Update () {

        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        Vector3 desiredCameraPos = transform.parent.TransformPoint (dollyDir * maxDistance);
		RaycastHit hit;

		if (Physics.SphereCast (transform.parent.position, 10.0f ,transform.forward, out hit)) {

            if((hit.collider.tag != "Cell") && (hit.collider.tag != "Player"))
            {
                
                distance = Mathf.Clamp((hit.distance * 0.87f), minDistance, maxDistance);
                Debug.Log(hit.collider.name);

            }
            else
            {
                distance = maxDistance;
                Debug.Log("max");

            }


            //Collides but doesnt stop the camera from 
        } else {
            Debug.Log("max");
            distance = maxDistance;
				}



				transform.localPosition = Vector3.Lerp (transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
	}


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }






    //nEW cAMERA SCRIPT 
    /*
    

    // Use this for initialization
    void Start()
    {
        //the statement below automatically positions the camera behind the target.
        rotateAround = target.eulerAngles.y - 45f;


    }

    void LateUpdate()
    {

        HorizontalAxis = Input.GetAxis("Horizontal");
        VerticalAxis = Input.GetAxis("Vertical");

        //Offset of the targets transform (Since the pivot point is usually at the feet).
        Vector3 targetOffset = new Vector3(target.position.x, (target.position.y + 2f), target.position.z);
        Quaternion rotation = Quaternion.Euler(cameraHeight, rotateAround, cameraPan);
        Vector3 vectorMask = Vector3.one;
        Vector3 rotateVector = rotation * vectorMask;
        //this determines where both the camera and it's mask will be.
        //the camMask is for forcing the camera to push away from walls.
        camPosition = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;
        camMask = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;

        occludeRay(ref targetOffset);
        smoothCamMethod();

        transform.LookAt(target);

        #region wrap the cam orbit rotation
        if (rotateAround > 360)
        {
            rotateAround = 0f;
        }
        else if (rotateAround < 0f)
        {
            rotateAround = (rotateAround + 360f);
        }
        #endregion

        rotateAround += HorizontalAxis * camRotateSpeed * Time.deltaTime;
        //DistanceUp = Mathf.Clamp(DistanceUp += VerticalAxis, -0.79f, 2.3f);
        DistanceAway = Mathf.Clamp(DistanceAway += VerticalAxis, minDistance, maxDistance);

    }
    void smoothCamMethod()
    {

        smooth = 4f;
        transform.position = Vector3.Lerp(transform.position, camPosition, Time.deltaTime * smooth);
    }
    void occludeRay(ref Vector3 targetFollow)
    {
        #region prevent wall clipping
        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();
        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        if (Physics.Linecast(targetFollow, camMask, out wallHit, CamOcclusion))
        {
            //the smooth is increased so you detect geometry collisions faster.
            smooth = 10f;
            //the x and z coordinates are pushed away from the wall by hit.normal.
            //the y coordinate stays the same.
            camPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.5f);
        }
        #endregion
    }


    */
}
