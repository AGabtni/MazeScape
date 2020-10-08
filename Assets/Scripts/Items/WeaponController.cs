using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public Item item;
    public  int maxAmmo;
    public  int currentAmmo;
    public Transform muzzleTransform ; 
    private WaitForSeconds shotDuration = new WaitForSeconds(0.7f); 
    private float nextFire;      //Store time player allowed to fire again 
    public float fireRate = 0.25f; //How often player can fire .
    public Transform bulletPrefab ;      

    private bool isShooting ; 

    
    // Start is called before the first frame update
    void Start()
    {

        maxAmmo = item.maxAmount;
        
        this.GetAmmo();
        isShooting = false;
    }
    
    //Get the ammo from inventory
    public void GetAmmo(){

        Dictionary<Ammo,int> ammunition = Inventory.instance.ammunition;
        if (!(ammunition.Count > 0))
            return;

        KeyValuePair<Ammo,int> foundAmmo; 
        foreach (var ammo in ammunition)
        {
            if (ammo.Key.weapon.itemName.Equals(this.item.itemName))
            {
                foundAmmo = ammo;
                Debug.Log("Found ammo for weapon " + ammo.Key.weapon.itemName);
            }


        }


        
        if(foundAmmo.Key != null && ammunition.ContainsKey(foundAmmo.Key)){
            int adjustment = foundAmmo.Value - LoadAmmo(foundAmmo.Value);
            ammunition[foundAmmo.Key] = adjustment;
              
        }

        //Update ui for weapon slot
        if(EquipmentManager.instance.weaponSlot.onItemUsedCallback != null)
            EquipmentManager.instance.weaponSlot.onItemUsedCallback.Invoke();
    }
    int LoadAmmo(int availableQuantity){
        
        if(currentAmmo >= maxAmmo)
            return 0;
        
        int requiredQuantity = maxAmmo - currentAmmo;
        if(availableQuantity > requiredQuantity){
            
            currentAmmo += requiredQuantity; 

        }
        else
        {

            currentAmmo +=availableQuantity;
            requiredQuantity = availableQuantity;
            
        }   

        return requiredQuantity;
        

        
    }
    // Update is called once per frame

    public void Shoot(){
        if (!(currentAmmo > 0) )
            return;

        currentAmmo--;

        isShooting = true ; 
    }
    void Update()
    {
      
        if(isShooting && Time.time > nextFire){
            nextFire = Time.time + fireRate ;

            StartCoroutine (ShotEffect());
            isShooting = false;


        }

        Debug.DrawRay(muzzleTransform.position, muzzleTransform.forward * 20.0f, Color.green);
    }

    IEnumerator ShotEffect(){

        Transform bulletInstance = Instantiate(bulletPrefab,muzzleTransform.position,muzzleTransform.rotation);
        bulletInstance.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.forward *2, muzzleTransform.position,ForceMode.Impulse);
        yield return shotDuration;

        Destroy(bulletInstance.gameObject);
    }
    void OnDestroy()
    {
        Debug.Log("On destroy");
       
        Dictionary<Ammo,int> ammunition = Inventory.instance.ammunition;
        if (!(ammunition.Count > 0))
            return;

        KeyValuePair<Ammo,int> foundAmmo; 
        
        foreach (var ammo in ammunition)
        {
            
            if (ammo.Key.weapon.itemName.Equals(item.itemName)){
                    
                    foundAmmo = ammo;
                    Debug.Log("Found ammo to return " + currentAmmo);

                    break;
            }

        }
        if (foundAmmo.Key != null){

            Debug.Log("Returned " + currentAmmo);
            int adjustment = foundAmmo.Value + currentAmmo;
            ammunition[foundAmmo.Key] = adjustment;

        }
                
    }
}
