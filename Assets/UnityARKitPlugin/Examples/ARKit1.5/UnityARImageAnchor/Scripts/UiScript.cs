using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
            Component preset = null;
            Component obj = null;
            switch (entry.Type)
            {
                case FormEntryType.Label:
                    preset = GetComponent("LabelPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    (obj as Text).text = entry.Label;
                    break;
                case FormEntryType.TextField:
                    preset = GetComponent("InputPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    (obj as InputField).onValueChanged.AddListener((v) => { entry.Value = v; }); 
                    break;
                case FormEntryType.Button:
                    preset = GetComponent("ButtonPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    obj.GetComponentInChildren<Text>().text = entry.Label;
                    break;
                case FormEntryType.Checkbox:
                    preset = GetComponent("CheckboxPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    obj.GetComponentInChildren<Text>().text = entry.Label;
                    (obj as Toggle).onValueChanged.AddListener((v) => { entry.Value = v.ToString(); });
                    break;
                case FormEntryType.Line:
                    preset = GetComponent("LinePreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    break;
                case FormEntryType.Select:
                    preset = GetComponent("SelectPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    // TODO: fill chilren
                    (obj as Dropdown).onValueChanged.AddListener((v) => { entry.Value = (entry as Select).Dictionary.Values.ToList()[v]; });
                    break;
                case FormEntryType.Slider:
                    preset = GetComponent("SliderPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    (obj as Slider).minValue = (float) (entry as FormSlider).Min;
                    (obj as Slider).maxValue = (float) (entry as FormSlider).Max;
                    (obj as Slider).onValueChanged.AddListener((v) => { entry.Value = v; });
                    break;
                default:
                    continue;
            }
            bottomOfFormPosition.y -= (preset as RectTransform).sizeDelta[1] - 10;
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
