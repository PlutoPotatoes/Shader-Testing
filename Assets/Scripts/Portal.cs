using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform destination;
    [SerializeField] AnnouncementManager announcer;
    [SerializeField] GameObject tooltip;
    [SerializeField] string message;


    private void Start()
    {
        tooltip.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player")
        {
            tooltip.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            tooltip.SetActive(false);

        }
    }

    public void announceMessage()
    {
        if (message.Length > 0)
        {
            announcer.Announce(message);
        }
    }

}
