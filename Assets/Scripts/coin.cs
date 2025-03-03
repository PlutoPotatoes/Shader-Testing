using UnityEngine;

public class coin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private coinManager manager;

    void Start()
    {
        manager = GetComponentInParent<coinManager>();
    }

    // Update is called once per frame
    void Update()
    {
        rotate();
    }
    
    private void rotate()
    {
        transform.Rotate(new Vector3(60 * Time.deltaTime, 0, 0));

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            manager.coinGet();
            Destroy(this.gameObject);
        }
    }
}
