using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeScript : MonoBehaviour
{
    private KeyScript playerContr;

    public GameObject otherGate;
    
    void Awake() {
        playerContr = GameObject.FindObjectOfType<KeyScript>();
        otherGate.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { 
            
            Debug.Log("gate opened!");
            if(KeyScript.keyNum == 1) {
                Destroy(gameObject);
                otherGate.SetActive(true);
            }
            
        }
    }
}