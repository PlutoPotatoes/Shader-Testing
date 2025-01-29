using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject platform;

    private Transform movingTo;
    private Transform movingFrom;
    private Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //  transform.position = startPoint.position;
        movingTo = endPoint;
        movingFrom = transform;
        rb = platform.GetComponent<Rigidbody>();
        platform.transform.position = startPoint.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //movePlatform();
    }

    private void movePlatform()
    {
        //movement sticks me somewhere random and doesn't move anything
        Vector3 newPos = Vector3.MoveTowards(movingFrom.position, movingTo.position, moveSpeed*Time.deltaTime);
        platform.transform.position = newPos;
        if(Vector3.Distance(platform.transform.position, movingTo.position) == 0)
        {
            Transform hold = movingTo;
            movingTo = movingFrom;
            movingFrom = hold;
        }

    }
    


}
