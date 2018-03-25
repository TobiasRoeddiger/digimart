using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiScript : MonoBehaviour {

	public static IDigiModule _currentModule = new InterpolatingModule();

    private const float _LERP_TIME = 1f;
    private float _currentLerpTime = 1f;
    private Vector2 _formTopPosition;
    private Vector2 _formBottomPosition;
    private Vector2 _formStartPosition;
    private Vector2 _formEndPosition;

    // Use this for initialization
    void Start ()
	{
		var fp = GameObject.Find ("FormPanel").GetComponent<RectTransform> ().anchoredPosition;
		_formBottomPosition = fp;
		_formTopPosition = new Vector2(fp.x, fp.y + 100);
	}
	
	// Update is called once per frame
	void Update () {
	    //increment timer once per frame
	    _currentLerpTime += Time.deltaTime;
	    if (_currentLerpTime > _LERP_TIME)
	    {
	        _currentLerpTime = _LERP_TIME;
	    }
	    else
	    {
	        //lerp Form panel
	        var t = _currentLerpTime / _LERP_TIME;
	        t = t * t * (3f - 2f * t);
			GameObject.Find("FormPanel").GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(_formStartPosition, _formEndPosition, t);
        }
    }

    public void AdButton_OnClick(string sceneName)
    {
		Debug.Log ("Ad Button Clicked");
        _currentModule = new AdModule();
        HighlightButton(EventSystem.current.currentSelectedGameObject);
        GameObject.Find("FormPanel").GetComponent<RectTransform>().anchoredPosition = _formBottomPosition;
        //OpenForm();
    }
    public void InterpolatingButton_OnClick(string sceneName)
    {
		Debug.Log ("Interpolating Button Clicked");
        _currentModule = new InterpolatingModule();
        HighlightButton(EventSystem.current.currentSelectedGameObject);
        OpenForm();
    }
    public void SearchButton_OnClick(string sceneName)
    {
		Debug.Log ("Search Button Clicked");
        _currentModule = new SearchModule();
        HighlightButton(EventSystem.current.currentSelectedGameObject);
        OpenForm();
    }
    public void AllergyButton_OnClick(string sceneName)
    {
		Debug.Log ("Allergy Button Clicked");
        _currentModule = new AllergyModule();
        HighlightButton(EventSystem.current.currentSelectedGameObject);
        OpenForm();
    }

    public void FormSaveButton_OnClick(string sceneName = null)
    {
        _currentModule.ApplyForm();
        CloseForm();
    }
    public void FormCancelButton_OnClick(string sceneName = null)
    {
        CloseForm();
    }


    private void HighlightButton(GameObject btn)
    {
        ResetButtonColors();
        var colors = btn.GetComponent<Button>().colors;
        colors.normalColor = colors.pressedColor;
        btn.GetComponent<Button>().colors = colors;
    }

    private void ResetButtonColors()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("NavButton"))
        {
            var colors = obj.GetComponent<Button>().colors;
            colors.normalColor = new Color(1, 1, 1, 1);
            obj.GetComponent<Button>().colors = colors;
        }
    }

    private void OpenForm()
    {
        // access form panel
		var formObject = GameObject.Find("FormPanel");
		if (formObject == null) {
			return;
		}

        // access container for dynamic entries
        var parent = GameObject.Find("DynFormContainer").transform;
        var bottomOfFormPosition = parent.position;

        // remove old form entries
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        // start opening animation
        _formStartPosition = _formBottomPosition;
        _formEndPosition = _formTopPosition;
        _currentLerpTime = 0f;

        // iterate over dynamic entry creation
        foreach (var entry in _currentModule.Form.GetEntries())
        {
			GameObject preset = null;
			GameObject obj = null;
            switch (entry.Type)
            {
                case FormEntryType.Label:
					preset = GameObject.Find("LabelPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), parent);
					(obj.GetComponent<Text>()).text = entry.Label;
                    break;
                case FormEntryType.TextField:
					preset = GameObject.Find("InputPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), parent);
					(obj.GetComponent<InputField>()).onValueChanged.AddListener((v) => { entry.Value = v; FormSaveButton_OnClick(); }); 
                    break;
                case FormEntryType.Button:
					preset = GameObject.Find("ButtonPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), parent);
                    obj.GetComponentInChildren<Text>().text = entry.Label;
                    break;
                case FormEntryType.Checkbox:
					preset = GameObject.Find("CheckboxPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), parent);
                    obj.GetComponentInChildren<Text>().text = entry.Label;
					(obj.GetComponent<Toggle>()).onValueChanged.AddListener((v) => { entry.Value = v.ToString(); FormSaveButton_OnClick(); });
                    break;
                case FormEntryType.Line:
					preset = GameObject.Find("LinePreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), parent);
                    break;
                case FormEntryType.Select:
					preset = GameObject.Find("SelectPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), parent);
                    (obj.GetComponent<Dropdown>()).options.Clear();
                    foreach (var pair in (entry as Select).Dictionary)
                    {
                        (obj.GetComponent<Dropdown>()).options.Add(new Dropdown.OptionData(entry.Value));
                    }
				    (obj.GetComponent<Dropdown>()).onValueChanged.AddListener((v) => { entry.Value = (entry as Select).Dictionary.Values.ToList()[v]; FormSaveButton_OnClick(); });
                    break;
                case FormEntryType.Slider:
					preset = GameObject.Find("SliderPreset");
                    obj = Instantiate(preset, bottomOfFormPosition, new Quaternion(), parent);
					(obj.GetComponent<Slider>()).minValue = (float) (entry as FormSlider).Min;
					(obj.GetComponent<Slider>()).maxValue = (float) (entry as FormSlider).Max;
					(obj.GetComponent<Slider>()).onValueChanged.AddListener((v) => { entry.Value = v.ToString(); FormSaveButton_OnClick(); });
                    break;
                default:
                    continue;
            }
			bottomOfFormPosition.y -= preset.GetComponent<RectTransform>().rect.height;
        }
    }

    private void CloseForm()
    {
        _formStartPosition = _formTopPosition;
        _formEndPosition = _formBottomPosition;
        _currentLerpTime = 0f;
    }
}
