	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class WaterPlatform : SceneObject {

	public GameObject[] all;

		void Start () {
			float id = 0;
			foreach (GameObject m in all) {
				m.GetComponent<Animation> () ["Water"].time = id / 10;
				id++;
			}
		}
	}
