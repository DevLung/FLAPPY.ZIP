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
    int currentVersion;
    int latestVersion;

    void Start()
    {
        currentVersion = latestVersion = Int32.Parse(Application.version.Replace(".", string.Empty));

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

        if (request.result == UnityWebRequest.Result.Success)
        {
            // extract tag name/version number from response
            string rawLatestVersion = System.Text.RegularExpressions.Regex.Match(
                    request.downloadHandler.text,
                    "(?<=\"tag_name\"\\s*:\\s*\")[^\"]*(?=\")"      // (?<="tag_name"\s*:\s*")[^"]*(?=")   finds  "tag_name":  and matches the following characters between the next two "
                ).ToString();

            try
            {
                latestVersion = Int32.Parse(rawLatestVersion.Replace(".", string.Empty));
            } catch (FormatException)
            {
                return;
            }

            Debug.Log(currentVersion < latestVersion); // TODO implement new version available screen
        }
    }

    public void SetLoadingIndicatorDotsKeyframe(int keyframe)
    {
        text.text = string.Concat(Enumerable.Repeat(".", keyframe));
    }
}
