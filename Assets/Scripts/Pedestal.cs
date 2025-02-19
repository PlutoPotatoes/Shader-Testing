using UnityEngine;

public class Pedestal : MonoBehaviour
{
    [SerializeField] Transform holdPoint;
    [SerializeField] GameObject movingPlatformPrefab;
    public Transform platformDockPoint;

    private GameObject node;
    private NodeGrab nodeScript;
    private NodeData nodeData;
    private GameObject platformInstance;
    public bool hasPlatform;


    void Start()
    {
        platformInstance = null;
        hasPlatform = false;
        //tryCreateRoute();

    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Node" && !node)
        {
            node = other.gameObject;
            nodeScript = other.GetComponent<NodeGrab>();
            other.GetComponent<boxSpin>().onPedestal = true;
            nodeData = other.GetComponent<NodeData>();
            nodeScript.snapThreshold = 3f;
            nodeScript.onPedestal = true;
            nodeScript.pedHold = holdPoint;
            nodeData.setPedestal(this);
            tryCreateRoute();


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == node)
        {
            nodeData.setPedestal(null);
            nodeScript.snapThreshold = 1f;
            nodeScript.onPedestal = false;
            boxSpin spin = other.GetComponent<boxSpin>();
            spin.onPedestal = false;
            spin.resetRotation();
            tryDestroyPlatform();
            nodeScript.pedHold = null;
            node = null;
            nodeScript = null;
            nodeData = null;


        }
    }

    private void tryCreateRoute()
    {
        if (nodeData.hasCompletePath() && !hasPlatform)
        {
            print("Create platform");
            //update pedestal bools for the pair
            nodeData.getPairedPedestal().hasPlatform = true;

            hasPlatform = true;

            //instantiate platform
            platformInstance = Instantiate(movingPlatformPrefab, platformDockPoint, true);
            nodeData.getPairedPedestal().platformInstance = this.platformInstance;
            MovingPlatform platformScript = platformInstance.GetComponent<MovingPlatform>();
            platformScript.setPath(nodeData.getPath());


        }
    }

    private void tryDestroyPlatform()
    {
        print("destroy platforms");
        if (hasPlatform)
        {

            nodeData.getPairedPedestal().hasPlatform = false;
            nodeData.getPairedPedestal().platformInstance = null;
            hasPlatform = false;
            Destroy(platformInstance);
        }

    }
}
