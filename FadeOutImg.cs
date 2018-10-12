using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum TypeOfUI
{
    image,
    text,
    textWithOutline
}

public class FadeOutImg : MonoBehaviour {

    public TypeOfUI typeOfUI;
    public AnimationCurve fade;
    private Image img;
    private Text textElement;
    private Outline outline;
    private float timer;

	void Start () {
        timer = 0;
        switch (typeOfUI)
        {
            case TypeOfUI.image:
                img = GetComponent<Image>();
                break;
            case TypeOfUI.text:
                textElement = GetComponent<Text>();
                break;
            case TypeOfUI.textWithOutline:
                textElement = GetComponent<Text>();
                outline = GetComponent<Outline>();
                break;
            default:
                break;
        }
        
	}

    private void Update()
    {
        if (fade.Evaluate(timer) > 0)
        {
            timer += Time.deltaTime;
            switch (typeOfUI)
            {
                case TypeOfUI.image:
                    img.color = new Color(1, 1, 1, fade.Evaluate(timer));
                    break;
                case TypeOfUI.text:
                    textElement.color = new Color(1, 1, 1, fade.Evaluate(timer));
                    break;
                case TypeOfUI.textWithOutline:
                    textElement.color = new Color(1, 1, 1, fade.Evaluate(timer));
                    outline.effectColor = new Color(0, 0, 0, fade.Evaluate(timer));
                    break;
                default:
                    break;
            }
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
