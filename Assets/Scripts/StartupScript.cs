using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartupScript : MonoBehaviour
{
    public Text text;
    const string latestVersionInfoUrl = "https://api.github.com/repos/DevLung/FLAPPY.ZIP/releases/latest";

    void Start()
    {
        CheckForUpdates();
        SceneManager.LoadSceneAsync("MainGame");
    }

    private async void CheckForUpdates()
    {
        UnityWebRequest request = UnityWebRequest.Get(latestVersionInfoUrl);
        request.SetRequestHeader("accept", "application/vnd.github+json");
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }
    }

    public void SetLoadingIndicatorDotsKeyframe(int keyframe)
    {
        text.text = string.Concat(Enumerable.Repeat(".", keyframe));
    }
}
