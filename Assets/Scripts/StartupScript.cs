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
    public Animator animator;
    public GameObject updatePrompt;
    const string latestVersionInfoUrl = "https://api.github.com/repos/DevLung/FLAPPY.ZIP/releases/latest";
    int currentVersion;
    int latestVersion;
    bool newVersionAvailable = false;
    string loadingIndicatorInfoText = "starting game";
    public AsyncOperation sceneLoader;
    bool activateMainSceneIfLoaded = true;

    async void Start()
    {
        currentVersion = latestVersion = Int32.Parse(Application.version.Replace(".", string.Empty));

        loadingIndicatorInfoText = "checking for updates";
        await CheckForUpdates();
        if (newVersionAvailable)
        {
            updatePrompt.SetActive(true);
            activateMainSceneIfLoaded = false;
        }

        loadingIndicatorInfoText = "loading scene";
        StartCoroutine(LoadMainScene());
    }

    async Task CheckForUpdates()
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

            newVersionAvailable = currentVersion < latestVersion;
        }
    }

    IEnumerator LoadMainScene()
    {
        sceneLoader = SceneManager.LoadSceneAsync("MainGame");
        sceneLoader.allowSceneActivation = false;

        bool sceneReady = false;
        while (!sceneLoader.isDone)
        {
            // run once as soon as scene is loaded
            if (sceneLoader.progress >= 0.9f && !sceneReady)
            {
                sceneReady = true;

                // stop animation
                animator.SetBool("done", true);
                loadingIndicatorInfoText = "Done!";
                // remove any loading indicator dots
                SetLoadingIndicatorDotsKeyframe(0);

                sceneLoader.allowSceneActivation = activateMainSceneIfLoaded;
            }

            yield return null;
        }
    }

    public void SetLoadingIndicatorDotsKeyframe(int keyframe)
    {
        text.text = loadingIndicatorInfoText + string.Concat(Enumerable.Repeat(".", keyframe));
    }
}
