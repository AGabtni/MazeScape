using UnityEngine;
using System.Collections;

public class MazeDoor : MazePassage {

	private static Quaternion
		normalRotation = Quaternion.Euler(0f, -90f, 0f),
		mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

	public Transform hinge;
    bool openDoor = false;

	private bool isMirrored;

    public void Update()
    {
        
        if (openDoor)
        {
            if (isMirrored)
            {
                Debug.Log("Slerping");
                hinge.localRotation = Quaternion.Slerp(hinge.localRotation, mirroredRotation, 2 * Time.deltaTime);
                OtherSideOfDoor.hinge.localRotation = Quaternion.Slerp(OtherSideOfDoor.hinge.localRotation, mirroredRotation, 2 * Time.deltaTime);

                if(Quaternion.Angle(hinge.localRotation,mirroredRotation)<= 0)
                {
                    openDoor = false;
                }

            }
            else
            {


                hinge.localRotation = Quaternion.Slerp(hinge.localRotation, normalRotation, 2 * Time.deltaTime);
                OtherSideOfDoor.hinge.localRotation = Quaternion.Slerp(OtherSideOfDoor.hinge.localRotation, normalRotation, 2 * Time.deltaTime);
                Debug.Log(Quaternion.Angle(hinge.localRotation, normalRotation));
                if (Quaternion.Angle(hinge.localRotation, normalRotation) <= 90)
                {
                    openDoor = false;
                }


            }


            //openDoor = false;
        }

    }
    
    private MazeDoor OtherSideOfDoor {
		get {
			return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
		}
	}
	
	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
		base.Initialize(primary, other, direction);

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