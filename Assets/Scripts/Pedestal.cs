using UnityEngine;

public class Pedestal : MonoBehaviour
{
    [SerializeField] Transform holdPoint;
    private GameObject node;
    private NodeGrab nodeScript;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Node")
        {
            node = other.gameObject;
            nodeScript = other.GetComponent<NodeGrab>();
            nodeScript.snapThreshold = 3f;
            nodeScript.onPedestal = true;
            nodeScript.pedHold = holdPoint;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == node)
        {
            nodeScript.snapThreshold = 1f;
            nodeScript.onPedestal = false;
            nodeScript.pedHold = null;
            node = null;
            nodeScript = null;

        }
    }
}
