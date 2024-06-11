using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StartTextScript : MonoBehaviour
{
    public Text text;
    
    void Start()
    {
        if (Application.isMobilePlatform)
        {
            text.text = "tap to start";
        }
    }
}
