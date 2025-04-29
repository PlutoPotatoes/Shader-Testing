using UnityEngine;


public class PickupTooltip : MonoBehaviour
{
    
    public SpriteRenderer tooltip;
    public Camera MainCamera;
    public bool showTooltip;

    private Transform holdPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tooltip = GetComponent<SpriteRenderer>();
        holdPoint = transform;
        tooltip.enabled = false;
        showTooltip = true;

    }

    private void Update()
    {
        transform.forward = MainCamera.transform.forward;
        if (!showTooltip)
        {
            tooltip.enabled = false;

        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            tooltip.enabled = showTooltip;

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
