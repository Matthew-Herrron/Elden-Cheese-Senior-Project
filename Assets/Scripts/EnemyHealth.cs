using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyHealth : FlockController
{
    public float health;
    public float maxHealth;

    private int enKill;
    private float nextTimeToBeAttacked;
    public bool canBeAttacked;

    public bool dead;
    public GameObject healthBarUI;
    public Slider slider;
    Animator animator2;
    public Transform target;
    NavMeshAgent agent;
    private Rigidbody enemyRb;
    private FlockController XP;
    public Collider attackRange;
    private BoxCollider bCollider;
    private CapsuleCollider CapCollider;


    // Start is called before the first frame update
    float CalculateHealth()
    {
        return health / maxHealth;
    }

    void Awake(){
        dead = false;
    }
    void Start()
    {
        
        animator2 = GetComponent<Animator>();
       // target = playerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        bCollider = GetComponent<BoxCollider>();
        CapCollider = GetComponent<CapsuleCollider>();
        health = maxHealth;
        slider.value = CalculateHealth();
        animator2.SetBool("IsDead", false);
        canBeAttacked = true;
        attackRange.enabled = true;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = CalculateHealth();

        if (health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        if (health <= 0)
        {
            //dead
            enKill = PlayerPrefs.GetInt("Ekilled");
            animator2.SetBool("IsDead", true);
            attackRange.enabled = false;
            bCollider.enabled = false;
            CapCollider.enabled = false;
            StartCoroutine(Die());
            if(!dead){
                enKill++;
            }
            dead = true;
            PlayerPrefs.SetInt("Ekilled", enKill);

        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }


        if(nextTimeToBeAttacked < Time.time)
        {
            canBeAttacked = true;
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "attackBox" && PlayerPrefs.GetInt("attacking") == 1 && canBeAttacked)
        {
            Debug.Log("enemy attacked");
            float ran = Random.Range(8f, 12f);
            health -= ran;
            canBeAttacked = false;
            nextTimeToBeAttacked = Time.time + .4f;
        }
        if (other.tag == "attackBox" && PlayerPrefs.GetInt("attacking") == 2 && canBeAttacked)
        {
            Debug.Log("enemy attacked");
            float ran = Random.Range(14f, 18f);
            health -= ran;
            canBeAttacked = false;
            nextTimeToBeAttacked = Time.time + .4f;
        }
        if (other.tag == "attackBox" && PlayerPrefs.GetInt("attacking") == 3 && canBeAttacked)
        {
            Debug.Log("enemy attacked");
            float ran = Random.Range(40f, 50f);
            health -= ran;
            canBeAttacked = false;
            nextTimeToBeAttacked = Time.time + .4f;
            //agent.enabled = false;
            //enemyRb.AddForce(Vector3.up * 1000, ForceMode.Impulse);
        }

    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}