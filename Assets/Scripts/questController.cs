using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;


public class questController : MonoBehaviour
{
    

    public List<Quest> QuestList = new List<Quest>();
    public string nameOfCurrentScene;


    public Canvas questCanvas;
    public Text Quest1;
    public Text Quest2;
    public Text Quest3;
    //public Text Quest4;
    //public Text Quest5;



    public Text MainQuest1;
    public Text MainQuest2;
    public Text MainQuest3;


    //public Text Quest4;

    public Text GoldAmount;

    //all quest objects

    public SceneChecker checker;

    //public Text winText;

    public int currentGold = 0;

    public int activeQuest = 0;

    private static questController s_Instance = null;


    void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);

            //Initialization code goes here[/INDENT]
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        questCanvas.enabled = false;
        QuestList.Add(new Quest(3, "Enter the portal in GreenLand and acquire the key ", "", 20));
        QuestList.Add(new Quest(3, "Unlock the gate and enter the portal in Redland", " ", 20));
        QuestList.Add(new Quest(3, "Enter Castle Redland", " ", 0));
        //QuestList.Add(new Quest(3, "Activate Quest", "Activate the three yellow pillars", 10));
        //QuestList.Add(new Quest(3, "Delivery Quest", "Deliver the green sphere to the yellow capsule", 10));

        GameObject checker = GameObject.Find("SceneChecker");

        MainQuest1.text = "Quest 1: In Progress";
        MainQuest2.text = "Quest 2: Locked";
        MainQuest3.text = "Quest 3: Locked";
 

        //quest4Button.SetActive(false);
        //quest3Button.SetActive(false);

        activateQuest(0);
         
    }
   
    // Update is called once per frame
    public void activateQuest(int index)
    {
        
        


 
        for (int i = 0; i < QuestList.Count; i++)
        {
            if(QuestList[i].status == 3)
            {
                UpdateQuestStatus(i, 2);
            }
        }

        Debug.Log(index);

        UpdateQuestStatus(index, 3);
        activeQuest = index;

       

    }


    bool enteredPortal1 = false;
    bool enteredPortal2 = false;
    bool enteredPortal3 = false;


    int[] completedQuests = new int[5];

    void Update()
    {
        GoldAmount.text = "" + currentGold;
        Quest1.text = " " + QuestList[0].stringStatus + "   Subquest 1:    " + QuestList[0].title + ", " + QuestList[0].description ;
        Quest2.text = " " + QuestList[1].stringStatus + "   Subquest 1:    " + QuestList[1].title + ", " + QuestList[1].description ;
        Quest3.text = " " + QuestList[2].stringStatus + "   Subquest 1:    " + QuestList[2].title + ", " + QuestList[2].description ;


        //game logic goes here
        if(checker.level1Completed)
        {
            UpdateQuestStatus(0, 4);
            Quest1.text = " " + QuestList[0].stringStatus + "   Subquest 1:    " + QuestList[0].title + ", " + QuestList[0].description;
            MainQuest1.text = "Quest 1: Complete";
            MainQuest2.text = "Quest 2: In Progress";

        }
        if (checker.level2Completed)
        {
            UpdateQuestStatus(1, 4);
            Quest1.text = " " + QuestList[0].stringStatus + "   Subquest 1:    " + QuestList[0].title + ", " + QuestList[0].description;
            MainQuest2.text = "Quest 2: Complete";
            MainQuest3.text = "Quest 3: In Progress";

        }
        if (checker.level3Completed)
        {
            UpdateQuestStatus(2, 4);
            Quest1.text = " " + QuestList[0].stringStatus + "   Subquest 1:    " + QuestList[0].title + ", " + QuestList[0].description;
            MainQuest3.text = "Quest 3: Complete";

        }





        //if we press the quest button
        if (Input.GetButton("Quest"))
        {
            questCanvas.enabled = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;

        }
        else
        {
            questCanvas.enabled = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        }


    }

    void UpdateQuestStatus(int index, int statusInt)
    {
        if (statusInt == 1)
        {
            QuestList[index].stringStatus = "Pending";
        }
        if (statusInt == 2)
        {
            QuestList[index].stringStatus = "Unlocked ";

        }
        if (statusInt == 3)
        {
            QuestList[index].stringStatus = "In progress";

        }
        if (statusInt == 4)
        {
            QuestList[index].stringStatus = "Completed  ";

        }
        if (statusInt == 5)
        {
            QuestList[index].stringStatus = "Done, collect reward";

        }
        if (statusInt == 6)
        {
            QuestList[index].stringStatus = "Canceled";

        }
    }
}

    
public class Quest
{
    [SerializeField]

    public int status;
    public string stringStatus;

    public string title;
    public string description;
    public int goldReward;

    public Quest(int status, string title, string description, int goldReward)
    {
        this.status = status;
        this.title = title;
        this.description = description;
        this.goldReward = goldReward;

        if (status == 1)
        {
            stringStatus = "Pending";
        }
        if (status == 2)
        {
             stringStatus = "Unlocked ";

        }
        if (status == 3)
        {
            stringStatus = "In progress";

        }
        if (status == 4)
        {
             stringStatus = "Completed  ";

        }
        if (status == 5)
        {
             stringStatus = "Done, collect reward";

        }
        if (status == 6)
        {
             stringStatus = "Canceled";

        }

    }
 
}
