using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionIcon : MonoBehaviour {

	public GameObject panel;
	public Transform container;
	GameObject icon;

	void Start()
	{
		panel.SetActive (false);
	}
	public void SetOn(Mission mission, GameObject specialIcon = null)
	{
		GetComponent<Animator> ().Play ("missionIconOn",0,0);
		panel.SetActive (true);
		if (icon != null) {
			StopAllCoroutines ();
			Destroy (icon);
			icon = null;
		}
		if (specialIcon != null) {
			icon = Instantiate (specialIcon);
		} else if (mission.missionIcon != null) {
			icon = Instantiate (mission.missionIcon);
		}
		if (icon == null)
			return;
		icon.transform.SetParent (container);
		icon.transform.localPosition = Vector3.zero;
		Animation anim = icon.GetComponentInChildren<Animation> ();
		if (anim != null) {
			StartCoroutine( PlayAnim (anim, anim.clip.name, false) );
		}
	}

	public static IEnumerator PlayAnim( Animation animation, string clipName, bool useTimeScales)
	{
		if(useTimeScales == false)
		{
			
			AnimationState _currState = animation[clipName];
			bool isPlaying = true;
			float _startTime = 0F;
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
				animation.Sample ();

				//Debug.Log(_progressTime);

				if (_progressTime >= _currState.length) 
				{
					//Debug.Log(&quot;Bam! Done animating&quot;);
					if(_currState.wrapMode != WrapMode.Loop)
					{
						//Debug.Log(&quot;Animation is not a loop anim, kill it.&quot;);
						//_currState.enabled = false;
						isPlaying = false;
					}
					else
					{
						//Debug.Log(&quot;Loop anim, continue.&quot;);
						_progressTime = 0.0f;
					}
				}

				yield return new WaitForEndOfFrame();
			}
			yield return null;
			//if(onComplete != null)
			//{
				//onComplete();
			//} 
		}
		else
		{
			animation.Play(clipName);
		}
	}
}
