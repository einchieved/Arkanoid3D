using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public GameObject multiBall;
    public GameObject extendedPaddle;

    public int hits;
    private int points;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        points = hits;
        material = new Material(Shader.Find("Unlit/Color"));
        gameObject.GetComponent<Renderer>().material = material;
        UpdateMaterial();
    }

    public int Damage()
    {
        hits--;
        if (hits > 0)
        {
            UpdateMaterial();
            return -1;
        }
        else
        {
            DropItem();
            return points;
        }
    }

    private void UpdateMaterial()
    {
        Color color;
        switch(hits)
        {
            case 5:
                color = Color.red;
                break;
            case 4:
                color = Color.yellow;
                break;
            case 3:
                color = Color.green;
                break;
            case 2:
                color = Color.blue;
                break;
            case 1:
                color = Color.magenta;
                break;
            default:
                color = Color.white;
                break;
        }
        material.color = color;
    }

    private void DropItem()
    {
        int nmbr = Random.Range(points * 2, 21);
        if (nmbr == 18)
        {
            Instantiate(multiBall, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), multiBall.transform.rotation);
        }
        else if (nmbr == 19)
        {
            Instantiate(extendedPaddle, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), extendedPaddle.transform.rotation);
        }
    }
}
