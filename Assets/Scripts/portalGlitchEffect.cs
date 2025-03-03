using UnityEngine;

public class portalGlitchEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject player;
    private SphereCollider effectArea;

    private void Start()
    {
        Shader.SetGlobalFloat("_MoshStr", 0);
        effectArea = this.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            player = null;
            Shader.SetGlobalFloat("_MoshStr", 0);

        }
    }

    private void Update()
    {
        if (player)
        {
            float effectStrength = 1 - (Vector3.Distance(player.transform.position, effectArea.center)/ effectArea.radius);
            Shader.SetGlobalFloat("_MoshStr", effectStrength);
        }
    }
}
