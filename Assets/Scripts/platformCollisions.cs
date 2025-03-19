using UnityEngine;

public class platformCollisions : MonoBehaviour
{
    private NavPlatform platformScript;
    private void Start()
    {
        platformScript = GetComponentInParent<NavPlatform>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            platformScript.player = other.gameObject;
            platformScript.carryingPlayer = true;
            other.transform.SetParent(platformScript.transform, true);
            other.gameObject.GetComponentInChildren<playerInteract>().onPlat = true;


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            platformScript.player = null;
            platformScript.carryingPlayer = false;
            other.transform.SetParent(null, true);
            other.gameObject.GetComponentInChildren<playerInteract>().onPlat = false;


        }
    }

}
