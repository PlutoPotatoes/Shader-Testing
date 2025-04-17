using UnityEngine;

public class NodeGrab : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask excludedLayers;
    [SerializeField] LayerMask alwaysIncludeLayers;
    [SerializeField] Collider grabbableCollider;

    private const float holdStr = 0.9f;
    private Transform holdPoint;
    public bool isHeld;
    public bool onPedestal;
    private bool groundCollision = false;
    public float snapThreshold;
    public Transform pedHold;
    public Material mat;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grabbableCollider.excludeLayers &= (1 << excludedLayers);
        mat = GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        heldCycle();
    }

    private void heldCycle()
    {
        if (isHeld && transform.position != holdPoint.transform.position)
        {
            float dist = Vector3.Distance(transform.position, holdPoint.transform.position);
            if (!groundCollision)
            {
                if (pedHold != null)
                {
                    rb.position = pedHold.position;
                }
                else
                {
                    Vector3 dir = Vector3.Lerp(transform.position, holdPoint.transform.position, 10 * Time.deltaTime);
                    rb.MovePosition(dir);
                }
            }
            
            
            if ( dist >= snapThreshold)
            {
                rb.position = holdPoint.transform.position;
            }
        }
        else if(pedHold !=null)
        {
            pedGrab();
        }
    }
        
    public void grab(GameObject player, GameObject playerHoldPoint)
    {
        rb.useGravity = false;
        transform.position = playerHoldPoint.transform.position;
        holdPoint = playerHoldPoint.transform;
        isHeld = true;
        pedHold = null;
        grabbableCollider.excludeLayers = excludedLayers;
        transform.rotation = Quaternion.identity;
        rb.constraints = RigidbodyConstraints.FreezeAll;


    }

    private void pedGrab()
    {
        holdPoint = pedHold;
        Vector3 dir = Vector3.Lerp(transform.position, holdPoint.transform.position, 8 * Time.deltaTime);
        rb.MovePosition(dir);
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

    }

    public void release()
    {

        if (onPedestal)
        {
            pedGrab();

        }
        else
        {
            holdPoint = null;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
        grabbableCollider.excludeLayers &= (1 << excludedLayers);
        isHeld = false;


    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Ground")
        {
            groundCollision = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Ground")
        {
            groundCollision = false;
        }

    }
    
}
