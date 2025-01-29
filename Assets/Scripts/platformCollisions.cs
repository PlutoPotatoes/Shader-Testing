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
            other.transform.SetParent(this.transform, true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // trying to get platform momentum to effect player on platform exit
            //other.GetComponent<Rigidbody>().AddForce(platformScript.exitForce(), ForceMode.Impulse);
            other.transform.SetParent(null, true);
        }
    }
}
