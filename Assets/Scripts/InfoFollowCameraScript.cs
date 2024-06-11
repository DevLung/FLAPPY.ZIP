using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InfoFollowCameraScript : MonoBehaviour
{
    public GameObject cameraToFollow;
    public Text text;
    public float yOffset;

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            text.text = "tap to\nskip";
        }
        
        if (cameraToFollow.transform.position.y <= -23 && cameraToFollow.GetComponent<Animator>().GetBool("inCredits"))
        {
            if (text.color.a != 0.7f)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.7f);
            }
            transform.position = new Vector3(transform.position.x, cameraToFollow.transform.position.y - yOffset);
        }
    }
}
