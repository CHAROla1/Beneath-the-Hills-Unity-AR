using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;

public class Popup : MonoBehaviour
{

    [SerializeField] GameObject closeButton;

    void Update()
    {
        if (GetComponent<TapAnimation>())
        {
            if (GetComponent<TapAnimation>().finish)
            {
                closeButton?.SetActive(true);
            }
            else
            {
                closeButton?.SetActive(false);
            }
        }
    }

    public void Close()
    {
        var animator = GetComponent<Animator>();
        animator.SetBool("Close", true);
        StartCoroutine(RunPopupDestroy());
    }

    public void SetText(string text)
    {
        GetComponent<TapAnimation>()?.SetText(text);
    }

    private IEnumerator RunPopupDestroy()
    {
        yield return new WaitForSeconds(0.6f);
        SelectManager.Instance.enabled = true;
        ObjectDestroyer.DestroyObject(gameObject);
    }

    private void OnEnable()
    {
        var animator = GetComponent<Animator>();
        animator.Play("Popup Open");
        SelectManager.Instance.enabled = false;
    }

    public void Disable()
    {
        var animator = GetComponent<Animator>();
        animator.SetBool("Close", true);
        StartCoroutine(SetDisable());
    }

    IEnumerator SetDisable()
    {
        yield return new WaitForSeconds(0.6f);
        SelectManager.Instance.enabled = true;
        gameObject.SetActive(false);
    }

}
