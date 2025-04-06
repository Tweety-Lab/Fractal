using UnityEngine;
using UnityEngine.Events;

public class logic_gate_node : MonoBehaviour
{
    public bool State;
    public UnityEvent OnTrue_Trigger_Output;
    public void Execute()
    {
        if (State)
        {
            OnTrue_Trigger_Output.Invoke();
        }
    }
    public void ToggleState(bool state)
    {
        State = state;
    }
}
