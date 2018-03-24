using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScript : MonoBehaviour {

    private IDigiModule _currentModule = new SearchModule();

    private float _lerpTime;
    private float _currentLerpTime;
    private Vector3 _formTopPosition;
    private Vector3 _formBottomPosition;
    private Vector3 _formStartPosition;
    private Vector3 _formEndPosition;

    // Use this for initialization
    void Start ()
	{
	    foreach (var renderer in GetComponent("ObjectStore").GetComponentsInChildren<Renderer>())
	    {
	        renderer.enabled = false;
	    }
	    _formBottomPosition = GetComponent("FormPanel").transform.position;
	    _formTopPosition = _formBottomPosition + new Vector3(0, 375, 0);
	}
	
	// Update is called once per frame
	void Update () {
	    //increment timer once per frame
	    _currentLerpTime += Time.deltaTime;
	    if (_currentLerpTime > _lerpTime)
	    {
	        _currentLerpTime = _lerpTime;
	    }

	    //lerp Form panel
	    var perc = _currentLerpTime / _lerpTime;
	    transform.position = Vector3.Lerp(_formStartPosition, _formEndPosition, perc);
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

        _currentLerpTime = 0f;
        _formStartPosition = _formBottomPosition;
        _formEndPosition = _formTopPosition;
    }

    public void CloseForm()
    {
        var formObject = GetComponent("FormPanel") as RectTransform;
        if (formObject == null)
            return;
        
        _currentLerpTime = 0f;
        _formStartPosition = _formTopPosition;
        _formEndPosition = _formBottomPosition;
    }
}
