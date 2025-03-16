using UnityEngine;

public class FullscreenScript : MonoBehaviour
{
    public GameObject fullscreenToggle;

    public void Start()
    {
        if (Application.isMobilePlatform)
        {
            fullscreenToggle.SetActive(false);
        }
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
