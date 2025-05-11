using UnityEngine;
using TMPro;
public class MenuLogic : MonoBehaviour
{
    public TextMeshProUGUI Version;
    void Start()
    {
        Version.text = Application.version;
    }
}
