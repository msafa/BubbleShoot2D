using UnityEngine;
using System.Collections.Generic;

public class HexGridLayout : MonoBehaviour
{
    public static int counter;
    public static float time;
    public GameObject bubblePrefab;
    public GameObject bubbleEmptyPrefab;

    public static List<Transform> emptyList;
    public static GameObject lastBullet;

    public Vector3 v3Center = Vector3.zero;
    public float objDistance = 0.75f;  // Distance apart objects are apart on a row
    public int rows = 12;
    public int cols = 12;

    private float rowDist;
    private float rowStart;
    private Vector3 v3Pos;

    void Start()
    {
        counter = 0;
        rowDist = Mathf.Sqrt((objDistance * objDistance) - ((objDistance * objDistance * 0.25f)));
        rowStart = -(cols * objDistance / 2.0f - 0.25f * objDistance);
        v3Pos = new Vector3(rowStart, rows * rowDist / 2.0f, 0.0f);

        for (int i = 0; i < rows; i++)
        {

            if ((i % 2) == 1)
                v3Pos.x -= objDistance / 2.0f;

            for (int j = 0; j < cols; j++)
            {

                Vector3 spherePosition = v3Pos + v3Center;
                GameObject newSphere;
                if (i > 3) // for empty 
                {
                    newSphere = Instantiate(bubbleEmptyPrefab, spherePosition, Quaternion.identity);
                }
                else
                {
                    newSphere = Instantiate(bubblePrefab, spherePosition, Quaternion.identity);
                }
                newSphere.transform.parent = GameObject.FindWithTag("ParentofSphere").transform;

                v3Pos.x += objDistance;
            }

            v3Pos.x = rowStart;
            v3Pos.y -= rowDist;

        }

        // collecting all empty object to the list
        emptyList = new List<Transform>();
        GameObject[] argoEmpty = GameObject.FindGameObjectsWithTag("Empty");
        foreach (GameObject go in argoEmpty)
        {
            emptyList.Add(go.transform);
        }

    }


    void Update()
    {
        if(counter > 2 && (Time.time - time > 0.25f))
        {
            counter = 0;
            //Destroy controllers
            ClearControllers();
            //swap all of k to empty object
            SwapKtoEmpty();
        }else if (counter > 0 && (Time.time - time > 0.25f))
        {
            counter = 0;
            //Destroy controllers
            ClearControllers();
            //swap all of k to empty object
            ClearKNames();
        }
    }

    void ClearKNames()
    {
        GameObject[] controllers = GameObject.FindGameObjectsWithTag("Bubble");
        foreach (GameObject cGo in controllers)
        {
            if (cGo.name.Equals("k"))
            {
                cGo.name = "Bubble(Clone)";
            }
        }
    }

    void ClearControllers()
    {
        GameObject[] controllers = GameObject.FindGameObjectsWithTag("Controller");
        foreach (GameObject cGo in controllers)
        {
            Destroy(cGo);
        }
    }

    void SwapKtoEmpty()
    {
        GameObject[] k = GameObject.FindGameObjectsWithTag("Bubble");
        for (int i = 0; i < k.Length; i++)
        {
            if (k[i].name.Equals("k"))
            {
                k[i].tag = "Empty";
                k[i].GetComponent<Renderer>().enabled = false;
                k[i].name = "Bubble(Clone)";
                emptyList.Add(k[i].transform);
                
            }
            else if (k[i].transform.Equals(lastBullet.transform))
            {
                k[i].tag = "Empty";
                k[i].GetComponent<Renderer>().enabled = false;
                k[i].name = "Bubble(Clone)";
                emptyList.Add(k[i].transform);
            }
        }



    }

}