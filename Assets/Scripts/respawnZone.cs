using UnityEngine;

public class respawnZone : MonoBehaviour
{
    [SerializeField] Transform respawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<StarterAssets.FirstPersonController>().respawnPoint = respawn; 
        }
        else if(other.tag == "Node")
        {
            other.GetComponent<NodeData>().respawnPoint = respawn;
        }
        else if (other.tag == "Portal Orb")
        {
            other.GetComponent<OrbRespawn>().respawnPoint = respawn;
        }
    }
}
