using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] AnnouncementManager announcer;



    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player")
        {
            announcer.Announce("You Win");
            //other.transform.SetPositionAndRotation(destination.position, Quaternion.identity);

        }
    }

}
