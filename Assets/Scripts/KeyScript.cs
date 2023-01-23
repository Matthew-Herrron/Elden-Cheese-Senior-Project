using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public static int keyNum;
    
    private PlayerController playerController;

    void Awake() {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { 
            keyNum = keyNum + 1;
            Debug.Log("you got the key!");
            Destroy(gameObject);
        }
        playerController.UpdateKeyValue(keyNum);

    }
}