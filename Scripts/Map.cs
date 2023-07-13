using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject tile;

    public int NoOfKingdoms = 100;

    private void Start()
    {
        int ind = 0;

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject tileGo = Instantiate(tile,new Vector3(i-5,j-5,0), Quaternion.identity) as GameObject;
                tileGo.GetComponent<Tile>().kingdomName = "Kingdom " + ind;

                ind++;
            }
        }
    }

}