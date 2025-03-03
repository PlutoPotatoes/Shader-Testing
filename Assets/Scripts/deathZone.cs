using UnityEngine;

public class deathZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<StarterAssets.FirstPersonController>().respawn();
        }
        else if(other.transform.parent == null)
        {
            other.transform.SetPositionAndRotation(respawnPoint.position, Quaternion.identity);
        }
    }
}
