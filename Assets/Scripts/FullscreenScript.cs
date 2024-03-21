using UnityEngine;

public class FullscreenScript : MonoBehaviour
{
    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
