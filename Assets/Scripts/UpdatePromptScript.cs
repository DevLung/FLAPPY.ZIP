using UnityEngine;

public class UpdatePromptScript : MonoBehaviour
{
    public StartupScript startupScript;
    public Font robotoMono;
    public Font robotoMonoBold;
    const string LatestGameReleaseUrl = "https://github.com/DevLung/FLAPPY.ZIP/releases/latest";
    const int BoxWidth = 500;
    const int BoxHeight = 250;
    const int BoxPadding = 30;
    const int LabelWidth = BoxWidth;
    const int LabelHeight = 50;
    const int ButtonWidth = 200;
    const int ButtonHeight = 50;

    private void OnGUI()
    {
        GUIStyle boxStyle = new GUIStyle("box");
        boxStyle.font = robotoMonoBold;
        boxStyle.fontSize = 30;
        boxStyle.normal.textColor = new Color(0.7f, 0, 0);
        boxStyle.padding = new RectOffset(BoxPadding, BoxPadding, BoxPadding, BoxPadding);

        GUIStyle labelStyle = new GUIStyle("label");
        labelStyle.font = robotoMono;
        labelStyle.fontSize = 16;
        labelStyle.alignment = TextAnchor.UpperCenter;



        GUI.BeginGroup(
            new Rect(
                (Screen.width - BoxWidth) / 2,          // horizontally centered
                (Screen.height - BoxHeight) * 0.7f,     // vertically centered on lower 70% of screen
                BoxWidth,
                BoxHeight
            )
        );


        GUI.Box(
            new Rect(
                0,
                0,
                BoxWidth,
                BoxHeight
            ),
            "UPDATE AVAILABLE",
            boxStyle
        );


        GUI.Label(
            new Rect(
                0,
                (BoxHeight - LabelHeight) * 0.45f,
                LabelWidth,
                LabelHeight
            ),
            "current version: " + Application.version + "\n latest version: " + startupScript.rawLatestVersion,
            labelStyle
        );


        if (
            GUI.Button(
                new Rect(
                    BoxPadding,
                    BoxHeight - ButtonHeight - BoxPadding,
                    ButtonWidth,
                    ButtonHeight
                ),
                "DOWNLOAD"
            )
        )
        {
            Application.OpenURL(LatestGameReleaseUrl);
            Application.Quit();
        }


        if (
            GUI.Button(
                new Rect(
                    BoxWidth - ButtonWidth - BoxPadding,
                    BoxHeight - ButtonHeight - BoxPadding,
                    ButtonWidth,
                    ButtonHeight
                ),
                "PLAY ANYWAY"
            )
        )
        {
            startupScript.sceneLoader.allowSceneActivation = true;
            gameObject.SetActive(false);
        }


        GUI.EndGroup();
    }
}
