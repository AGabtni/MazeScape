using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{

    public Weapon item;
    [SerializeField] private float bulletSpeed = 1.0f;

    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private Transform bulletPrefab;
    [HideInInspector] public int currentAmmo;
    private int _maxAmmo;
    private float _nextFire;


    private bool _isShooting;


    // Start is called before the first frame update
    void Start()
    {
        _maxAmmo = item.maxAmount;
        //currentAmmo = item.initialAmount;

        //Update ui for weapon slot
        //if (EquipmentManager.instance.weaponSlot.onItemUsedCallback != null)
        //    EquipmentManager.instance.weaponSlot.onItemUsedCallback.Invoke();

        this.GetAmmo();
        _isShooting = false;
    }

    //Gets ammo and adjusts the leftover ammo in inventory. 
    public void GetAmmo()
    {


        Dictionary<Ammo, int> ammunition = Inventory.instance.ammunition;
        if (!(ammunition.Count > 0))
            return;

        KeyValuePair<Ammo, int> foundAmmo;
        foreach (var ammo in ammunition)
        {
            if (ammo.Key.weapon.itemName.Equals(this.item.itemName))
            {
                foundAmmo = ammo;
                Debug.Log("Found ammo for weapon " + ammo.Key.weapon.itemName);
            }


        }



        if (foundAmmo.Key != null && ammunition.ContainsKey(foundAmmo.Key))
        {
            int adjustment = foundAmmo.Value - LoadAmmo(foundAmmo.Value);
            ammunition[foundAmmo.Key] = adjustment;

        }

        //Update ui for weapon slot
        if (EquipmentManager.instance.weaponSlot.onItemUsedCallback != null)
            EquipmentManager.instance.weaponSlot.onItemUsedCallback.Invoke();
    }

    //Loads ammo into weapon . 
    //@availableQuantity : amount available of bullets in inventory
    //return value :  amount  of ammo taken
    int LoadAmmo(int availableQuantity)
    {

        if (currentAmmo >= _maxAmmo)
            return 0;

        int requiredQuantity = _maxAmmo - currentAmmo;
        if (availableQuantity > requiredQuantity)
        {

            currentAmmo += requiredQuantity;

        }
        else
        {

            currentAmmo += availableQuantity;
            requiredQuantity = availableQuantity;

        }

        return requiredQuantity;



    }


    //Shoot command. 
    public void Shoot()
    {
        if (!(currentAmmo > 0))
            return;
        ReleaseBullet();

        currentAmmo--;

        _isShooting = true;
    }
    void Update()
    {

        if (_isShooting && Time.time > _nextFire)
        {
            _nextFire = Time.time + fireRate;
            _isShooting = false;

        }

        //Debug.DrawRay(muzzleTransform.position, muzzleTransform.forward * 20.0f, Color.green);
    }

    void ReleaseBullet()
    {

        Transform bulletInstance = Instantiate(bulletPrefab, muzzleTransform.position, muzzleTransform.rotation);
        bulletInstance.GetComponent<Rigidbody>().AddForceAtPosition(muzzleTransform.forward * bulletSpeed, muzzleTransform.position, ForceMode.Impulse);
        //yield return new WaitForSeconds(shotDuration);
        //Destroy(bulletInstance.gameObject);
    }
    void OnDestroy()
    {
        Debug.Log("On destroy");

        Dictionary<Ammo, int> ammunition = Inventory.instance.ammunition;
        if (!(ammunition.Count > 0))
            return;

        KeyValuePair<Ammo, int> foundAmmo;

        foreach (var ammo in ammunition)
        {

            if (ammo.Key.weapon.itemName.Equals(item.itemName))
            {

                foundAmmo = ammo;
                //Debug.Log("Found ammo to return " + currentAmmo);

                break;
            }

        }
        if (foundAmmo.Key != null)
        {

            //Debug.Log("Returned " + currentAmmo);
            int adjustment = foundAmmo.Value + currentAmmo;
            ammunition[foundAmmo.Key] = adjustment;

        }

    }
}
