using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stars : MonoBehaviour {

	[SerializeField] 
    Image star1;
    [SerializeField]
    Image star2;
    [SerializeField]
    Image star3;

	public void Init(int stars)
    {
        if (stars == 0)
        {
            star1.color = Color.black;
            star2.color = Color.black;
            star3.color = Color.black;
        }
        else if (stars == 1)
        {
            star2.color = Color.black;
            star3.color = Color.black;
        }
        else if (stars == 2)
        {
            star3.color = Color.black;
        }
	}
}
