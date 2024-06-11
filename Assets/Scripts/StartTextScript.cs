using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StartTextScript : MonoBehaviour
{
    public Text text;
    
    void Start()
    {
        if (Application.isMobilePlatform || EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android /*so changes also appear in editor*/)
        {
            text.text = "tap to start";
        }
    }
}
