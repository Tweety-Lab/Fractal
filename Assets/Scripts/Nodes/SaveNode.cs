using UnityEngine;

public class SaveNode : MonoBehaviour
{
    [SerializeField] SavingSystem SavingSystem;
    public void AutoSave()
    {
        SavingSystem.AutoSave();
    }
}
