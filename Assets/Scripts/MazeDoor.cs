using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MazeDoor : MazePassage {


    private OffMeshLink offMeshLink;
    public float closeDoorTime = 5.0f;
	private static Quaternion
		normalRotation = Quaternion.Euler(0f, -90f, 0f),
		mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

	public Transform hinge;
    bool openDoor = false;

	private bool isMirrored;

    public void Start()
    {
    }
    public void Update()
    {
        
        if (openDoor)
        {
            if (isMirrored)
            {
                hinge.localRotation = Quaternion.Slerp(hinge.localRotation, mirroredRotation, 2 * Time.deltaTime);
                OtherSideOfDoor.hinge.localRotation = Quaternion.Slerp(OtherSideOfDoor.hinge.localRotation, mirroredRotation, 2 * Time.deltaTime);

                if( Quaternion.Angle(hinge.localRotation, mirroredRotation) <= 0)
                {
                    openDoor = false;
                    StartCoroutine("CloseDoor");
                }

            }
            else
            {
                hinge.localRotation = Quaternion.Slerp(hinge.localRotation, normalRotation, 2 * Time.deltaTime);
                OtherSideOfDoor.hinge.localRotation = Quaternion.Slerp(OtherSideOfDoor.hinge.localRotation, normalRotation, 2 * Time.deltaTime);
                if (Quaternion.Angle(hinge.localRotation, normalRotation) <= 0 )
                {

                    openDoor = false;

                    StartCoroutine("CloseDoor");
                }


            }


        }
        


    }
    

    private IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(4.0f);

        while(Quaternion.Angle(hinge.localRotation, Quaternion.identity) > 0)
        {
            hinge.localRotation = Quaternion.Slerp(hinge.localRotation, Quaternion.identity, 2 * Time.deltaTime);
            OtherSideOfDoor.hinge.localRotation = Quaternion.Slerp(OtherSideOfDoor.hinge.localRotation, Quaternion.identity, 2 * Time.deltaTime);

            yield return new WaitForSeconds(0.01f);

        }
        
        


    }
    private MazeDoor OtherSideOfDoor {
		get {
			return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
		}
	}
	
	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
		base.Initialize(primary, other, direction);
        offMeshLink = GetComponent<OffMeshLink>();


        if (OtherSideOfDoor != null) {
            isMirrored = true;

            hinge.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 p = hinge.localPosition;
			p.x = -p.x;
			hinge.localPosition = p;
		}
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			if (child != hinge) {
				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
			}
		}


   
        offMeshLink.startTransform = primary.transform;
        offMeshLink.endTransform = other.transform;

    }


    public override void OnPlayerEntered () {
		//OtherSideOfDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
        openDoor = true;
		OtherSideOfDoor.cell.room.Show();
	}
	
	public override void OnPlayerExited () {
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
        openDoor = false;
		OtherSideOfDoor.cell.room.Hide();
	}

    public bool isOpen
    {
        get {
            return openDoor;
        }
    }




}