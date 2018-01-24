using UnityEngine;

public class BubbleScript : MonoBehaviour {

    // Use this for initialization
    public GameObject controllerPrefab;
    public Material[] material;
    Renderer rend;

    void Start () {

        if (gameObject.tag.Equals("Bubble"))
        {
            rend = GetComponent<Renderer>();
            int asd = Random.Range(0, 4);
            rend.material = material[asd];
        }

    }

    void OnTriggerEnter(Collider other)
    {

        // avoiding the repetition.
        if (!other.tag.Equals("Border") && !gameObject.tag.Equals("Empty") && !transform.name.Equals("k") && other.GetComponent<SphereCollider>().radius == 1)
        {
            //checking the colors of objects
            if (((int)(other.GetComponent<Renderer>().material.color.r * 10) == (int)(gameObject.GetComponent<Renderer>().material.color.r * 10)) &&
                ((int)(other.GetComponent<Renderer>().material.color.g * 10) == (int)(gameObject.GetComponent<Renderer>().material.color.g * 10)) &&
                ((int)(other.GetComponent<Renderer>().material.color.b * 10) == (int)(gameObject.GetComponent<Renderer>().material.color.b * 10)))

            {
                // the objects will change to empty which is named k.
                transform.name = "k";
                HexGridLayout.counter++;

                GameObject newCon = null;

                if (other.transform.position != transform.position)
                {
                    newCon = Instantiate(controllerPrefab, transform.position, Quaternion.identity);
                    //assigning the same color to controller.
                    newCon.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color;
                }


            }
        }   
    }

}
