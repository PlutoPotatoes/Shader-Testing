using UnityEngine;

public class coinManager : MonoBehaviour
{
    [SerializeField] GameObject portal;
    [SerializeField] AnnouncementManager announcer;
    private int totalCoins;
    private int collectedCoins;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalCoins = transform.childCount;
        portal.SetActive(false);
        collectedCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void coinGet()
    {
        collectedCoins += 1;
        if(collectedCoins == totalCoins)
        {
            portal.SetActive(true);
        }

        if(totalCoins == collectedCoins)
        {
            announcer.Announce("The Portal is Open");
        }
        else
        {
            announcer.Announce(totalCoins - collectedCoins + " Coins Left");
        }
    }
}
