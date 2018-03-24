using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UiScript : MonoBehaviour {

    private IDigiModule _currentModule = new SearchModule();


	// Use this for initialization
	void Start ()
	{
	    foreach (var renderer in GetComponent("ObjectStore").GetComponentsInChildren<Renderer>())
	    {
	        renderer.enabled = false;
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AdButton_OnClick(string sceneName)
    {
        _currentModule = new AdModule();
        OpenForm();
    }
    public void InterpolatingButton_OnClick(string sceneName)
    {
        _currentModule = new InterpolatingModule();
        OpenForm();
    }
    public void SearchButton_OnClick(string sceneName)
    {
        _currentModule = new SearchModule();
        OpenForm();
    }
    public void AllergyButton_OnClick(string sceneName)
    {
        _currentModule = new AllergyModule();
        OpenForm();
    }

    public void FormSaveButton_OnClick(string sceneName)
    {
        _currentModule.ApplyForm();
    }
    public void FormCancelButton_OnClick(string sceneName)
    {
        CloseForm();
    }


    public void OpenForm()
    {
        var formObject = GetComponent("FormPanel") as RectTransform;
        if (formObject == null)
            return;
        
        formObject.transform.DoMove(formObject.transform.position + new Vector3(0, 375, 0), 1f)
            .SetEase(Ease.EaseInOutCubic).SetDelay(0f);
    }

    public void CloseForm()
    {
        var formObject = GetComponent("FormPanel") as RectTransform;
        if (formObject == null)
            return;

        formObject.transform.DoMove(formObject.transform.position + new Vector3(0, -375, 0), 1f)
            .SetEase(Ease.EaseInOutCubic).SetDelay(0f);
    }
}
