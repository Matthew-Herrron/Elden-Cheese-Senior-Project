using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectable : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject QC;
    private questController questAccessor;

    private void Start()
    {
        questAccessor = QC.GetComponent<questController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && this.CompareTag("Reward"))
        {
            Debug.Log("Coin Collected");
            questAccessor.currentGold += 20;
            this.gameObject.SetActive(false);


        }
        if(this.tag == "Collectable")
        {
            this.gameObject.SetActive(false);
        }
        if (other.tag == "Player" && this.CompareTag("DeliveryObject")) 
        {
            Debug.Log("Object picked up");
             //this.gameObject.SetActive(false);


        }

        if (other.tag == "Deliver" && this.CompareTag("Player"))
        {
            
                Debug.Log("Object delivered");
            other.gameObject.SetActive(false);
            

        }
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && this.CompareTag("pillar"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("pillar activated");
                this.gameObject.SetActive(false);
            }


        }
    }
}
