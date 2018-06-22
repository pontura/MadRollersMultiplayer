using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

    public Image sprite;
    private float progress = 0;
    public float progression;

	private void Start()
	{
        reset();
	}
	private void Awake () {
        sprite.fillAmount = 0;
	}
	public void setProgression(float progression)
	{
        this.progression = progression;
		if(progression>100) progression = 100;
		progress = progression/100.0f;
        sprite.fillAmount = progress;
	}
    public void reset()
    {
        setProgression(0);
    }
	
}
