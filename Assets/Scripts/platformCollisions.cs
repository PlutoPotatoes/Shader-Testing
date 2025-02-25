using UnityEngine;

public class platformCollisions : MonoBehaviour
{
    private MovingPlatform platformScript;
    private void Start()
    {
        platformScript = GetComponentInParent<MovingPlatform>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            platformScript.player = other.gameObject;
            platformScript.carryingPlayer = true;
            other.transform.SetParent(this.transform, true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            platformScript.player = null;
            // trying to get platform momentum to effect player on platform exit
            //other.GetComponent<Rigidbody>().AddForce(platformScript.exitForce(), ForceMode.Impulse);
            platformScript.carryingPlayer = false;
            other.transform.SetParent(null, true);
        }
    }
}
