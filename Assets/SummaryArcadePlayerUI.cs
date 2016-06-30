using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SummaryArcadePlayerUI : MonoBehaviour {

    public Image background;
    public GameObject titleFields;
    public GameObject scoreFields;
    public GameObject positionFields;

	public void Init(Color color, string title, int score, int percent, int position) {

        background.color = color;

        foreach (Text field in titleFields.GetComponentsInChildren<Text>())
            field.text = title;

        foreach (Text field in scoreFields.GetComponentsInChildren<Text>())
        {
            if (score == 0)
                field.text = "NO EsITE";
            else
            {
                field.text = score + "x (" + percent + "%)";
            }
        }

        foreach (Text field in positionFields.GetComponentsInChildren<Text>())
            field.text = position.ToString();
	}
}
