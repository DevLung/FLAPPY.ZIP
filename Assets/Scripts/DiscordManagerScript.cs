using UnityEngine;
using Discord;

public class DiscordManagerScript : MonoBehaviour
{
    Discord.Discord discord;
    ActivityManager activityManager;
    Activity activity;
    public LogicScript logicScript;
    public long applicationId;
    public string largeImage = "icon";
    public string largeImageText = "FLAPPY.ZIP";
    public string detailsPlaying = "Playing";
    public string detailsMenu = "Main Menu";
    private long startTime;

    void Start()
    {
        discord = new Discord.Discord(applicationId, (ulong)Discord.CreateFlags.NoRequireDiscord);
        activityManager = discord.GetActivityManager();
        startTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - (long)(Time.realtimeSinceStartupAsDouble * 1000 /* to milliseconds */);

        activity = new Discord.Activity
        {
            Assets =
            {
                LargeImage = largeImage,
                LargeText = largeImageText
            },
            Timestamps =
            {
                Start = startTime
            }
        };

        UpdateActivity(false);
    }

    void OnDisable()
    {
        discord.Dispose();
    }

    void Update()
    {
        discord.RunCallbacks();
    }

    public void UpdateActivity(bool playing)
    {
        activity.State = playing ? "Score: " + logicScript.playerScore.ToString() : "High Score: " + PlayerPrefs.GetInt("high score").ToString();
        activity.Details = playing ? detailsPlaying : detailsMenu;

        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res != Result.Ok)
            {
                Debug.LogError("Failed to update Discord activity");
            }
        });
    }
}
