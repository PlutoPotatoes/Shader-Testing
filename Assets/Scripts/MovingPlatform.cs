using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float pauseTime;
    [SerializeField] GameObject platform;


    private Transform currentTarget;
    private bool paused;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //  transform.position = startPoint.position;
        currentTarget = endPoint;
        platform.transform.position = startPoint.position;
        paused = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movePlatform();
    }

    private void movePlatform()
    {
        float dist = Vector3.Distance(platform.transform.position, currentTarget.transform.position);

        if(dist == 0f)
        {
            currentTarget = (currentTarget == endPoint) ? startPoint : endPoint;
            StartCoroutine(platformControl());

        }

        if (!paused)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentTarget.position, moveSpeed*Time.deltaTime);
        }
    }
    
    IEnumerator platformControl()
    {
        paused = true;
        yield return new WaitForSeconds(pauseTime);
        paused = false;
    }


    public Vector3 exitForce()
    {
        Vector3 force;

        if (!paused)
        {
            force = Vector3.MoveTowards(platform.transform.position, currentTarget.position, 1) * moveSpeed;
        }
        else
        {
            force = Vector3.zero;

        }

        return force;
    }

}
