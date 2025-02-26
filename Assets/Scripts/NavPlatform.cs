using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavPlatform : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float pauseTime;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject platform;
    [SerializeField] LineRenderer line;

    public bool carryingPlayer;
    public GameObject player;
    private Vector3 currentTarget;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //  transform.position = startPoint.position;
        StartCoroutine(platformCreate());
        agent.updateRotation = false;
        
    }

    IEnumerator platformCreate()
    {
        yield return new WaitForSeconds(0.1f);
        agent.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        movePlatform();
    }

    private void movePlatform()
    {

        if(agent.remainingDistance <= 0.01f && !agent.isStopped)
        {

            if(currentTarget == endPoint.position)
            {
                currentTarget = startPoint.position;
            }
            else
            {
                currentTarget = endPoint.position;
            }

            agent.SetDestination(currentTarget);
            
            StartCoroutine(platformPause());
            
        }

        if (!agent.isStopped && carryingPlayer)
        {
            Shader.SetGlobalFloat("_VariableButton", 1);
        }
    }

    IEnumerator platformPause()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(pauseTime);
        agent.isStopped = false;
    }

    public void setPath(Transform[] path)
    {
        //node point = 0, paired node point = 1

        startPoint = path[0];
        endPoint = path[1];
        agent.SetDestination(startPoint.position);
        currentTarget = startPoint.position;
        transform.position = startPoint.position;
        setLine();
        StartCoroutine(platformPause());

        



    }

    public void jetisonPlayer()
    {
        if (carryingPlayer)
        {
            player.transform.SetParent(null);
        }
    }

    private void setLine()
    {
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        //line.positionCount = agent.path.corners.Length;
        //line.SetPositions(agent.path.corners);
        line.positionCount = 2;
        line.SetPosition(0, startPoint.position);
        line.SetPosition(0, endPoint.position);

        print(line.positionCount);

        
    }
}
