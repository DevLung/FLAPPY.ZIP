using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordManagerScript : MonoBehaviour
{
    Discord.Discord discord;
    Discord.ActivityManager activityManager;
    Discord.Activity activity;
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
        startTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

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

        ChangeActivity(false);
    }

    void OnDisable()
    {
        discord.Dispose();
    }

    void Update()
    {
        discord.RunCallbacks();
    }

    public void ChangeActivity(bool playing)
    {
        activity.State = playing ? "Score: " + logicScript.playerScore.ToString() : "High Score: " + logicScript.highScore.ToString();
        activity.Details = playing ? detailsPlaying : detailsMenu;

        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res != Discord.Result.Ok)
            {
                Debug.LogError("Failed to connect to Discord");
            }
        });
    }
}
