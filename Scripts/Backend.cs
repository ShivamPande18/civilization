using UnityEngine;
using System.Net;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System.IO;

public class Backend : MonoBehaviour
{

    const string API_URL = "http://localhost:5000/";

    public string GetPlayers()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL + "get");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return reader.ReadToEnd();
    }

    public void SignUp(string username, string password)
    {
        print("started");
        StartCoroutine(API_SignUp(username,password));
    }

    public void UpdatePlayerOccupation(string id, int occupation)
    {
        StartCoroutine(API_UpdatePlayerOccupation(id, occupation));
    }

    public void UpdatePlayerExpirence(string id, int expirence)
    {
        StartCoroutine(API_UpdatePlayerExpirence(id, expirence));
    }

    public void UpdatePlayerKingdom(string id, int kingdom)
    {
        StartCoroutine(API_UpdatePlayerKingdom(id, kingdom));
    }

    public string GetKingdoms()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL + "getKingdoms");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return reader.ReadToEnd();
    }

    IEnumerator API_SignUp(string username, string password)
    {

        NewPlayer player = new NewPlayer(username, password, 0,-1,0);

        var jsonData = JsonConvert.SerializeObject(player);

        using UnityWebRequest webRequest = new UnityWebRequest(API_URL + "save", "POST");
        webRequest.SetRequestHeader("Content-type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(jsonData);
        webRequest.uploadHandler = new UploadHandlerRaw(rawData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        print(webRequest.result);
    }

    IEnumerator API_UpdatePlayerOccupation(string id, int occupation)
    {

        UpdatedPlayer player = new UpdatedPlayer(id, occupation);

        var jsonData = JsonConvert.SerializeObject(player);

        using UnityWebRequest webRequest = new UnityWebRequest(API_URL + "updatePlayerOccupation", "POST");
        webRequest.SetRequestHeader("Content-type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(jsonData);
        webRequest.uploadHandler = new UploadHandlerRaw(rawData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        print(webRequest.result);
    }

    IEnumerator API_UpdatePlayerExpirence(string id, int expirence)
    {

        UpdatedPlayerExpirence player = new UpdatedPlayerExpirence(id, expirence);

        var jsonData = JsonConvert.SerializeObject(player);

        using UnityWebRequest webRequest = new UnityWebRequest(API_URL + "updatePlayerExpirence", "POST");
        webRequest.SetRequestHeader("Content-type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(jsonData);
        webRequest.uploadHandler = new UploadHandlerRaw(rawData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        print(webRequest.result);
    }

    IEnumerator API_UpdatePlayerKingdom(string id, int kingdom)
    {

        UpdatedPlayerKingdom player = new UpdatedPlayerKingdom(id, kingdom);

        var jsonData = JsonConvert.SerializeObject(player);

        using UnityWebRequest webRequest = new UnityWebRequest(API_URL + "updatePlayerKingdom", "POST");
        webRequest.SetRequestHeader("Content-type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(jsonData);
        webRequest.uploadHandler = new UploadHandlerRaw(rawData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        print(webRequest.result);
    }
}


public class NewPlayer
{
    public string username;
    public string password;
    public int occupation;
    public int expirence;
    public int kingdom;

    public NewPlayer(string _username, string _password, int _occupation, int _kingdom, int _expirence)
    {
        username = _username;
        password = _password;
        occupation = _occupation;
        expirence = _expirence;
        kingdom = _kingdom;
    }
}

public class UpdatedPlayer
{
    public string _id;
    public int occupation;

    public UpdatedPlayer(string id, int _occupation)
    {
        _id = id;
        occupation = _occupation;
    }
}


public class UpdatedPlayerExpirence
{
    public string _id;
    public int expirence;

    public UpdatedPlayerExpirence(string id, int _expirence)
    {
        _id = id;
        expirence = _expirence;
    }
}

public class UpdatedPlayerKingdom
{
    public string _id;
    public int kingdom;

    public UpdatedPlayerKingdom(string id, int _kingdom)
    {
        _id = id;
        kingdom = _kingdom;
    }
}
