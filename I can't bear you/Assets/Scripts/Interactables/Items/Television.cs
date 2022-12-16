using UnityEngine;

public class Television : AttractiveItem, ISmashable, IInteractable
{
    [Header("Television Materials")]
    private Material[] tempMatList;
    [SerializeField] private MeshRenderer tvScreenMR;
    [SerializeField] private Material tvOff;
    [SerializeField] private Material tvOn;

    private void Update()
    {
        if (functioning)
        {
            Attracted();
        }
    }

    public void Smash()
    {
        if (charged) return;
        Electrocute();
    }

    [ContextMenu("Switch")]
    public override void Switch()
    {
        functioning = !functioning;
        tempMatList = tvScreenMR.materials;
        if (!functioning)
        {
            tempMatList[1] = tvOff;
            StopAttracted();
        }
        else
        { 
            tempMatList[1] = tvOn;
        }
        tvScreenMR.materials = tempMatList;
    }
    
    public void Interact(Vector3 sourcePos)
    {
        Debug.Log("Interacting with " + gameObject.name);
        Switch();
    }
    
}
