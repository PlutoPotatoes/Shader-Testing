using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour
{

    [SerializeField] GameObject tooltip;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject[] dialogues;

    private int dialogueState;
    public bool isTalking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tooltip.SetActive(false);
        dialogueBox.SetActive(false);
        dialogueState = 0;
        isTalking = false;

        foreach(GameObject i in dialogues)
        {
            i.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void talk()
    {
        isTalking = true;
        tooltip.SetActive(false);
        StartCoroutine(dialogue(0));
    }

    public void next()
    {
        StopAllCoroutines();
        if (dialogueState == dialogues.Length - 1)
        {
            dialogueBox.SetActive(false);
            dialogues[dialogueState].SetActive(false);
            isTalking = false;
            dialogueState = 0;
        }
        else
        {
            dialogues[dialogueState].SetActive(false);
            dialogueState++;
            StartCoroutine(dialogue(dialogueState));
        }
        
    }

    IEnumerator dialogue(int dialogueNo)
    {
        int i = dialogueNo;
        dialogueBox.SetActive(true);
        dialogues[i].SetActive(true);
        yield return new WaitForSeconds(3); 
        if (i == dialogueState)
        {
            dialogueBox.SetActive(false);
            dialogues[i].SetActive(false);
            dialogueState = 0;
            isTalking = false;


        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            tooltip.SetActive(true);
            other.GetComponentInChildren<playerInteract>().interactQueue.Add(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            tooltip.SetActive(false);
            other.GetComponentInChildren<playerInteract>().interactQueue.Remove(this.gameObject);
            dialogueBox.SetActive(false);


        }
    }
}
