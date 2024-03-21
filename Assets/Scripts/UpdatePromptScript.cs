using UnityEngine;

public class UpdatePromptScript : MonoBehaviour
{
    public StartupScript startupScript;
    const string latestGameReleaseUrl = "https://github.com/DevLung/FLAPPY.ZIP/releases/latest";

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 600, 400), "New Update Available");

        if (GUI.Button(new Rect(15, 20, 100, 25), "download"))
        {
            Application.OpenURL(latestGameReleaseUrl);
        }
        if (GUI.Button(new Rect(15, 45, 100, 25), "ignore"))
        {
            startupScript.sceneLoader.allowSceneActivation = true;
            gameObject.SetActive(false);
        }
    }
}
