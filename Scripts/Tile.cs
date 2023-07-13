using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string kingdomName;

    private void OnMouseOver()
    {
        print(kingdomName);
    }
}