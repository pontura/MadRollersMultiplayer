using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiTutorial : MonoBehaviour {

    public GameObject helpPanel;
    public MissionSignal MissionSignal;
    public GameObject buttonJump;
    public GameObject buttonShoot;
    
    public GameObject helpMove;
    public GameObject helpJump;
    public GameObject helpShoot;

    public GameObject[] helps;

    public Text jumpTitle;
    public Text jumpSubtitle;

    private bool ready;
    private int jumpLevel = 2;
    private int doubleJumpLevel = 3;
    private int shootLevel = 4;

    public states state;
    public enum states
    {
        MOVE,
        JUMP,
        DOUBLEJUMP,
        SHOOT,
        READY
    }

    private CharactersManager charactersManager;

    private bool isMobile()
    {
        return true;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            return true;
        return false;
    }
	void Start () 
    {
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            Destroy(helpPanel);
            print("________________destroy tutorial ___________");
            return;
        }

        foreach (GameObject go in helps)
            go.SetActive(false);
       

        buttonJump.SetActive(false);
        buttonShoot.SetActive(false);
        
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnAvatarJump += OnAvatarJump;
        Data.Instance.events.OnAvatarShoot += OnAvatarShoot;

        charactersManager = Game.Instance.GetComponent<CharactersManager>();
	}
    public void Help()
    {
        if (Time.timeScale == 1)
        {
            foreach (GameObject go in helps)
                go.SetActive(true);

            helpMove.SetActive(false);

            if (Data.Instance.playMode == Data.PlayModes.STORY)
            {
                if (Data.Instance.missionActive == 1)
                {
                    helpMove.SetActive(true);
                    helpJump.SetActive(false);
                    helpShoot.SetActive(false);
                }
                else if (Data.Instance.missionActive == 2)
                {
                    helpJump.SetActive(false);
                    helpShoot.SetActive(false);
                }
                else if (Data.Instance.missionActive == 3)
                {
                    helpJump.SetActive(true);
                    helpShoot.SetActive(false);
                }
                else if (Data.Instance.missionActive > 3)
                {
                    helpJump.SetActive(true);
                    helpShoot.SetActive(true);
                }
            }

           

            MissionSignal.Close();
            Time.timeScale = 0;
        }
    }
    public void CloseHelp()
    {
        Time.timeScale = 1;
        foreach (GameObject go in helps)
            go.SetActive(false);
    }
    private bool canDisplaySignal()
    {
        if (charactersManager.getMainCharacter() == null
            ||
            charactersManager.getMainCharacter().state == CharacterBehavior.states.CRASH
            ||
           charactersManager.getMainCharacter().state == CharacterBehavior.states.DEAD
        )
            return false;
        return true;
    }
    private void OnListenerDispatcher(string message)
    {
        if (!canDisplaySignal())
            return;

        if (Data.Instance.missionActive == 1)
        {
            state = states.MOVE;

            if(message != "ShowMissionName")
                Invoke("showHelp", 1);
        }
        else if (Data.Instance.missionActive == jumpLevel)
            state = states.JUMP;
        else if (Data.Instance.missionActive == doubleJumpLevel)
            state = states.DOUBLEJUMP;
        else if (Data.Instance.missionActive == shootLevel)
            state = states.SHOOT;
        else
        {
            state = states.READY;
            showButtons();
        }
        if (state != states.MOVE && state != states.READY && message == "ShowMissionName")
            showHelp();
    }
    private void showHelp()
    {

        if (isMobile())
        {
            
        }
        else
        {
        }
        if (state == states.MOVE)
        {
            MissionSignal.Close();
            helpMove.SetActive(true);
            helpMove.GetComponent<Animation>().Play( "MoveOn" );
            Invoke("OnDeviceMovedOver", 3);
        }
        else if (state == states.JUMP)
        {
            StartCoroutine(Play(helpJump.GetComponent<Animation>(), "JumpOn", false));
        }
        else if (state == states.DOUBLEJUMP)
        {
            StartCoroutine(Play(helpJump.GetComponent<Animation>(), "JumpOn", false));
        }
        else
        {
            StartCoroutine(Play(helpShoot.GetComponent<Animation>(), "ShootOn", false));
        }
    }
	public void showButtons () 
    {
        if (!canDisplaySignal()) return;

        if (isMobile())
        {
            if (state == states.JUMP || state == states.DOUBLEJUMP)
                buttonJump.SetActive( true );
            else  if (state != states.MOVE)
            {
                buttonJump.SetActive( true );
                buttonShoot.SetActive( true );                
            }
            if (state == states.READY)
                Reset();
        }
	}
    void OnDestroy()
    {
        Reset();
    }
    void Reset()
    {
        Data.Instance.events.OnAvatarJump -= OnAvatarJump;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
    }
    private void OnDeviceMovedOver()
    {
        helpMove.SetActive(false);
        Data.Instance.events.OnGamePaused(false);
    }
    private void OnAvatarJump()
    {
         if (!ready) return;
         if (state == states.SHOOT) return;
         ready = false;

         helpJump.SetActive(false);

         Data.Instance.events.OnGamePaused(false);
         //Data.Instance.events.OnAvatarJump -= OnAvatarJump;
    }
    private void OnAvatarShoot()
    {

        if (!ready) return;
        if (state != states.SHOOT) return;
        ready = false;

        helpShoot.SetActive(false);
        Data.Instance.events.OnGamePaused(false);
        //Data.Instance.events.OnAvatarJump -= OnAvatarJump;
    }
    

    //private IEnumerator Play(this Animation animation, string clipName, bool useTimeScale, Action onComplete)
    private IEnumerator Play(this Animation animation, string clipName, bool useTimeScale)
    {
        if (canDisplaySignal())
        {
            if (state == states.JUMP)
                yield return new WaitForSeconds(1.9f);
            else if (state == states.SHOOT)
                yield return new WaitForSeconds(2.4f);
            else if (state == states.DOUBLEJUMP)
            {
                yield return new WaitForSeconds(2.9f);
                charactersManager.getMainCharacter().state = CharacterBehavior.states.JUMP;
            }
        }

        animation.gameObject.SetActive(true);
        MissionSignal.Close();
        showButtons();

        if (canDisplaySignal())
        {

            ready = true;

            //if (ready) yield break;

            Data.Instance.events.OnGamePaused(true);
            charactersManager.character.ResetJump();

            if (isMobile())
            {
                //if (state == states.JUMP)
                //    helpSprite.enabled = true;
                //else if (state == states.DOUBLEJUMP)
                //    DoubleJumpHelpSprite.enabled = true;
                //else if (state == states.SHOOT)
                //    ShootHelpSprite.enabled = true;
            }
            //else if (state == states.JUMP)
            //    helpJumpSpriteKeyboard.enabled = true;
            //else if (state == states.DOUBLEJUMP)
            //    helpJumpSpriteKeyboard.enabled = true;
            //else if (state == states.SHOOT)
            //    helpShootSpriteKeyboard.enabled = true;

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
                //if (onComplete != null)
                //{
                //    onComplete();
                //}
            }
            else
                animation.Play(clipName);
        }
    }
}
