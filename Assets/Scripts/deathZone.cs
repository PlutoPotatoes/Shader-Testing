using UnityEngine;

public class deathZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<StarterAssets.FirstPersonController>().respawn();
        }
        else if(other.tag == "Node" && other.transform.parent == null)
        {
                other.GetComponent<NodeData>().respawn();
        }
    }
}
