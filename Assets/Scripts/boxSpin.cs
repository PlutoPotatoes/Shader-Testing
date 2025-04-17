using UnityEngine;

public class boxSpin : MonoBehaviour
{
    public bool onPedestal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onPedestal)
        {
            transform.Rotate(new Vector3(0, 1, 0));
        }
    }

    public void resetRotation()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

}
