using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingChecker : MonoBehaviour
{


    public Collider playerAttackRadius;
    void Awake()
    {
        playerAttackRadius.enabled = false;
    }

    void Update()
    {
        if(PlayerPrefs.GetInt("attacking") == 1 || PlayerPrefs.GetInt("attacking") == 2 || PlayerPrefs.GetInt("attacking") == 3)
            playerAttackRadius.enabled = true;
        else
            playerAttackRadius.enabled = false;

    }

}
