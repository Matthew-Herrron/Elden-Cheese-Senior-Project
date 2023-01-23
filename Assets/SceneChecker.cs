using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChecker : MonoBehaviour
{


    private static SceneChecker s_Instance = null;

    public bool level1Completed = false;
    public bool level2Completed = false;
    public bool level3Completed = false;

    public questController QuestController;

    void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);

            //Initialization code goes here[/INDENT]
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        if(scene.name == "Dungeon 1")
        {
             level1Completed = true;
            
        }
        if (scene.name == "Dungeon 2")
        {
             level2Completed = true;
        }
        if (scene.name == "Dungeon 3")
        {
             level3Completed = true;
        }
    }
}
