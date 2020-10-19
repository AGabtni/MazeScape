using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    public LayerMask ignoreMask;
    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;


    private Vector3 _targetDirection;
    private float _distance;
    private float _zoomeOutFOV;
    private float _zoomedInFOV;
    public float zoomeOutFOV{
        get {return _zoomeOutFOV;}
    }
    public float zoomedInFOV{
        get {return _zoomedInFOV;}
    }


    // Use this for initialization
    void Awake()
    {
        _targetDirection = transform.localPosition.normalized;
        _distance = transform.localPosition.magnitude;
        GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("Map"));
        _zoomeOutFOV = GetComponent<Camera>().fieldOfView;
        _zoomedInFOV = _zoomeOutFOV/2 ;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 desiredCameraPos = transform.parent.TransformPoint(_targetDirection * maxDistance);
        RaycastHit hit;
		
        //Zoom in when camera hits one of the ignored masks (Wall , door, obstacles)

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit, ignoreMask))
            _distance = Mathf.Clamp((hit.distance * 0.87f), minDistance, maxDistance);
        else
            _distance = maxDistance;

        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetDirection * _distance, Time.deltaTime * smooth);

    }
}
