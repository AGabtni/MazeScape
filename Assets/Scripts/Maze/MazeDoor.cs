using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MazeDoor : MazePassage
{


    private OffMeshLink offMeshLink;
    public float closeDoorTime = 5.0f;
    private static Quaternion
        normalRotation = Quaternion.Euler(0f, -90f, 0f),
        mirroredRotation = Quaternion.Euler(0f, 90f, 0f);
    private static Vector3 UpPosition, MirroredUp;
    public Transform hinge;

    private bool openDoor = false;
    private bool isOpen = false;

    private bool isMirrored;

    public void Start()
    {

    }
    public void Update()
    {



        isOpen = Quaternion.Angle(hinge.localRotation, Quaternion.identity) > 0 ? true : false;


    }


    //TODO : fix door operations . Issue for when the door is mirrored



    public IEnumerator OpenDoor()
    {
        StopAllCoroutines();
        openDoor = true;
        OtherSideOfDoor.isDoorOpen = true;

        while (Quaternion.Angle(hinge.localRotation, isMirrored ? mirroredRotation : normalRotation) > 1)
        {
            OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.Slerp(hinge.localRotation,
                                                                                         isMirrored ? mirroredRotation : normalRotation,
                                                                                         2 * Time.deltaTime);

            yield return null;

        }

        //StartCoroutine("CloseDoor");



    }

    public IEnumerator CloseDoor()
    {
        //yield return new WaitForSeconds(2.0f);

        while (Quaternion.Angle(hinge.localRotation, Quaternion.identity) > 0)
        {
            hinge.localRotation = Quaternion.Slerp(hinge.localRotation, Quaternion.identity, 2 * Time.deltaTime);
            OtherSideOfDoor.hinge.localRotation = Quaternion.Slerp(OtherSideOfDoor.hinge.localRotation, Quaternion.identity, 2 * Time.deltaTime);

            yield return null;

        }

        openDoor = false;
        OtherSideOfDoor.isDoorOpen = false;




    }
    private MazeDoor OtherSideOfDoor
    {
        get
        {
            return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
        }
    }

    public override void Initialize(MazeCell primary, MazeCell other, MazeDirection direction)
    {
        base.Initialize(primary, other, direction);
        offMeshLink = GetComponentInChildren<OffMeshLink>();


        if (OtherSideOfDoor != null)
        {
            isMirrored = true;

            hinge.localScale = new Vector3(-1f, 1f, 1f);
            Vector3 p = hinge.localPosition;
            p.x = -p.x;
            hinge.localPosition = p;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != hinge)
            {
                child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
            }
        }



        //offMeshLink.startTransform = primary.transform;
        //offMeshLink.endTransform = other.transform;
        UpPosition = hinge.localPosition;
        MirroredUp = hinge.localPosition;

        UpPosition.y += 0.9f;
        MirroredUp.y += 0.9f;
    }


    public override void OnPlayerEntered()
    {
        //OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;

        OtherSideOfDoor.cell.room.Show();

    }

    public override void OnPlayerExited()
    {
        OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
        StartCoroutine(CloseDoor());

        OtherSideOfDoor.cell.room.Hide();
    }

    public bool isDoorOpen
    {
        get
        {
            return openDoor;
        }
        set
        {
            openDoor = value;
        }
    }







}