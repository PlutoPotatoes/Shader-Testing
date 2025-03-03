using UnityEngine;
using TMPro;
using System.Collections;

public class AnnouncementManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText;

    private float textFade = 1f;
    private bool messagePlaying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (messagePlaying)
        {
            updateAnnouncement();
        }
    }

    public void Announce(string message)
    {
        if (!messagePlaying)
        {
            mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 1);
            mainText.text = message;
            messagePlaying = true;
        }
    }
    private void updateAnnouncement()
    {
        
        if (mainText.color.a > 0)
        {
            mainText.color = new Color (mainText.color.r, mainText.color.g, mainText.color.b, mainText.color.a-(textFade*Time.deltaTime));
        }
        else
        {
            messagePlaying = false;
        }
    }
    
}
