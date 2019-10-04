using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Transform mainCanvas;
    public Transform settingsCanvas;
    Camera mainCamera;

    [Header("Interactive buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button exitButton;


    [Header("Sound FX")]
    public AudioClip UI_Click;
    public AudioClip Button_Hover;
    private AudioSource audioSource { get { return GetComponent<AudioSource>(); } }
    

    


    private void Start()
    {
        mainCamera = Camera.main;
        gameObject.AddComponent<AudioSource>();

        playButton.onClick.AddListener(() => Play());   
        
    }

    public void LookAtSettings()
    {

        //mainCamera.transform.LookAt(settingsCanvas);
        StartCoroutine("rotateCamera");
    }

    IEnumerator rotateCamera()
    {

        while (Vector3.Angle(mainCamera.transform.position, settingsCanvas.position)>0) {
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, settingsCanvas.rotation, Time.deltaTime * 2);
            yield return new WaitForEndOfFrame();
        }

    }


    void Play()
    {
        SceneManager.LoadScene(1);
    } 
    void PlaySound(AudioClip soundClip)
    {

        audioSource.PlayOneShot(soundClip);
        
    }
}
