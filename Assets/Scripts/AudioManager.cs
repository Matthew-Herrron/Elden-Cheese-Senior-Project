using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audio;

    public AudioClip audioClipNormal;
    public AudioClip audioClipBattle;



    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "OpenWorld")
        {
            // Do something...
            audio.PlayOneShot(audioClipNormal);
        }
        else
        {
            audio.PlayOneShot(audioClipBattle);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
