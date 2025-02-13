using UnityEngine;

public class NodeData : MonoBehaviour
{
    [SerializeField] NodeData pairedNode;
    private Pedestal heldPedestal;
    private Pedestal pairedPedestal;

    public void setPedestal(Pedestal newPed)
    {
        heldPedestal = newPed;
        pairedNode.setPairedPed(newPed);

    }

    public void setPairedPed(Pedestal newPed)
    {
        pairedPedestal = newPed;
    }


    public Pedestal getPairedPedestal()
    {
        return pairedPedestal;
    }
}
