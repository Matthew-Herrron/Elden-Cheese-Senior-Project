using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterKiller : MonoBehaviour
{

    public Collider water;
    // Start is called before the first frame update
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            PlayerPrefs.SetInt("health", 0);

    }
}
