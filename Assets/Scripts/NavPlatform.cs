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


    public bool carryingPlayer;
    public GameObject player;
    private Vector3 currentTarget;
    private Material platMaterial;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //  transform.position = startPoint.position;
        StartCoroutine(platformCreate());

        agent.updateRotation = false;
        
    }

    IEnumerator platformCreate()
    {
        platform.transform.position = startPoint.position;
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
            Shader.SetGlobalFloat("_MoshStr", 1);
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
        transform.position = startPoint.position;
        agent.SetDestination(startPoint.position);
        currentTarget = startPoint.position;
        agent.isStopped = true;

    }

    public void jetisonPlayer()
    {
        if (carryingPlayer)
        {
            player.transform.SetParent(null);
        }
        agent.isStopped = true;
    }

    public void setColor(Color nodeColor)
    {
        platform.GetComponent<Renderer>().material.color = nodeColor;
    }

}
