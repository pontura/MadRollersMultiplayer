using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AlpacaSound.RetroPixelPro;

[Serializable]
public class VideogameData {

	public int id;
	public string name;
	public Color floor_top;
	public Color floor_border;
	public Material skybox;
	public Color fog;
	public Sprite logo;
	public Sprite floppyCover;
	public RetroPixelPro retroPixelPro;
}
