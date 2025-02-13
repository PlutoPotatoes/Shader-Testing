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
    public bool carryingPlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //  transform.position = startPoint.position;

        currentTarget = startPoint;
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

        if(dist < 0.01f && !paused)
        {
            if(endPoint != null)
            {
                print(currentTarget);
                if(currentTarget == endPoint)
                {
                    currentTarget = startPoint;
                }
                else
                {
                    currentTarget = endPoint;
                }
                //currentTarget = (currentTarget == endPoint) ? startPoint : endPoint;
            }
            else
            {
                currentTarget = startPoint;
            }
            StartCoroutine(platformControl());

        }

        if (!paused)
        {
            if (carryingPlayer)
            {
                Shader.SetGlobalFloat("_VariableButton", 1);
            }
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

        }

    }
    
    IEnumerator platformControl()
    {
        paused = true;
        yield return new WaitForSeconds(pauseTime);
        paused = false;
    }


}
