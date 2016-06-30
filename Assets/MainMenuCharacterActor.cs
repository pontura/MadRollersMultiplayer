using UnityEngine;
using System.Collections;
using AlpacaSound.RetroPixelPro;

public class MainMenuCharacterActor : MonoBehaviour {

    public int id;
    public Animation anim;
    public MeshRenderer[] toColorize;
    public GameObject bg;

	void Start () {
        anim.Play("run");

        foreach(MeshRenderer mr in toColorize)
            mr.material.color = Data.Instance.multiplayerData.colors[id];

        SetState(id, false);
	}
    public void SetState(int playerID, bool isActive)
    {
        if (playerID == id)
        {
            if (!isActive)
            {
                anim.Play("run");
                GetComponent<RetroPixelPro>().strength = 0;
                bg.SetActive(true);
            }
            else
            {
                anim.Play("saluda");
                GetComponent<RetroPixelPro>().strength = 1;
                bg.SetActive(false);
            }
        }
    }
}
