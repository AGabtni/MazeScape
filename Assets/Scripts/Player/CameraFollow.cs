using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private VariableJoystick variableJoystick;
	public float CameraMoveSpeed = 120.0f;
	public GameObject followPoint;
	public GameObject aimPoint;
	Vector3 FollowPOS;
	private const float clampAngleX = 80.0f;
    private const float clampAngleMinY = 0f;
    private const float clampAngleMaxY = 80.0f;

    public float inputSensitivity = 150.0f;
	public GameObject PlayerObj;
	
	public float mouseX;
	public float mouseY;
	
	private float rotY = 0.0f;
	private float rotX = 0.0f;

	public bool focus;


	// Use this for initialization
	void Start () {
        #if UNITY_ANDROID && UNITY_EDITOR
                variableJoystick = GameObject.Find("Camera Joystick").GetComponent<VariableJoystick>();
        #elif UNITY_EDITOR
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
        #endif

        Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;	
		focus =false;
		
	}
	
	// Update is called once per frame
	void Update () {

  

            #if UNITY_ANDROID && UNITY_EDITOR

        
                mouseX = variableJoystick.Horizontal;
                mouseY = variableJoystick.Vertical;
            #elif !UNITY_EDITOR

                mzouseX = Input.GetAxis ("Mouse X");
                mouseY = Input.GetAxis ("Mouse Y");

            #endif
	



    }

	void LateUpdate () {
		CameraUpdater ();
	}

	public void CameraFocus(Vector3 movement){

		focus = !focus;
	}
	void CameraUpdater() {
		// set the target object to follow
		Transform target;
		if(!focus){
			target = followPoint.transform;
			
        	rotY += mouseX * inputSensitivity * Time.deltaTime;
			rotX += mouseY * inputSensitivity * Time.deltaTime;

			rotX = Mathf.Clamp (rotX, -clampAngleX, clampAngleX);
        	rotX = Mathf.Clamp(rotX, clampAngleMinY, clampAngleMaxY);
			Quaternion localRotation = Quaternion.Euler (rotX, rotY, 0.0f);
			transform.rotation = localRotation;

		}
		else{
			
			target = aimPoint.transform;	
					//Camera rotation : 
			
			Vector3 newDirection = Vector3.RotateTowards(transform.position, target.parent.forward, Time.deltaTime, 0.0f);

			//transform.LookAt(newDirection);
			transform.rotation = target.rotation;
			transform.LookAt(target);
		}
		//move towards the game object that is the target
		
		float step = CameraMoveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.position, step);


		

	


	}
}
