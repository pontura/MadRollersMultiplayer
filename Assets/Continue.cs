using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class Continue : MonoBehaviour {

	delegate void DelayedMethod();
	private int num = 9;
	public Text countdown_txt;
	private float speed = 0.5f;
    private bool clicked;
	
	IEnumerator WaitAndDo(float time, DelayedMethod method)
	{
		yield return new WaitForSeconds(time);
		method();
	}

	public void DoStuff()
	{
		num--;
		if(num==0)
		{
            Data.Instance.LoadLevel("MainMenu");
            clicked = true;
			return;
		}
		countdown_txt.text = num.ToString();
		StartCoroutine(WaitAndDo(speed, DoStuff));
	}	

	void Start () {
        clicked = false;
		StartCoroutine(WaitAndDo(speed, DoStuff));
	}

	void Update () {
        if (clicked) return;
        if (Input.anyKeyDown)
        {
            clicked = true;
            StopCoroutine("WaitAndDo");
            Data.Instance.LoadLevel("Game");
		}
	}

}
