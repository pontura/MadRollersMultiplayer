using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FBHolder : MonoBehaviour
{
    public Texture2D picture;
   // public string username;
    private Dictionary<string, string> profile = null;
    void Start()
    {
        SocialEvents.FBLogin += FBLogin;
    }
    void FBLogin()
    {
        Login();
    }
    public void Init()
    {
        FB.Init(SetInit, OnHideUnity);
    }
    void OnHideUnity(bool isLoading)
    {
        Debug.Log("Facebook logging in...");
    }
    void SetInit()
    {
        Debug.Log("Facebook init done");
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook logged in");
            isLogged();
        }
    }
    public void Login()
    {
        FB.Login("email,publish_actions", AuthCalback);
    }
    void isLogged()
    {
       // FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, DealWithProfilePicture);
        FB.API("/me?fields=id,name", Facebook.HttpMethod.GET, DealWithUserName);
    }
    void DealWithUserName(FBResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("Problemas al traer el nombre");
            FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, DealWithUserName);
            return;
        }
        profile = Util.DeserializeJSONProfile(result.Text);
       
        string username = profile["name"];
        Data.Instance.userData.username = username;

        string facebookID = profile["id"];
        SocialEvents.OnFacebookUserLoaded(facebookID, username);

       if (Data.Instance.userData.userId > 0)
       {
           Debug.Log("Usuario existía en la base, agrega facebookID nomas");
           Social.Instance.dataController.AddFacebookIdToExistingAccount(Data.Instance.GetComponent<UserData>().userId, facebookID);
       }
       else
       {
           Debug.Log("Usuario Nuevo : No existe un usuario en la base con ese Facebook id");
           Social.Instance.dataController.CheckIfFacebookIdExists(facebookID);
       }
    }
    void AuthCalback(FBResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook logged worked!");
            isLogged();
        }
        else
        {
            Debug.Log("Facebook logged failed!");
        }
    }
    public void SetScore(int score)
    {
        Debug.Log("SetScore");
        var scoreData = new Dictionary<string, string>();
        scoreData["score"] = score.ToString();
        FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult result)
        {
            Debug.Log("score submit result" + result.Text);
        }, scoreData
        );
    }
    public void Share()
    {
        FB.Feed(
            linkCaption: "Estoy jugando este juego mortal!",
            // picture: "HHTP PICTURE: ",
            linkName: "Check out this game",
            link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag" + (FB.IsLoggedIn ? FB.UserId : "guest")
            );
    }
    public void Invite()
    {
        FB.AppRequest(
            message: "This game is perfect, join me!",
            title: "Invite your friends to join Mad Rollers!"
            );
    }
    //void DealWithProfilePicture(FBResult result)
    //{
    //    if (result.Error != null)
    //    {
    //        Debug.Log("Problemas al traer la prifile picture");
    //        FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, DealWithProfilePicture);
    //        return;
    //    }
    //    picture = result.Texture;
    //    Data.Instance.events.OnFacebookPictureLoaded();
    //}
}
