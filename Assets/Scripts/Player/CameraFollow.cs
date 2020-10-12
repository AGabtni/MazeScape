using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

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


    private bool isCameraLocked = false;

    // Use this for initialization
    void Start()
    {
#if UNITY_ANDROID && UNITY_EDITOR
                variableJoystick = GameObject.Find("Camera Joystick").GetComponent<VariableJoystick>();
#else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        focus = false;

    }


    // Update is called once per frame
    void Update()
    {

#if UNITY_ANDROID && !UNITY_EDITOR

        
    	mouseX = isCameraLocked? 0 : variableJoystick.Horizontal;
		mouseY = isCameraLocked? 0 :variableJoystick.Vertical;
#else

        mouseX = isCameraLocked ? 0 : Input.GetAxis("Mouse X");
        mouseY = isCameraLocked ? 0 : Input.GetAxis("Mouse Y");

#endif

    }

    void LateUpdate()
    {
        CameraUpdater();
    }

    public void CameraFocus(Vector3 movement)
    {

        focus = !focus;
		transform.LookAt(followPoint.transform.parent.forward);
		            

    }
    void CameraUpdater()
    {
        // set the target object to follow
        if (!focus)
        {

            rotY += mouseX * inputSensitivity * Time.deltaTime;
            rotX += mouseY * inputSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngleX, clampAngleX);
            rotX = Mathf.Clamp(rotX, clampAngleMinY, clampAngleMaxY);
            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation;

        }
        else
        {

            //Camera rotation : 
            transform.rotation = followPoint.transform.rotation;
            transform.LookAt(Vector3.RotateTowards(transform.position, followPoint.transform.position, Time.deltaTime, 0.0f));
	
        }
        //move towards the game object that is the target

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, followPoint.transform.position, step);







    }

    public void TriggerCameraLock()
    {
        isCameraLocked = !isCameraLocked;
        Cursor.lockState = isCameraLocked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isCameraLocked;
    }
    public void CameraUnlock()
    {
        isCameraLocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
