using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{

    //static becuase flock member will be accessing
    public GameObject FlockModel;
    public static int flockSize = 50;
    public static GameObject[] flockArray = new GameObject[flockSize];

    public GameObject portal1;
    public GameObject portal2;
    public GameObject portal3;

    public SceneChecker sceneChecker;

    //flight behaviors


    public GameObject player;
    public static Vector3 targetPos = Vector3.zero;

    public static Vector3 leaderPOS = Vector3.zero;
    public static Vector3 leaderTarg = Vector3.zero;

    private static FlockController s_Instance = null;


     
    // Start is called before the first frame update
    void Start()
    {


        //spawn flock
        for (int i = 0; i < flockSize; i++)
        {
            //now all members should spawn at a random point
            Vector3 randomWorldSpawn = new Vector3(Random.Range(-50, 50), Random.Range(0, 50), Random.Range(-50, 50));

            //Vector3 zero = Vector3.zero;
            flockArray[i] = GameObject.Instantiate(FlockModel, this.transform.position, Quaternion.identity);
        }
        //set first member to be the leader
        flockArray[0].GetComponent<FlockMember>().leader = true;
    }
    public void GivingXP(Vector3 enemyDeathPos)
    {
        //spawn flock
        for (int i = 0; i < flockSize; i++)
        {
            //now all members should spawn at a random point
            //Vector3 randomWorldSpawn = new Vector3(Random.Range(-50, 50), Random.Range(0, 50), Random.Range(-50, 50));

            //Vector3 zero = Vector3.zero;
            flockArray[i] = GameObject.Instantiate(FlockModel, enemyDeathPos, Quaternion.identity);
        }
        //set first member to be the leader
        flockArray[0].GetComponent<FlockMember>().leader = true;
    }

    // Update is called once per frame
    void Update()
    {

        targetPos = player.transform.position;
        leaderTarg = player.transform.position;
        //check flight model
        GameObject checker = GameObject.Find("PersistentObject");
        SceneChecker checker2 = checker.GetComponent<SceneChecker>();

        if (checker2.level1Completed && !checker2.level2Completed)
        {
            targetPos = portal2.transform.position;
        }
        else if (checker2.level2Completed && checker2.level1Completed)
        {
            targetPos = portal3.transform.position;
        }
        else
        {

            targetPos = player.transform.position;
            leaderTarg = player.transform.position;
        }





    }
}
