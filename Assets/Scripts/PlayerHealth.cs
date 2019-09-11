using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    private Image HealthFill;
    public float maxHealth = 100.0f;
    private MeshDestroy meshController;


    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        HealthFill = GameObject.Find("HealthFill").GetComponent<Image>();
        meshController = GetComponentInChildren<MeshDestroy>();

    }

    private void LateUpdate()
    {
        if(currentHealth <= 0 && GetComponent<PlayerMovement>().enabled)
        {

            meshController.DestroyMesh();
            GetComponent<PlayerMovement>().enabled = false;
        }
    }


    public void ChangeHealth(int healthAmount)
    {
        if (HealthFill == null)
            return;

        currentHealth += healthAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthFill.fillAmount = currentHealth / maxHealth;

    }

    public void RestoreHealth()
    {
        if (HealthFill == null)
            return;

        currentHealth = maxHealth;
        HealthFill.fillAmount = 1;



    }



}
