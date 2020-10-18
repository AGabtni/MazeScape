using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    // Start is called before the first frame update

    public float damageModifier = 0f;
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


        //Debug.Log("Bullet hit " + LayerMask.LayerToName(collision.gameObject.layer));
        if(collision.gameObject.layer == _weaponMask)
            return;

        if (collision.gameObject.layer != _playerMask)
        {
            Debug.Log("Bullet hit : " + collision.gameObject.name);
            if (collision.gameObject.GetComponent<Health>())
            {
                collision.gameObject.GetComponent<Health>().ChangeHealth(-20);
            }
            Destroy(this.gameObject);
        }
    }

    IEnumerator OnBulletCollided()
    {

        yield return new WaitForSeconds(1.0f);
    }
}
