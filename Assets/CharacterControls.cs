using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterControls : MonoBehaviour {

	public bool isAutomata;
    CharacterBehavior characterBehavior;
	public List<CharacterBehavior> childs;
    Player player;
    private float rotationY;
    private float rotationZ = 0;
    private float turnSpeed = 2.8f;
    private float speedX = 9f;
    private bool mobileController;
    public bool ControlsEnabled = true;
    private CharactersManager charactersManager;

	void Start () {
        characterBehavior = GetComponent<CharacterBehavior>();
        player = GetComponent<Player>();
        Invoke("EnabledMovements", 0.5f);
        charactersManager = Game.Instance.GetComponent<CharactersManager>();
	}
	public void EnabledMovements(bool enabledControls)
    {
		ControlsEnabled = enabledControls;
    }
	public void AddNewChild(CharacterBehavior child)
	{
		childs.Add (child);
	}
	public void RemoveChild(CharacterBehavior child)
	{
		childs.Remove (child);
	}
	// Update is called once per frame
	void LateUpdate () {
		
		if (characterBehavior.state == CharacterBehavior.states.CRASH || characterBehavior.state == CharacterBehavior.states.DEAD) 
			return;
		if (Time.deltaTime == 0) return;
		if (mobileController) {
			moveByAccelerometer ();
		} else if(!isAutomata)
        {
            if (InputManager.getFire(player.id))
            {
                characterBehavior.CheckFire();
            }
            if (InputManager.getJump(player.id))
            {
                characterBehavior.Jump();
				if(childs.Count>0)
					StartCoroutine ( ChildsJump ());
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

		characterBehavior.UpdateByController(rotationY); 
	}

  
	float lastHorizontalKeyPressed;
    private void moveByKeyboard()
    {
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION && Game.Instance.level.charactersManager.distance<40)
			return;
		float _speed = InputManager.getHorizontal(player.id);
		if (lastHorizontalKeyPressed != _speed) {
			lastHorizontalKeyPressed = _speed;
			if(!isAutomata)
				Data.Instance.inputSaver.MoveInX (lastHorizontalKeyPressed, transform.position);
		}
		MoveInX (_speed);
    }
	public void MoveInX(float _speed)
	{
		
		if (_speed < -0.5f || _speed > 0.5f) {
			float newPosX = _speed*speedX;
			float newRot = turnSpeed * ( Time.deltaTime * 35);
			if (newPosX > 0)
				rotationY += newRot;
			else if (newPosX < 0)
				rotationY -= newRot;
			else if (rotationY > 0)
				rotationY -= newRot;
			else if (rotationY < 0)
				rotationY += newRot;
		} else{
			rotationY = 0;
		}

		if (rotationY > 30) rotationY = 30;
		else if (rotationY < -30) rotationY = -30;

		if (Time.deltaTime == 0) return;

		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationY, rotationZ);

		if (childs.Count > 0)
			UpdateChilds ();
	}

	//childs:
	IEnumerator ChildsJump()
	{
		if(childs == null || childs.Count>0)
			yield return null;
		foreach (CharacterBehavior cb in childs) {
			yield return new WaitForSeconds (0.18f);
			cb.Jump ();
		}
		yield return null;
	}
	void UpdateChilds()
	{
		foreach (CharacterBehavior cb in childs) {
			cb.controls.rotationY = rotationY / 1.5f;
			cb.transform.localRotation = transform.localRotation;
		}
	}













	/// <summary>
	/// For mobile
	/// </summary>
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
}
