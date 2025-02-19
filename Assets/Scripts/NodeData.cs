using UnityEngine;

public class NodeData : MonoBehaviour
{
    [SerializeField] NodeData pairedNode;
    private Pedestal heldPedestal;
    private Pedestal pairedPedestal;
    private Transform pedPoint;
    private Transform pairedPedPoint;


    public void setPedestal(Pedestal ped)
    {
        if (ped)
        {
            heldPedestal = ped;
            pairedNode.setPairedPed(ped);
            pedPoint = ped.platformDockPoint;
        }
        else
        {
            heldPedestal = null;
            pairedNode.setPairedPed(null);
            pedPoint = null;
        }

    }

    public void setPairedPed(Pedestal ped)
    {
        if (ped)
        {
            pairedPedestal = ped;
            pairedPedPoint = ped.platformDockPoint;
        }
        else
        {
            pairedPedestal = null;
            pairedPedPoint = null;
        }
    }


    public Pedestal getPairedPedestal()
    {
        return pairedPedestal;
    }

    public Transform[] getPath()
    {
        Transform[] path = { pedPoint, pairedPedPoint};
        return path;
    }

    public bool hasCompletePath()
    {

        if (heldPedestal != null && pairedPedestal != null)
            return true;

        return false;
    }
    
}
