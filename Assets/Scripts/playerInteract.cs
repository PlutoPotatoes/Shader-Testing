using UnityEngine;
using System.Collections.Generic;

public class playerInteract : MonoBehaviour
{
    public GameObject holdPoint;
    [SerializeField] SphereCollider grabZone;

    private string[] interactTags = {"Node", "Portal"};
    private List<string> interactable;
    private List<Collider> inYaZone = new List<Collider>();
    private NodeGrab heldObject;
    public bool onPlat = false;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactable = new List<string>(interactTags);
    }

    // Update is called once per frame
    void Update()
    {
        pickUp();
        interact();
    }

    private void pickUp()
    {
        if (Input.GetMouseButtonDown(1) && heldObject == null && inYaZone.Count > 0)
        {
            Collider interacting = inYaZone[0];
            switch (interacting.tag)
            {
                case "Node":
                    grabNode(interacting);
                    break;

            }
        }else if(heldObject !=null && Input.GetMouseButtonDown(1))
        {
            releaseObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactable.Contains(other.tag))
        {
            inYaZone.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactable.Contains(other.tag))
        {
            inYaZone.Remove(other);
        }
    }

    private void grabNode(Collider node)
    {
        if (!onPlat)
        {
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
        if (Input.GetKeyDown("e") && inYaZone.Count > 0)
        {
            Collider interact = inYaZone[0];
            switch (interact.tag)
            {
                case "Portal":
                    portalInteract(interact);
                    break;
            }
        }
    }

    private void portalInteract(Collider interact)
    {
        Portal portal = interact.gameObject.GetComponent<Portal>();
        transform.parent.position = portal.destination.position;
        portal.announceMessage();

    }




}
