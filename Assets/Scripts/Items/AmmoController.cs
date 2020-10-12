using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    // Start is called before the first frame update

    private Collider _boxCollider;
    private int _playerMask;
    private int _weaponMask;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _playerMask = LayerMask.NameToLayer("Player");
        _weaponMask = LayerMask.NameToLayer("Weapon");
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter(Collision collision)
    {


        Debug.Log("Bullet hit " + LayerMask.LayerToName(collision.gameObject.layer));
        if (collision.gameObject.layer != _playerMask && collision.gameObject.layer != _weaponMask)
            Destroy(this.gameObject);
        
    }

    IEnumerator OnBulletCollided()
    {

        yield return new WaitForSeconds(1.0f);
    }
}
