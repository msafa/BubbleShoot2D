using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletScript : MonoBehaviour {


    public GameObject controllerPrefab;
    public GameObject bulletPrefab;
    public Material[] material;
    public int bulletSpeed = 6;

    bool isFired;
    Transform tr;

    Vector3 mousePos;

    // Use this for initialization
    void Start () {

        isFired = false;

        GetComponent<Renderer>().material = material[Random.Range(0, 4)];
        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);

    }
	
	// Update is called once per frame
	void Update () {

        if (isFired)
        {
            Move();
        }

        if (Input.GetMouseButtonDown(0))
        {
            isFired = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, 0);

            Vector3 diff = mousePos - transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        }

    }

    void Move()
    {
        transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {

        if(other.tag.Equals("Bubble") || other.tag.Equals("Border"))
        {

            if (transform.position.y < -2.6f)
            {
                Debug.Log("GameOver");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if(isFired) // after shoot the function must return 1 closest transform
            {
                TransformBulletToBubble();
                isFired = false;
            }
                
        }else if (other.tag.Equals("Wall"))
        {
            Vector3 v = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(v.x, v.y, -v.z);
        }
    }


    Transform ClosestTransform(Transform currentPos)
    {
        float dist = Mathf.Infinity;
        Vector3 curPos = currentPos.position;
        Transform closestOne = HexGridLayout.emptyList[0];
        foreach (Transform trans in HexGridLayout.emptyList)
        {
            float tempDist = (trans.position - curPos).sqrMagnitude;
            if (tempDist < dist)
            {
                closestOne = trans;
                dist = tempDist;
            }
        }
        return closestOne;
    }

    //find that where should place bullet.. and make that empty place full
    void TransformBulletToBubble()
    {

        tr = ClosestTransform(gameObject.transform);
        //Debug.Log("TransformBulletToBubble - closest pos : " + tr.position);
        HexGridLayout.emptyList.Remove(tr);
        tr.tag = "Bubble";
        tr.GetComponent<Renderer>().enabled = true;
        tr.GetComponent<Renderer>().material = gameObject.GetComponent<Renderer>().material;
        tr.GetComponent<SphereCollider>().enabled = true;
        tr.GetComponent<SphereCollider>().isTrigger = true;
        HexGridLayout.lastBullet = tr.gameObject;

        GameObject newController = Instantiate(controllerPrefab, tr.transform.position, Quaternion.identity);
        newController.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color;

        GameObject newBullet = Instantiate(bulletPrefab, new Vector3(0, -5.5f, 0), Quaternion.identity);
        newBullet.name = "Bullet";


        Destroy(gameObject);
        HexGridLayout.time = Time.time;

        
    }
}
