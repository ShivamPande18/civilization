using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class TaskManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject choicePanel;
    public GameObject waitPanel;

    public Image slider;
    public TMP_Text timeLeftTxt;

    float taskDuration;
    float curTask = 0;

    bool isTaskGoing = false;


    String startTime;

    private void Start()
    {
        startTime = DateTime.Now.ToString();
        string quitTime = PlayerPrefs.GetString("quitTime");

        int[] diff = CalcOfflineTime(startTime,quitTime);
        int diffInSecs = (diff[0] * 3600) + (diff[1] * 60) + (diff[2]);

        isTaskGoing = PlayerPrefs.GetInt("isTaskGoing") == 1 ? true : false;
        curTask = PlayerPrefs.GetFloat("taskLeft");
        taskDuration = PlayerPrefs.GetFloat("taskDuration");

        curTask -= diffInSecs;

        if (isTaskGoing)
        {
            waitPanel.SetActive(true);
            choicePanel.SetActive(false);
        }

        
    }

    private void Update()
    {
        if (isTaskGoing)
        {
            if (curTask > 0)
            {
                curTask -= Time.deltaTime;
                slider.fillAmount = (float)curTask / taskDuration;
                timeLeftTxt.text = (Mathf.Floor(curTask / 60f) + 1).ToString() + "mins";
            }
            else
            {
                choicePanel.SetActive(true);
                waitPanel.SetActive(false);
                gameManager.experience += 10;
                isTaskGoing = false;
            }
        }
        else
        {
            curTask = 0;
        }
    }

    public void OnTask(float taskTime)
    {
        taskDuration = taskTime;
        curTask = taskDuration;
        isTaskGoing = true;

        choicePanel.SetActive(false);
        waitPanel.SetActive(true);
    }

    int[] CalcOfflineTime(string startTime, string quitTime)
    {
        DateTime start = DateTime.Parse(startTime);
        DateTime quit = DateTime.Parse(quitTime);

        string differenceStr = (start - quit).ToString();
        string[] difference = differenceStr.Split(":");

        int hrs = int.Parse(difference[0]);
        int min = int.Parse(difference[1]);
        int sec = int.Parse(difference[2].Split(".")[0]);

        int[] timeDiff = { hrs, min, sec };

        return  timeDiff;
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("quitTime", DateTime.Now.ToString());

        int isTaskGoingInt = isTaskGoing ? 1 : 0; 
        PlayerPrefs.SetInt("isTaskGoing", isTaskGoingInt);
        PlayerPrefs.SetFloat("taskLeft", curTask);
        PlayerPrefs.SetFloat("taskDuration", taskDuration);
    }
}