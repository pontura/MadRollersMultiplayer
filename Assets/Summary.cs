using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Summary : MonoBehaviour {

    public GameObject panel;
    private int countDown;
    public Animation anim;

	public List<MainMenuButton> buttons;
	int optionSelected = 0;
    private bool isOn;

    void Start()
    {
        panel.SetActive(false);
		Data.Instance.events.OnGameOver += OnGameOver;
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
		Data.Instance.events.OnFireUI -= OnFireUI;
    }
	void OnGameOver()
    {
        if (isOn) return;

        Invoke("SetOn", 2F);
    }
    void SetOn()
    {
		isOn = true;
        panel.SetActive(true);
        
		buttons [0].SetOn (true);
		buttons [1].SetOn (false);

        StartCoroutine(Play(anim, "popupOpen", false, null));
	}
	public void Restart()
	{
		Data.Instance.isReplay = true;
		Game.Instance.ResetLevel();        
	}
	void Update()
	{
		if (!isOn)
			return;

		lastClickedTime += Time.deltaTime;
		if (lastClickedTime > 0.1f)
			processAxis = true;
		for (int a = 0; a < 4; a++) {
			if (InputManager.getJump (a)) 
				OnJoystickClick ();
			if (InputManager.getFire (a)) 
				OnJoystickClick ();
			if (processAxis) {
				float v = InputManager.getVertical (a);
				if (v < -0.5f)
					OnJoystickUp ();
				else if (v > 0.5f)
					OnJoystickDown ();
			}
		}
	}


	float lastClickedTime = 0;
	bool processAxis;

	void OnJoystickUp () {
		buttons [0].SetOn (true);
		buttons [1].SetOn (false);
		optionSelected = 0;
	}
	void OnJoystickDown () {
		buttons [1].SetOn (true);
		buttons [0].SetOn (false);
		optionSelected = 1;
	}

	void OnJoystickClick () {
		if (optionSelected == 0)
			Restart ();
		else
			Game.Instance.GotoLevelSelector ();	
		isOn = false;
	}
	void OnJoystickBack () {
		//Data.Instance.events.OnJoystickBack ();
	}
	void ResetMove()
	{
		processAxis = false;
		lastClickedTime = 0;
	}



//    void CountDown()
//    {
//		if (!isOn) return;
//        if (countDown < 1)
//        {
//			isOn = false;
//			Game.Instance.GotoLevelSelector ();
//            return;
//        }
//        countDown--;
//
//		foreach(Text C in Continue)
//        	C.text = countDown.ToString();
//		
//        Invoke("CountDown", 0.5f);
//    }
	//public void Revive()
  //  {
	//	isOn = false;
     //   cancelCountDown = true;
     //   ReviveConfirma();
   // }
  
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
