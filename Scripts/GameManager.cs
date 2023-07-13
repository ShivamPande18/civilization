using UnityEngine;
using TMPro;
using System.Collections.Generic;
using StreamChat.Core.Helpers;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, GameObject> gameScreens = new Dictionary<string, GameObject>();

    public Backend backend;
    public Kingdom kingdomSrc;
    public ChatManager chatManager;

    public GameObject loginScreen;
    public GameObject signUpScreen;
    public GameObject kingdomSelectionScreen;
    public GameObject mainScreen;
    public GameObject kingdomMembersScreen;

    public TMP_Text occupationTxt;
    public TMP_Text expTxt;

    public string _id;
    public string username;

    public int kingdom = -1;
    public int occupation;
    public int experience;

    public string gameScreen;

    Occupations occupationsClass;

    private void Start()
    {
        gameScreens.Add("login",loginScreen);
        gameScreens.Add("signUp",signUpScreen);
        gameScreens.Add("kingdomSelection",kingdomSelectionScreen);
        gameScreens.Add("main",mainScreen);
        gameScreens.Add("kingdomMembers", kingdomMembersScreen);

        gameScreen = "login";
        //gameScreen = "kingdomMembers";
        //ChangeGameScreen(gameScreen);
        occupationsClass = new Occupations();
    }


    private void Update()
    {
        if (gameScreen == "main")
        {
            occupationTxt.text = occupationsClass.ocuupations[occupation].ToString();
            expTxt.text = experience.ToString() + "/" + "100";
        }
    }

    //void CheckExpirence()
    //{
    //    if(experience>=100)
    //    {
    //        experience = 0;
    //        occupation++;
    //        backend.UpdatePlayerOccupation(_id,occupation);

    //    }
    //}

    public void OnKingdomMembers()
    {
        ChangeGameScreen("kingdomMembers");
    }


    public void ChangeGameScreen(string screen)
    {
        gameScreens[gameScreen].SetActive(false);
        gameScreen = screen;
        gameScreens[gameScreen].SetActive(true);

        if(screen == "main")
        {
            string playersData = backend.GetPlayers();

            SimpleJSON.JSONNode players = SimpleJSON.JSON.Parse(playersData);

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i]["_id"].ToString().Split('"')[1] == _id)
                {
                    username = players[i]["username"].ToString().Split('"')[1];
                    occupation = int.Parse(players[i]["occupation"]);
                    experience = int.Parse(players[i]["expirence"]);
                    kingdom = int.Parse(players[i]["kingdom"]);
                    break;
                }
            }
            chatManager.StartChatAsync().LogExceptionsOnFailed();
        }
        else if(screen == "kingdomMembers")
        {
            List<string> members = new List<string>();
            string playerData = backend.GetPlayers();

            SimpleJSON.JSONNode players = SimpleJSON.JSON.Parse(playerData);

            for (int i = 0; i < players.Count; i++)
            {
                if (int.Parse(players[i]["kingdom"]) == kingdom)
                {
                    members.Add(players[i]["username"].ToString().Split('"')[1]);
                }
            }

            kingdomSrc.ShowKingdomMembers(members);
        }
    }

    private void OnApplicationQuit()
    {
        backend.UpdatePlayerExpirence(_id, experience);
    }
}