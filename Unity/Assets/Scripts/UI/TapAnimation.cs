using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TapAnimation : MonoBehaviour
{
    public bool finish { get; set; } // finish generating text
    bool isTyping;  //text is typing
    public TextMeshProUGUI dialogText;
    public float textTime = 0.1f;

    List<string> textList = new List<string>();
    string text;

    // Start is called before the first frame update
    void OnEnable()
    {
        finish = false;
        isTyping = true;
        text = dialogText.text;
        dialogText.text = "";
        StartCoroutine(SetTextUI(text));
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // if the text is typing -> skip typing
            isTyping = false;
        }
    }

    IEnumerator SetTextUI(string text)
    {
        int word = 0;
        if (text != "")
        {
            while (isTyping && word < text.Length - 1)
            {
                dialogText.text += text[word];
                word++;
                yield return new WaitForSeconds(textTime);
            }
            dialogText.text = text;
        }

        finish = true;
        isTyping = false;
    }

    void OnDisable()
    {
        dialogText.text = text;
        StopAllCoroutines();
    }

    public void SetText(string text)
    {
        StopAllCoroutines();
        dialogText.text = "";
        StartCoroutine(SetTextUI(text));
    }
}
