using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScript : MonoBehaviour {

    private IDigiModule _currentModule = new SearchModule();

    private float _lerpTime = 1f;
    private float _currentLerpTime = 1f;
    private Vector3 _formTopPosition;
    private Vector3 _formBottomPosition;
    private Vector3 _formStartPosition;
    private Vector3 _formEndPosition;

    // Use this for initialization
    void Start ()
	{
	    foreach (Transform child in transform)
	    {
	        child.gameObject.SetActive(false);
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
	    else
	    {
	        //lerp Form panel
	        var t = _currentLerpTime / _lerpTime;
	        t = t * t * (3f - 2f * t);
	        transform.position = Vector3.Lerp(_formStartPosition, _formEndPosition, t);
        }
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

        _formStartPosition = _formBottomPosition;
        _formEndPosition = _formTopPosition;
        _currentLerpTime = 0f;

        var bottomOfFormPosition = formObject.transform.position;

        foreach (var entry in _currentModule.Form.GetEntries())
        {
            switch (entry.Type)
            {
                case FormEntryType.Label:
                    var preset = GetComponent("LabelPreset");
                    //var obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    //
                    break;
                case FormEntryType.TextField:

                    break;
                case FormEntryType.Button:

                    break;
                case FormEntryType.Checkbox:

                    break;
                case FormEntryType.Line:

                    break;
                case FormEntryType.Select:

                    break;
                case FormEntryType.Slider:

                    break;
            }
        }
    }

    public void CloseForm()
    {
        var formObject = GetComponent("FormPanel") as RectTransform;
        if (formObject == null)
            return;
        
        _formStartPosition = _formTopPosition;
        _formEndPosition = _formBottomPosition;
        _currentLerpTime = 0f;
    }
}
