using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UiScript : MonoBehaviour {

	public static IDigiModule _currentModule = new InterpolatingModule();

    private float _lerpTime = 1f;
    private float _currentLerpTime = 1f;
    private Vector2 _formTopPosition;
    private Vector2 _formBottomPosition;
    private Vector2 _formStartPosition;
    private Vector2 _formEndPosition;

    // Use this for initialization
    void Start ()
	{
		foreach (Transform child in GameObject.Find("ObjectStore").transform)
	    {
	        child.gameObject.SetActive(false);
	    }


		var canv = GameObject.Find ("UiCanvas").GetComponent<RectTransform> ().anchoredPosition;
		var fp = GameObject.Find ("FormPanel").GetComponent<RectTransform> ().anchoredPosition;
		_formBottomPosition =  fp;
		_formTopPosition = new Vector2(fp.x, fp.y + 375);
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
			GameObject.Find("FormPanel").GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(_formStartPosition, _formEndPosition, t);
			Debug.Log ("Y Pos: " + GameObject.Find("FormPanel").transform.position.y);
        }
    }

    public void AdButton_OnClick(string sceneName)
    {
		Debug.Log ("Ad Button Clicked");
        _currentModule = new AdModule();
        OpenForm();
    }
    public void InterpolatingButton_OnClick(string sceneName)
    {
		Debug.Log ("Interpolating Button Clicked");
        _currentModule = new InterpolatingModule();
        OpenForm();
    }
    public void SearchButton_OnClick(string sceneName)
    {
		Debug.Log ("Search Button Clicked");
        _currentModule = new SearchModule();
        OpenForm();
    }
    public void AllergyButton_OnClick(string sceneName)
    {
		Debug.Log ("Allergy Button Clicked");
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
		var formObject = GameObject.Find("FormPanel");
		if (formObject == null) {
			Debug.Log ("Returning");
			return;
		}

        _formStartPosition = _formBottomPosition;
        _formEndPosition = _formTopPosition;
        _currentLerpTime = 0f;

        var bottomOfFormPosition = formObject.transform.position;

		Debug.Log ("Number of entries: " + _currentModule.Form.GetEntries().Count);

        foreach (var entry in _currentModule.Form.GetEntries())
        {
			Debug.Log ("Writing entry.");
			GameObject preset = null;
			GameObject obj = null;
            switch (entry.Type)
            {
                case FormEntryType.Label:
					preset = GameObject.Find("LabelPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
					(obj.GetComponent<Text>()).text = entry.Label;
                    break;
                case FormEntryType.TextField:
					preset = GameObject.Find("InputPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
					(obj.GetComponent<InputField>()).onValueChanged.AddListener((v) => { entry.Value = v; }); 
                    break;
                case FormEntryType.Button:
					preset = GameObject.Find("ButtonPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    obj.GetComponentInChildren<Text>().text = entry.Label;
                    break;
                case FormEntryType.Checkbox:
					preset = GameObject.Find("CheckboxPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    obj.GetComponentInChildren<Text>().text = entry.Label;
					(obj.GetComponent<Toggle>()).onValueChanged.AddListener((v) => { entry.Value = v.ToString(); });
                    break;
                case FormEntryType.Line:
					preset = GameObject.Find("LinePreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    break;
                case FormEntryType.Select:
					preset = GameObject.Find("SelectPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
                    // TODO: fill chilren
				(obj.GetComponent<Dropdown>()).onValueChanged.AddListener((v) => { entry.Value = (entry as Select).Dictionary.Values.ToList()[v]; });
                    break;
                case FormEntryType.Slider:
					preset = GameObject.Find("SliderPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), formObject.transform);
					(obj.GetComponent<Slider>()).minValue = (float) (entry as FormSlider).Min;
					(obj.GetComponent<Slider>()).maxValue = (float) (entry as FormSlider).Max;
					(obj.GetComponent<Slider>()).onValueChanged.AddListener((v) => { entry.Value = v.ToString(); });
                    break;
                default:
                    continue;
            }
			bottomOfFormPosition.y -= (preset.GetComponent<RectTransform>()).sizeDelta[1] - 10;
        }
    }

    public void CloseForm()
    {
        _formStartPosition = _formTopPosition;
        _formEndPosition = _formBottomPosition;
        _currentLerpTime = 0f;
    }
}
