using UnityEngine;

public class objectGrabbable : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private const float holdStr = 0.9f;
    private Transform holdPoint;
    private bool isHeld;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        heldCycle();
    }

    private void heldCycle()
    {
        if (isHeld && transform.position != holdPoint.transform.position)
        {
            //object is following grab point
            //but is moving the grab point empty with it??
            //idk man but its close
            Vector3 dir =  Vector3.Lerp(transform.position, holdPoint.transform.position, holdStr);
            rb.MovePosition(dir);
        }
    }
        
    public void grab(GameObject player, GameObject playerHoldPoint)
    {
        rb.useGravity = false;
        transform.position = playerHoldPoint.transform.position;
        holdPoint = playerHoldPoint.transform;
        isHeld = true;
        
    }

    public void release()
    {
        holdPoint = null;
        isHeld = false;
        rb.useGravity = true;

    }
}
