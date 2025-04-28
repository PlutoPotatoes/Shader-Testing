using UnityEngine;

public class OrbRespawn : MonoBehaviour
{
    public Transform respawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -16) respawn();
    }

    public void respawn()
    {
        transform.position = respawnPoint.position;
    }
}
