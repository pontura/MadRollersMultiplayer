using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Summary : MonoBehaviour {

    public GameObject panel;
    public GameObject panela;
   // public Text meters;
    //public Text heartsToRevive;
    public Text[] Continue;
   // public Button ContinueButton;
    private int countDown;
    public Animation anim;
   // int totalHearts;
   // int newHearts;
   // private int heartsToReviveNum = 250;
   // private bool cancelCountDown;
    private bool isOn;

    void Start()
    {
        panel.SetActive(false);
        panela.SetActive(false);
		Data.Instance.events.OnGameOver += OnGameOver;
        //Data.Instance.events.OnAvatarFall += Init;
        //Data.Instance.events.OnAvatarCrash += Init; 
		Data.Instance.events.OnFireUI += OnFireUI;
    }
	void OnFireUI()
	{
		if (!isOn)
			return;
		isOn = false;
		Restart ();
	}
    void OnDestroy()
    {
		Data.Instance.events.OnGameOver -= OnGameOver;
       // Data.Instance.events.OnAvatarFall -= Init;
       // Data.Instance.events.OnAvatarCrash -= Init;
		Data.Instance.events.OnFireUI -= OnFireUI;
    }
	void OnGameOver()
    {
        if (isOn) return;

        countDown = 9;
        isOn = true;
        Invoke("SetOn", 2F);
       // meters.text = Game.Instance.GetComponent<CharactersManager>().distance + " mts";
    }
    void SetOn()
    {
       // totalHearts = GetComponent<HearsManager>().total;
       // if (Data.Instance.playMode == Data.PlayModes.STORY || heartsToReviveNum > totalHearts)
      //  {
           // Restart();
           // return;
      //  }
	//
        panel.SetActive(true);
        panela.SetActive(true);
        
      //  newHearts = GetComponent<HearsManager>().newHearts;
      //  heartsToRevive.text = "x" + heartsToReviveNum.ToString();

		CountDown ();

        StartCoroutine(Play(anim, "popupOpen", false, null));

	}
    void CountDown()
    {
		if (!isOn) return;
        if (countDown < 1)
        {
			isOn = false;
			Game.Instance.GotoLevelSelector ();
            return;
        }
        countDown--;

		foreach(Text C in Continue)
        	C.text = countDown.ToString();
		
        Invoke("CountDown", 0.5f);
    }
	//public void Revive()
  //  {
	//	isOn = false;
     //   cancelCountDown = true;
     //   ReviveConfirma();
   // }
    public void Restart()
    {
        Game.Instance.ResetLevel();        
    }
   // public void ReviveConfirma()
  //  {
      //  Data.Instance.events.OnUseHearts(heartsToReviveNum);
     //   Data.Instance.events.OnSoundFX("consumeHearts", -1);
        
      //  panela.SetActive(false);
      //  panel.SetActive(false);

      //  Invoke("ReviveTimeOut", 1);
   // }
  //  void ReviveTimeOut()
   // {
    //    Reset();
    //    Game.Instance.Revive();
  //  }
    IEnumerator Play(Animation animation, string clipName, bool useTimeScale, Action onComplete)
    {

        //We Don't want to use timeScale, so we have to animate by frame..
        if (!useTimeScale)
        {
            AnimationState _currState = animation[clipName];
            bool isPlaying = true;
            float _progressTime = 0F;
            float _timeAtLastFrame = 0F;
            float _timeAtCurrentFrame = 0F;
            float deltaTime = 0F;


            animation.Play(clipName);

            _timeAtLastFrame = Time.realtimeSinceStartup;
            while (isPlaying)
            {
                _timeAtCurrentFrame = Time.realtimeSinceStartup;
                deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                _timeAtLastFrame = _timeAtCurrentFrame;

                _progressTime += deltaTime;
                _currState.normalizedTime = _progressTime / _currState.length;
                animation.Sample();

                if (_progressTime >= _currState.length)
                {
                    if (_currState.wrapMode != WrapMode.Loop)
                    {
                        isPlaying = false;
                    }
                    else
                    {
                        _progressTime = 0.0f;
                    }

                }

                yield return new WaitForEndOfFrame();
            }
            yield return null;
            if (onComplete != null)
            {
                onComplete();
            }
        }
        else
            animation.Play(clipName);
    }  
}
