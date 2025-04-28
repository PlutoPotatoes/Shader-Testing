using UnityEngine;

public class PortalPedestal : MonoBehaviour
{

    [SerializeField] Transform holdPoint;
    [SerializeField] GameObject portal;
    [SerializeField] bool isLocked = true;
    [SerializeField] GameObject forceField;




    void Start()
    {
        portal.SetActive(false);
        forceField.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Portal Orb")
        {
            GameObject orb = other.gameObject;
            NodeGrab nodeScript = orb.GetComponent<NodeGrab>();
            portal.SetActive(true);

            nodeScript.snapThreshold = 3f;
            nodeScript.onPedestal = true;
            nodeScript.pedHold = holdPoint;
            nodeScript.release();
            nodeScript.isLocked = isLocked;
            forceField.SetActive(true);


        }
    }

}
