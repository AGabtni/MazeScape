using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    public Image healthFill;
    public float maxHealth = 100.0f;
    private Image _healthFill;


    [SerializeField] private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
    }
    private void Start()
    {
        _currentHealth = maxHealth;
    }


    public void ChangeHealth(int healthAmount)
    {

        _currentHealth += healthAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        UIManager.instance.UpdateHealth.Invoke(this);
    }

    public void RestoreHealth()
    {

        _currentHealth = maxHealth;



    }

    public bool IsAlive(){
        return _currentHealth > 0;
    }


}
