using System.Collections;


using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //Global
    public float gravity = -78.48f;
    
    //public Collider waterCollider;

    //UI\
    public Image badge;
    public Image badge2;
    public TMP_Text jumpAmount;

    public TMP_Text enemiesAttackedAmount;

    public GameObject Panel;
    
    int jumpCounter = 0;
    
    public Color completedColor;
    public Color activeColor;

    public Image healthbar;
    public int health;
    public static int keyNum;
    

    private Color rollOp;

    private Color cir1Op;
    public Image rollCircle;
    public Image ctrlIcon;
    private float coolDownFill;

    public Image atk1Circle;
    private float atk1coolDownFill;

    private Color cir2Op;
    private Color rClickOp;
    public Image atk2Circle;
    public Image rClickIcon;
    private float atk2coolDownFill;

    private Color cir3Op;
    private Color qOp;
    public Image atk3Circle;
    public Image qIcon;
    public Image cheesed;
    private float atk3coolDownFill;


    //character 
    private Animator animator;
    public CharacterController controller;
    public Transform plyr;
    public Vector3 velocity;
    private Vector3 moveDir;
    public float speed = 32f;
    private float sprintVar = 1f;
    public float jumpHeight = 10f;
    public float rollspeed = 80f;


    //attack
    private float canAtk1time = 0f;
    private float canAtk2time = 0f;
    private float canAtk3time = 0f;
    public float atk1CD = .3f;
    public float atk2CD = 2f;
    public float atk3CD = 6f;
    private float atk1Speed = 1f;
    private float atk2Speed = 1f;
    private float atk3Speed = 1f;
    private bool canAtk1 = true;
    private bool canAtk2 = true;
    private bool canAtk3 = true;
    private bool canBeAtt = true;
    private float f_canBeAtt = 0f;
    
    private float whenAttackOver = 0f;

    private int enemiesAttacked = 0;

    public AudioClip attack1;
    public AudioClip attack2;
    public AudioSource audioSource;

    //more character attributes
    public bool onceDead = false;
    public bool onGround;
    private bool onCloseToGround;
    public bool isFalling;
    public bool isSprinting;
    public bool isAttacking;
    public bool isAlive;
    private bool canRoll = true;
    private bool rollFrozen = false;


    //time cooldowns
    private float frozenUntil;
    private float rollCoolDown = 3f;
    private float CantRollUntil = 0f;


    //groundcheck
    [SerializeField] Transform groundCheck;
    public float groundDist = 1.2f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask enemyMask;


    //camera & movement turning
    public Transform cam;
    float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;

    

    void Start()
    {
        
        //Hide cursor
        //Cursor.visible = false;
        cheesed.gameObject.SetActive(false);
        //player anmimator
        animator = GetComponent<Animator>();

        //UI coolodown meters
        rollCircle.enabled = false;
        atk1Circle.enabled = false;
        atk2Circle.enabled = false;
        atk3Circle.enabled = false;
        

        
  

        //player heath
        PlayerPrefs.SetInt("health", 4000);

        PlayerPrefs.SetInt("jAmnt", jumpCounter);
        PlayerPrefs.SetInt("Ekilled", 0);
        isAlive = true;
        animator.SetTrigger("getUp");
        onceDead = false;
        controller.enabled = true;

        badge.color = activeColor;
        badge2.color = activeColor;
        Panel.SetActive(false);

        
    }


    void Update()
    {
        //open badge panel 
        if (Input.GetKeyDown(KeyCode.B)) {
            Panel.SetActive(true);
        }
        // close panel
        if (Input.GetKeyDown(KeyCode.C)) {
            Panel.SetActive(false);
        }

        // badge checker
        if (Input.GetKeyDown(KeyCode.Space) && onGround && !rollFrozen && isAlive && jumpCounter < 10)
        { 
            jumpCounter = jumpCounter + 1;
            PlayerPrefs.SetInt("jAmnt", jumpCounter);
        }
        jumpAmount.text = jumpCounter.ToString();
        if (PlayerPrefs.GetInt("jAmnt") >= 10)
        {
            badge.color = completedColor;
            
        }
        if(PlayerPrefs.GetInt("Ekilled") <= 5){
            enemiesAttacked = PlayerPrefs.GetInt("Ekilled");
        }
        enemiesAttackedAmount.text = enemiesAttacked.ToString();
        if (PlayerPrefs.GetInt("Ekilled") >= 5)
        {
            badge2.color = completedColor;
            
        }


        
        //walking with respect to camera
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float overallSpeed = Mathf.Sqrt((horizontal * horizontal) + (vertical * vertical));
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && !rollFrozen && isAlive)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime * sprintVar *atk1Speed * atk2Speed* atk3Speed);
        }
        //adding gravity to the player
        if (onGround && velocity.y < 0f)
            velocity.y = -10f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        
        //gorundcheck
        onGround = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        //closish groundcheck
        onCloseToGround = Physics.CheckSphere(groundCheck.position, groundDist + 3f, groundMask);
        animator.SetBool("onGroundish", onCloseToGround);


        //update player health
        health = PlayerPrefs.GetInt("health");
        //PlayerPrefs.SetInt("death", 1);
        animator.SetInteger("health", health);
        //check if dead
        if (health <= 0 && isAlive && !onceDead)
        {
            onceDead = true;
            isAlive = false;
        }
        if (!isAlive && onceDead)
        {
            StartCoroutine(RestartGame());
            
            animator.SetTrigger("die");
            onceDead = false;
            controller.enabled = false;
        }



        //Check if falling
        if (velocity.y < -15f && !onGround)
            isFalling = true;
        else
            isFalling = false;



        //sprinting
        if (Input.GetKey(KeyCode.LeftShift) && onGround && !rollFrozen && !isAttacking && isAlive)
        {
            animator.SetBool("sprint", true);
            sprintVar = 1.5f;
            isSprinting = true;
        }
        else
        {
            sprintVar = 1f;
            animator.SetBool("sprint", false);
            isSprinting = false;
        }
        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && onGround && !rollFrozen && isAlive)
        {
            animator.SetTrigger("jump");
            velocity.y = Mathf.Sqrt(jumpHeight *-2f * gravity);
        }



        //check if you can roll
        if (Time.time > CantRollUntil)
            canRoll = true;
        else
            canRoll = false;
        //input for the rolling to start
        if (Input.GetKeyDown(KeyCode.LeftControl) && canRoll && onGround && isAlive)
            Roll();  //get the 2 future times need to roll, the frozen time and the cooldown cantrolluntil time
        //check if frozen and if not it will roll the character
        if (Time.time < frozenUntil)
        {
            rollFrozen = true;
            controller.Move(moveDir * rollspeed * Time.deltaTime); //doing the actual rolling
        }
        else
            rollFrozen = false;



        //Animator settings
        if(isFalling)
            animator.SetBool("falling", true);
        else
            animator.SetBool("falling", false);
        animator.SetFloat("speed", overallSpeed);



        //UI

        //update healthBar
        //float fHealth = (float)health;
        float displayHealth = Mathf.InverseLerp(0f, 4000, health);
        healthbar.fillAmount = displayHealth;

        //roll cooldown meter
        if (CantRollUntil > Time.time)
        {
            rollCircle.enabled = true;
            rollCircle.color = cir1Op;
            ctrlIcon.color = rollOp;
        }
        else
            rollCircle.enabled = false;

        coolDownFill = Mathf.InverseLerp(0f, rollCoolDown, CantRollUntil - Time.time);
        rollCircle.fillAmount = coolDownFill;
        cir1Op = Color.white;
        cir1Op.a = coolDownFill;
        rollOp = Color.white;
        rollOp.a = 1 - coolDownFill;
        

        ////attacking cooldown meters////
        ////  Attack 1 ////
        if (canAtk1time > Time.time)
        {
 
            atk1Circle.enabled = true;
            
        }
           

        else
            atk1Circle.enabled = false;

        atk1coolDownFill = Mathf.InverseLerp(0f, atk1CD, canAtk1time - Time.time);
        atk1Circle.fillAmount = atk1coolDownFill;

        ////  Attack 2 ////
        if (canAtk2time > Time.time)
        {
             atk2Circle.enabled = true;
            atk2Circle.color = cir2Op;
            rClickIcon.color = rClickOp;
        }

        else
            atk2Circle.enabled = false;


        atk2coolDownFill = Mathf.InverseLerp(0f, atk2CD, canAtk2time - Time.time);
        atk2Circle.fillAmount = atk2coolDownFill;
        cir2Op = Color.white;
        cir2Op.a = atk2coolDownFill;
        rClickOp = Color.white;
        rClickOp.a = 1 - atk2coolDownFill;

        ////  Attack 3 ////
        if (canAtk3time > Time.time)
        {

            atk3Circle.enabled = true;
            atk3Circle.color = cir3Op;
            qIcon.color = qOp;
        }
        else
            atk3Circle.enabled = false;

        atk3coolDownFill = Mathf.InverseLerp(0f, atk3CD, canAtk3time - Time.time);
        atk3Circle.fillAmount = atk3coolDownFill;
        cir3Op = Color.white;
        cir3Op.a = atk3coolDownFill;
        qOp = Color.white;
        qOp.a = 1 - atk3coolDownFill;
        


        //Attacking
        Attack(); //method call to do animations

        ////  Attack 1 quick main attack////
        if (Time.time > whenAttackOver)
        {
            isAttacking = false;
            PlayerPrefs.SetInt("attacking", 0);
        }
        else
            isAttacking = true;


        if (canAtk1time < Time.time)
        {
            canAtk1 = true;
            atk1Speed = 1f;
        }
        else{
            canAtk1 = false;
            if (isAttacking)
                atk1Speed = .40f;
            else
                atk1Speed = 1f;
        }

        ////  Attack 2 secondary atack medium cooldown////
        if (canAtk2time < Time.time)
        {
            canAtk2 = true;
            atk2Speed = 1f;
        }
        else{
            canAtk2 = false;
            if (isAttacking)
                atk2Speed = .30f;
            else
                atk2Speed = 1f;
        }

        ////  Attack 3 long one, knock enemy up ////
        if (canAtk3time < Time.time)
        {
            canAtk3 = true;
            atk3Speed = 1f;
        }
        else{
            canAtk3 = false;
            if (isAttacking)
                atk3Speed = .20f;
            else
                atk3Speed = 1f;
        }


        //////testing//////
        if (Input.GetKeyDown(KeyCode.X))
        {
            health -= 10;
            //Debug.Log(" taking damage ");
            PlayerPrefs.SetInt("health", health);
            animator.SetTrigger("small damage");
        }
        if (Input.GetKeyDown(KeyCode.Z) && !isAlive)
        {
            health = 100;
            PlayerPrefs.SetInt("health", health);
            animator.SetTrigger("getUp");
            onceDead = false;
            isAlive = true;
            controller.enabled = true;
        }
        
    }
    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(4f);
        cheesed.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("OpenWorld");
    }
    private void Attack()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !rollFrozen && canAtk1 && !isFalling && !isAttacking && isAlive){
            //start attack anim
            animator.SetTrigger("atk1");
            audioSource.PlayOneShot(attack2);
            PlayerPrefs.SetInt("attacking", 1);//for player combat set the attacking to 1
            //cooldown for attack
            canAtk1time = Time.time + atk1CD;
            //timewhen attack animation should be over
            whenAttackOver = Time.time + .5f;
        }
        if (Input.GetKey(KeyCode.Mouse1) && !rollFrozen && canAtk2 && !isFalling && !isAttacking && isAlive){
            animator.SetTrigger("atk2");
            canAtk2time = Time.time + atk2CD;
            audioSource.PlayOneShot(attack1);
            PlayerPrefs.SetInt("attacking", 2);

            whenAttackOver = Time.time + .5f;
        }
        if (Input.GetKey(KeyCode.Q) && !rollFrozen && canAtk3 && !isFalling && !isAttacking && isAlive){
            animator.SetTrigger("atk3");
            canAtk3time = Time.time + atk3CD;
            audioSource.PlayOneShot(attack1);
            PlayerPrefs.SetInt("attacking", 3);

            whenAttackOver = Time.time + 1f;
        }

    }

    private void Roll()
    {
        //start roll animation
        animator.SetTrigger("roll");
        //cooldown for roll
        CantRollUntil = Time.time + rollCoolDown;
        //what actually controlls the rolling
        frozenUntil = Time.time + .75f;
        //set can roll to false 
        canRoll = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "enemyAttackBox" )
        {
           
            
            //Debug.Log("enemy attacked You!!");
            float ran = Random.Range(20f, 80f);
            int ranInt = (int)ran;
            health = health - ranInt;
           
            PlayerPrefs.SetInt("health", health);
            
            if(f_canBeAtt < Time.time){
                animator.SetTrigger("small damage");
                f_canBeAtt = Time.time + 2f;
            }
        }
        if (other.tag == "s_enemyAttackBox" )
        {
           
            
            //Debug.Log("enemy attacked You!!");
            float ran = Random.Range(8f, 20f);
            int ranInt = (int)ran;
            health = health - ranInt;
           
            PlayerPrefs.SetInt("health", health);
            if(f_canBeAtt < Time.time){
                animator.SetTrigger("small damage");
                f_canBeAtt = Time.time + 1f;
            }
            //animator.SetTrigger("small damage");
        }
    
        

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "enemyAttackBox")
        {
            //Debug.Log("enemy is sttacking you You!!");
            health -= 4;
            PlayerPrefs.SetInt("health", health);
        }
        if (other.tag == "s_enemyAttackBox")
        {
            health -= 1;
            PlayerPrefs.SetInt("health", health);
        }
        
    }

    //Testing
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("he's far away now");
    }
   
    public void UpdateKeyValue(int keyNum) {
        //Debug.Log("key number = 1");
    }
}

