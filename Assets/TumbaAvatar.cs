using UnityEngine;
using System.Collections;

public class TumbaAvatar : SceneObject {

    public TextMesh field;
    public Renderer renderer;

    public void SetField(string content)
    {
       // field.text = content;
    }
    public void SetPicture(string facebookID)
    {
        Texture2D texture2d = Social.Instance.hiscores.GetPicture(facebookID);
        if (texture2d)
            SetLoadedPicture(texture2d);
        else
            StartCoroutine(GetPicture(facebookID));
    }
    public void SetLoadedPicture(Texture2D texture2d)
    {
        renderer.material.mainTexture = texture2d;
    }
    IEnumerator GetPicture(string facebookID)
    {
        if (facebookID == "")
            yield break;

        WWW receivedData = new WWW("https" + "://graph.facebook.com/" + facebookID + "/picture?width=128&height=128");
        yield return receivedData;
        if (receivedData.error == null)
        {
            SetLoadedPicture(receivedData.texture);
            SocialEvents.OnFacebookImageLoaded(facebookID, receivedData.texture);
        }
        else
        {
            Debug.Log("ERROR trayendo imagen");
        }
    }
}
