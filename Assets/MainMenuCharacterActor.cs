using UnityEngine;
using System.Collections;
using AlpacaSound.RetroPixelPro;

public class MainMenuCharacterActor : MonoBehaviour {

    public int id;
    public Animation anim;
    public GameObject bg;

	public MeshRenderer GorroMaterial;

	public Material gorro1;
	public Material gorro2;
	public Material gorro3;
	public Material gorro4;


	void Start () {
        anim.Play("run");

		if(id==0)
			GorroMaterial.material = gorro1;
		else if(id==2)
			GorroMaterial.material = gorro2;
		else if(id==3)
			GorroMaterial.material = gorro3;
		else
			GorroMaterial.material = gorro4;

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
