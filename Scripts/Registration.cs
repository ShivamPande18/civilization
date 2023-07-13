using UnityEngine;
using TMPro;
using SimpleJSON;

public class Registration : MonoBehaviour
{
    public TMP_InputField login_username;
    public TMP_InputField login_password;

    public TMP_InputField sign_username;
    public TMP_InputField sign_password;

    public GameObject loginScreen;
    public GameObject signUpScreen;
    public GameObject mainScreen;

    public GameManager gameManager;
    public Backend backend;




    public void OnNew()
    {
        gameManager.ChangeGameScreen("signUp");
    }

    public void OnOld()
    {
        gameManager.ChangeGameScreen("login");
    }

    public void OnLogin(bool isLogin = true)
    {
        string username = login_username.text;
        string password = login_password.text;

        string usersData = backend.GetPlayers();

        SimpleJSON.JSONNode users = SimpleJSON.JSON.Parse(usersData);
        bool isIn = false;

        for (int i = 0; i < users.Count; i++)
        {
            if (users[i]["username"].ToString() == ('"' + username + '"')  && users[i]["password"].ToString() == ('"' + password + '"'))
            {
                isIn = true;
                SetPlayer(users[i]["_id"].ToString().Split('"')[1]);
                break;
            }
        }

        if (isLogin)
        {
            if (isIn) gameManager.ChangeGameScreen("main");
            else print("failed");
        }

    }

    public void OnSignUp()
    {
        string username = sign_username.text;
        string password = sign_password.text;
        backend.SignUp(username,password);

        login_username.text = username;
        login_password.text = password;
        OnLogin(isLogin : false);
        
        gameManager.ChangeGameScreen("kingdomSelection");
    }


    void SetPlayer(string id)
    {
        gameManager._id = id;
    }
}