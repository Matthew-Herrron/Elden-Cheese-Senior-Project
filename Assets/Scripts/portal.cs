using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class portal : MonoBehaviour
{

    public int dungeonIndex;
    public bool entered1 = false;
    public bool entered2 = false;
    public bool entered3 = false;



    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && dungeonIndex == 1)
        {
            entered1 = true;
            SceneManager.LoadScene("Dungeon 1");
        }
        if (other.tag == "Player" && dungeonIndex == 2)
        {
            entered2 = true;
            SceneManager.LoadScene("Dungeon 2");
        }
        if (other.tag == "Player" && dungeonIndex == 3)
        {
            entered3 = true;
            SceneManager.LoadScene("Dungeon 3");
        }

        if (other.tag == "Player" && dungeonIndex == 4)
        {
            SceneManager.LoadScene("OpenWorld");
        }
        if (other.tag == "Player" && dungeonIndex == 5)
        {
            SceneManager.LoadScene("YouWin");
        }
    }
}
