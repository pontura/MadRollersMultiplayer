using UnityEngine;
using System.Collections;

public class CharacterControls : MonoBehaviour {

    CharacterBehavior characterBehavior;
    Player player;
    private float rotationY;
    private float rotationZ = 0;
    private float turnSpeed = 2.8f;
    private float speedX = 9f;
    private bool mobileController;
    private bool ControlsEnabled = false;
    private CharactersManager charactersManager;

	void Start () {
        characterBehavior = GetComponent<CharacterBehavior>();
        player = GetComponent<Player>();
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            mobileController = true;
        StartCoroutine(enabledMovements());

        charactersManager = Game.Instance.GetComponent<CharactersManager>();
	}
    IEnumerator enabledMovements()
    {
        yield return new WaitForSeconds(0.5f);
        ControlsEnabled = true;
    }
	// Update is called once per frame
	void LateUpdate () {

		if (InputManager.getFire(player.id))
		{
			Data.Instance.events.OnFireUI();
		}

        if (characterBehavior.state == CharacterBehavior.states.CRASH || characterBehavior.state == CharacterBehavior.states.DEAD) return;

        if (mobileController)
            moveByAccelerometer();
        else
        {
            if (InputManager.getFire(player.id))
            {
                characterBehavior.CheckFire();
            }
            if (InputManager.getJump(player.id))
            {
                characterBehavior.Jump();
            } else
            if (Input.GetButton("Jump1"))
            {
                characterBehavior.JumpPressed();
            }
            else
            {
                characterBehavior.AllButtonsReleased();
            }
           
            moveByKeyboard();
        }

        if (Time.deltaTime == 0) return;
        characterBehavior.UpdateByController(rotationY);
       // player.UpdateByController();
	}

    private void moveByAccelerometer()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];
           if (touch.position.x < Screen.width / 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    characterBehavior.Jump();
                else
                {
                    characterBehavior.JumpPressed();
                }
            }
            else if (touch.position.x > Screen.width / 2)
            {
                characterBehavior.CheckFire();
            }
        } else
        {
            characterBehavior.AllButtonsReleased();
        } 


        if (Time.deltaTime == 0) return;
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, Input.acceleration.x * 50, rotationZ);
       // transform.Translate(0, 0, Time.deltaTime * characterBehavior.speed);

    }

    private void moveByKeyboard()
    {
		if (Game.Instance.level.charactersManager.distance<35)
			return;
		float _speed = InputManager.getHorizontal(player.id);
		if (_speed < -0.5f || _speed > 0.5f) {
			float newPosX = _speed*speedX;
			if (newPosX > 0)
				rotationY += turnSpeed;
			else if (newPosX < 0)
				rotationY -= turnSpeed;
			else if (rotationY > 0)
				rotationY -= turnSpeed;
			else if (rotationY < 0)
				rotationY += turnSpeed;
		} else{
			rotationY = 0;
		}

        if (rotationY > 30) rotationY = 30;
        else if (rotationY < -30) rotationY = -30;

        if (Time.deltaTime == 0) return;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationY, rotationZ);


    }
}
