using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIEvent : UnityEvent<Health>
{
}

public class HUDEvent : UnityEvent<bool>
{
}
public class UIManager : MonoBehaviour
{

    [SerializeField] private Image crossHair;
    [SerializeField] private GameObject gameMenu;



    [HideInInspector] public UIEvent UpdateHealth;
    [HideInInspector] public HUDEvent UpdateHUD;


    #region Singleton
    public static UIManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one UI manager instance ");
            return;
        }

        instance = this;
        ToggleGameMenu(false);
    }
    #endregion

    void Start()
    {
        if (UpdateHealth == null)
            UpdateHealth = new UIEvent();
        if (UpdateHUD == null)
            UpdateHUD = new HUDEvent();

        UpdateHealth.AddListener(UpdateHealthBar);
        UpdateHUD.AddListener(UpdateUI);
    }


    public void UpdateHealthBar(Health health)
    {
        if (health.healthFill == null)
            return;

        float healthAmount = health.currentHealth / health.maxHealth;
        health.healthFill.fillAmount = healthAmount;

    }

    public void UpdateUI(bool active)
    {
        Color crossHairColor = crossHair.color;
        crossHairColor.a = active ? 1f : 0f;
        crossHair.color = crossHairColor;
    }

    //On game menu toggled
    public void ToggleGameMenu(bool active)
    {
        gameMenu.SetActive(active);
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = active;
    }

    //For the pause button event
    public void ToggleGameMenu()
    {
        gameMenu.SetActive(!gameMenu.activeSelf);
        Cursor.lockState = gameMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = gameMenu.activeSelf;
    }

    //On quit button clicked
    public void OnQuit()
    {
        SceneManager.LoadSceneAsync(0);
    }
}