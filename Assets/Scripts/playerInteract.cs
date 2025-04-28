using UnityEngine;
using System.Collections.Generic;

public class playerInteract : MonoBehaviour
{
    public GameObject holdPoint;
    [SerializeField] SphereCollider grabZone;

    private string[] interactTags = {"NPC", "Portal"};
    private string[] pickUpTags = { "Node", "Portal Orb"};
    private List<string> canPickUp;
    private List<Collider> grabQueue = new List<Collider>();
    private NodeGrab heldObject;
    public bool onPlat = false;

    public List<GameObject> interactQueue;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canPickUp = new List<string>(pickUpTags);
    }

    // Update is called once per frame
    void Update()
    {
        pickUp();
        interact();
    }

    private void pickUp()
    {
        if (Input.GetKeyDown("e") && heldObject == null && grabQueue.Count > 0)
        {
            Collider interacting = grabQueue[0];
            switch (interacting.tag)
            {
                case "Node":
                    grabNode(interacting);
                    break;
                case "Portal Orb":
                    grabNode(interacting);
                    break;

            }
        }else if(heldObject !=null && Input.GetKeyDown("e"))
        {
            releaseObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canPickUp.Contains(other.tag))
        {
            grabQueue.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (canPickUp.Contains(other.tag))
        {
            grabQueue.Remove(other);
        }
    }

    private void grabNode(Collider node)
    {
        if (!onPlat)
        {
            print(node);
            heldObject = node.gameObject.GetComponent<NodeGrab>();
            heldObject.grab(gameObject, holdPoint);
        }
    }

    private void releaseObject()
    {
        heldObject.release();
        heldObject = null;


    }

    public void dropSpecificObject(string type)
    {
        if(heldObject.tag == type)
        {
            print("force drop");
            releaseObject();
        }
    }

    private void interact()
    {
        if (Input.GetKeyDown("e") && interactQueue.Count > 0)
        {
            GameObject interact = interactQueue[0];
            switch (interact.tag)
            {
                case "Portal":
                    portalInteract(interact);
                    break;
                case "NPC":
                    NPCInteract(interact);
                    print("Talking");
                    break;
            }
        }
    }

    private void portalInteract(GameObject interact)
    {
        Portal portal = interact.gameObject.GetComponent<Portal>();
        transform.parent.position = portal.destination.position;
        portal.announceMessage();

    }

    private void NPCInteract(GameObject npc)
    {
        NPC npcScript = npc.GetComponent<NPC>();
        if (npcScript.isTalking)
        {
            npcScript.next();
        }
        else
        {
            npcScript.talk();
        }
    }


}
