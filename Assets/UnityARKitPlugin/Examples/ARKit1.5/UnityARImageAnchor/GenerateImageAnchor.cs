using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using System.Linq;

public class GenerateImageAnchor : MonoBehaviour {

	private ProductStore _store = new ProductStore();
	private IDigiModule _digiModule = UiScript._currentModule;
	private Product _product;

	[SerializeField]
	private ARReferenceImage referenceImage;

	[SerializeField]
	private GameObject prefabToGenerate;

	private GameObject imageAnchorGO;

	// Use this for initialization
	void Start () {
		//_digiModule.Form.GetEntry ("Nuts").Value = true.ToString();
		//_digiModule.ApplyForm ();
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
		_product = _store.GetProducts().Where(x => x.ImageName == prefabToGenerate.name).FirstOrDefault();
	}

	void AddImageAnchor(ARImageAnchor arImageAnchor)
	{
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			if (prefabToGenerate.name == "RiegelAd") {
				Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
				var newPos = new Vector3 (position.x, position.y + 0.06f, position.z);
				Quaternion rotation = Quaternion.AngleAxis(30, new Vector3(0, 0, 1));//UnityARMatrixOps.GetRotation (arImageAnchor.transform);

				imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, newPos, rotation);
			} else {
				Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
				Quaternion rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);

				imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, position, rotation);
			}
		}
		UpdateColor ();
	}

	void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	{
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			if (imageAnchorGO.name == "RiegelAd(Clone)") {
				
				Debug.Log ("RiegelStuff");

				var pos = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
				Debug.Log (pos.x);
				var newPos = new Vector3((float)(pos.x + 0.0f), (float)(pos.y + 0.06f), (float)(pos.z + 0.0f));
				imageAnchorGO.transform.position = newPos;
			} else {
				imageAnchorGO.transform.position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
				imageAnchorGO.transform.rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
			}
		}
		UpdateColor ();
	}

	void RemoveImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor removed");
		if (imageAnchorGO) {
			GameObject.Destroy (imageAnchorGO);
		}

	}

	void OnDestroy()
	{
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;

	}

	// Update is called once per frame
	void Update () {
	}

	void UpdateColor() {
		if (_product == null || imageAnchorGO == null)
			return;

		var color = _digiModule.Filter.CalculateOverlayColor (_product);


		Debug.Log (color.ToString());

		imageAnchorGO.GetComponent<MeshRenderer> ().material.SetColor ("_Color", color);
	}
}
