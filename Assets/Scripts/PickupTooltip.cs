using UnityEngine;


public class PickupTooltip : MonoBehaviour
{
    
    public SpriteRenderer tooltip;
    public Camera MainCamera;

    private Transform holdPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tooltip = GetComponent<SpriteRenderer>();
        holdPoint = transform;

    }

    private void Update()
    {
        transform.forward = MainCamera.transform.forward;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            tooltip.enabled = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            tooltip.enabled = false;

        }
    }

}
