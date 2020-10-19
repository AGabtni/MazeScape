using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class HomeMenu : MonoBehaviour
{


    [Header("Main Canvas UI")]
    public Transform mainScreen;
    public Button playButton;
    public Button settingsButton;
    public Button exitButton;

    [Header("Settings canvas UI")]
    public Transform settingsScreen;
    public Button backButton;

    [Header("Load Scene UI")]
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI progressText;

    [Header("Sound FX")]
    public AudioClip UI_Click;
    public AudioClip Button_Hover;
    private AudioSource audioSource { get { return GetComponent<AudioSource>(); } }


    private Camera _mainCamera;



    private void Start()
    {
        _mainCamera = Camera.main;
        gameObject.AddComponent<AudioSource>();

        playButton.onClick.AddListener(() => OnPlay());
        settingsButton.onClick.AddListener(() => OnSettingsClick());
        backButton.onClick.AddListener(() => OnBackClick());
    }

    void OnPlay()
    {
        StartCoroutine(LoadAsynchronously(1));

    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {        
        
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
    }
    void OnSettingsClick()
    {
        //_mainCamera.transform.LookAt(settingsScreen);
        //StartCoroutine("RotateCamera", settingsScreen);
    }
    void OnBackClick()
    {
        //StartCoroutine("RotateCamera2", mainScreen);
    }

    IEnumerator RotateCamera(Transform target)
    {
        //StopAllCoroutines();
        Debug.Log(Vector3.Angle(target.position, _mainCamera.transform.position));

        while (Vector3.Angle(_mainCamera.transform.position, target.position) > 0)
        {
            _mainCamera.transform.rotation = Quaternion.Lerp(_mainCamera.transform.rotation, target.rotation, Time.deltaTime * 2);
            yield return null;
        }

    }
    IEnumerator RotateCamera2(Transform target)
    {
        //StopAllCoroutines();
        Debug.Log(Vector3.Angle(_mainCamera.transform.position, target.position));

        while (Vector3.Angle(target.position, _mainCamera.transform.position) > 0)
        {
            _mainCamera.transform.rotation = Quaternion.Lerp(_mainCamera.transform.rotation, target.rotation, Time.deltaTime * 2);
            yield return null;
        }

    }
    
    
    void PlaySound(AudioClip soundClip)
    {

        audioSource.PlayOneShot(soundClip);

    }

}
