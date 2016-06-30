using UnityEngine;
using System.Collections;

public class AddFacebookAccount : MonoBehaviour {

    void Start()
    {
       SocialEvents.OnFacebookUserLoaded += OnFacebookUserLoaded;
        SocialEvents.OnFacebookIdAdded += OnFacebookIdAdded;
    }
    public void OnDestroy()
    {
       SocialEvents.OnFacebookUserLoaded -= OnFacebookUserLoaded;
        SocialEvents.OnFacebookIdAdded -= OnFacebookIdAdded;
    }
    public void AddAccount()
    {
        Data.Instance.GetComponentInChildren<FBHolder>().Login();
    }    
    void OnFacebookUserLoaded(string facebookID, string username)
    {
        if (Data.Instance.GetComponent<UserData>().userId > 0)
        {
            Debug.Log("Usuario existía en la base, agrega facebookID nomas");
            Social.Instance.dataController.AddFacebookIdToExistingAccount(Data.Instance.GetComponent<UserData>().userId, facebookID);
        }
        else
        {
            Debug.Log("Usuario Nuevo : No existe un usuario en la base con ese Facebook id");
            Social.Instance.dataController.CreateUserByFacebookID(facebookID);
        }
    }
    void OnFacebookIdAdded()
    {
        Back();
    }
    public void Back()
    {
        Data.Instance.LoadLevel("MainMenu");
    }
}
