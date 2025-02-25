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
    public GameObject player;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //  transform.position = startPoint.position;

        StartCoroutine(platformCreate());
    }

    IEnumerator platformCreate()
    {
        yield return new WaitForSeconds(0.1f);
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

    public void setPath(Transform[] path)
    {
        //node point = 0, paired node point = 1

        startPoint = path[0];
        endPoint = path[1];
        currentTarget = endPoint;
        paused = true;
        platform.transform.position = startPoint.position;


    }

    public void jetisonPlayer()
    {
        if (carryingPlayer)
        {
            player.transform.SetParent(null);
        }
    }
}
