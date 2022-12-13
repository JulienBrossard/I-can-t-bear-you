using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlWoldMap : Item, IInteractable
{
    [SerializeField] private string lvlToLoad;
    

    public void Interact(Vector3 sourcePos)
    {
        SceneManager.LoadScene(lvlToLoad);
    }
}
