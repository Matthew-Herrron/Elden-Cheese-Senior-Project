using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMember : MonoBehaviour
{


    public float speed = .5f;
    public float rotationSpeed = 4.0f;
    public float seperation= 3.5f;
    public bool leader = false;
    public bool turn;
    public bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        turn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //check if members are in bounds, world size set to 50
        
        //check distance from this objects position to world origin, if greater than, we need to turn around
        flightBehaviour();


        transform.Translate(0, 0, Time.deltaTime * speed);
    }
   
    void flightBehaviour()
    {
        //default flight model
        //get reference to all flock members
        GameObject[] flockMembers = FlockController.flockArray;
        


        Vector3 targetPos = Vector3.zero;
        
        //center of the flock
        Vector3 center = Vector3.zero;
        Vector3 avoidance = new Vector3(1f, 1f, 1f);
        targetPos = FlockController.targetPos;
        if(leader)
        {
            targetPos = FlockController.leaderTarg;
        }
            
        
        //https://learn.unity.com/tutorial/flocking#5e0bbe73edbc2a0021fb5811
        //https://www.intel.com/content/www/us/en/developer/articles/case-study/fish-flocking-with-unitysimulating-the-behavior-of-object-moving-with-ai.html
        //flock movement code here
        float distance;
        int flockSize = 0;
        float overallSpeed = .05f;

       
            foreach (GameObject unit in flockMembers)
            {
                if (unit != this.gameObject)
                {
                    distance = Vector3.Distance(unit.transform.position, this.transform.position);
                    if (distance <= seperation)
                    {
                        center += unit.transform.position;
                        flockSize++;
                        if (distance < 1.0f)
                        {
                            avoidance = avoidance + (this.transform.position - unit.transform.position);
                        }

                        FlockMember flock2 = unit.GetComponent<FlockMember>();
                        overallSpeed = overallSpeed + flock2.speed;
                    }
                }
            }
        
        

        //check flock size
        //get center
        //speed / speed
        //change heading 
        if (flockSize > 0) 
        {
            center = center / flockSize + (targetPos - this.transform.position);
            speed = overallSpeed / flockSize;
            Vector3 heading = (center + avoidance) - transform.position;
            
            if (heading != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(heading), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
