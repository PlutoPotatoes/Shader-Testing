using UnityEngine;
using System.Collections.Generic;

public class playerInteract : MonoBehaviour
{
    [SerializeField] SphereCollider grabZone;

    private string[] interactTags = {"Node"};
    private List<string> interactable;
    private List<Collider> inYaZone = new List<Collider>();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactable = new List<string>(interactTags);
    }

    // Update is called once per frame
    void Update()
    {
        Interact();
    }

    private void Interact()
    {
        if (Input.GetMouseButtonDown(1) && inYaZone.Count > 0)
        {
            Collider interacting = inYaZone[0];
            switch (interacting.tag)
            {
                case "Node":
                    grabNode(interacting);
                    break;

            }
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
        print("grabbing " + node.gameObject);
    }



}
