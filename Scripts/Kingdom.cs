using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Kingdom : MonoBehaviour
{
    public Transform kingdomMembersContainer;
    public GameObject kingdomMemberUI;

    public GameManager gameManager;
    public Backend backend;

    public void OnKingdomSelect(int kingdom)
    {
        backend.UpdatePlayerKingdom(gameManager._id, kingdom);
        gameManager.ChangeGameScreen("main");
    }

    public void ShowKingdomMembers(List<string> members)
    {
        for (int i = 0; i < members.Count; i++)
        {
            GameObject member =  Instantiate(kingdomMemberUI) as GameObject;
            member.transform.GetChild(0).GetComponent<TMP_Text>().text = members[i];
            member.transform.SetParent(kingdomMembersContainer, false);
        }
    }
}