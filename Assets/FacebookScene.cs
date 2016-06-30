using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FacebookScene : MonoBehaviour {
    
    public ProfilePicture profilePicture;
    public GameObject MyAvatar;
    public GameObject FB_Username;

    public GameObject ButtonCompite;
    public GameObject ButtonPlay;

   // public Text hiscore;
    private List<object> scoresList = null;

    [SerializeField] FriendButton friendButton;

    private Dictionary<string, string> profile = null;

    void Start()
    {
        MyAvatar.SetActive(false);
    }
    public void Init()
    {
        Invoke("checkToStart", 0.5f);
    }
    void checkToStart()
    {
        if (!Data.Instance.userData.isPlayerDataLogged())
        {
            Init();
            return;
        }

        MyAvatar.SetActive(true);

        FB_Username.GetComponent<Text>().text = Data.Instance.userData.username;
      //  hiscore.text = Data.Instance.userData.hiscore.ToString();
        profilePicture.SetPicture(Data.Instance.userData.facebookId);

    }
}
