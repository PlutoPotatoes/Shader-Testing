using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform destination;
    [SerializeField] AnnouncementManager announcer;
    [SerializeField] string message;


    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        print("You Win");
    }

    public void announceMessage()
    {
        if (message.Length > 0)
        {
            announcer.Announce(message);
        }
    }

}
