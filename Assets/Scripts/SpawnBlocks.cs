using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    public GameObject oneHitBlock;
    public GameObject twoHitBlock;
    public GameObject threeHitBlock;
    public GameObject fourHitBlock;
    public GameObject fiveHitBlock;
    public GameObject floor;

    private List<GameObject> allObjects;
    private int round = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private int PlaceBlocks()
    {
        // set start position to spawn blocks
        float xPosition = transform.position.x + 1.5f;
        float zPosition = transform.position.z - 2;
        // set the number of blocks per row
        int numberX = (int)(floor.transform.localScale.x * 10) / (int) oneHitBlock.transform.localScale.x;
        // set the number of rows, dependent on the current round
        int numberZ = Mathf.Clamp(round, 1, 6) + 1;
        // decides witch types of blocks are spawned, dependent on the current row
        int nmbr = Mathf.Clamp(round, 1, 4) + 1;
        allObjects = new List<GameObject>();
        for (int i = 1; i <= numberZ; i++)
        {
            for (int j = 0; j < numberX; j++)
            {
                allObjects.Add(InstantiateBlock(nmbr, new Vector3(xPosition, 0.5f, zPosition)));
                xPosition += oneHitBlock.transform.localScale.x;
            }
            xPosition = transform.position.x + 1.5f;
            zPosition -= 2;
        }
        return allObjects.Count;
    }

    private GameObject InstantiateBlock(int nmbr, Vector3 vector)
    {
        int a = Random.Range(0, nmbr);
        switch(a)
        {
            case 1:
                return Instantiate(twoHitBlock, vector, Quaternion.identity);
            case 2:
                return Instantiate(threeHitBlock, vector, Quaternion.identity);
            case 3:
                return Instantiate(fourHitBlock, vector, Quaternion.identity);
            case 4:
                return Instantiate(fiveHitBlock, vector, Quaternion.identity);
            default:
                return Instantiate(oneHitBlock, vector, Quaternion.identity);
        }
    }

    public int NextRound()
    {
        round++;
        return PlaceBlocks();
    }

    public int FirstRound()
    {
        if (allObjects != null)
        {
            allObjects.ForEach(item =>
            {
                if (item != null)
                {
                    Destroy(item);
                }
            });
        }
        round = 1;
        return PlaceBlocks();
    }
}
