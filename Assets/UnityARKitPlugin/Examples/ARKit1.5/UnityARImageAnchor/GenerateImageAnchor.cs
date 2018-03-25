using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using System.Linq;

public class GenerateImageAnchor : MonoBehaviour {

	private ProductStore _store = new ProductStore();
	private Product _product;

	[SerializeField]
	private ARReferenceImage referenceImage;

	[SerializeField]
	private GameObject prefabToGenerate;

	[SerializeField]
	private GameObject adToGenerate;

	private GameObject imageAnchorGO;
	private GameObject adGO;

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
			if (adToGenerate != null) {
				Debug.Log ("Generating Ad");
				Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
				Quaternion rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
				imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, position, rotation);

				Vector3 position2 = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
				var newPos2 = new Vector3 (position.x, position.y + 0.06f, position.z);
				Quaternion rotation2 = Quaternion.AngleAxis(30, new Vector3(0, 0, 1));//UnityARMatrixOps.GetRotation (arImageAnchor.transform);

				adGO = Instantiate<GameObject> (adToGenerate, newPos2, rotation2);
			} else {
				Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
				Quaternion rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);

				imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, position, rotation);
			}
		}
	}

	void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	{
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			if (imageAnchorGO.name == "Riegel(Clone)") {
				var pos = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
				var newPos = new Vector3((float)(pos.x + 0.0f), (float)(pos.y + 0.06f), (float)(pos.z + 0.0f));
				adGO.transform.position = newPos;

				imageAnchorGO.transform.position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
				imageAnchorGO.transform.rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
			} else {
				imageAnchorGO.transform.position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
				imageAnchorGO.transform.rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
			}
		}
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
		UpdateColor ();
	}

	void UpdateColor() {
		if (_product == null || imageAnchorGO == null)
			return;


		var color = UiScript._currentModule.Filter.CalculateOverlayColor (_product);

		if (color.a == 0f && color.r == 1f && color.g == 1f && color.b == 1f) {
			imageAnchorGO.SetActive (false);
		} else if (color.r == 0.95f && color.g == 0.9f && color.b == 0.26f && color.a == 0.4f) {
			imageAnchorGO.SetActive (false);
			if (adGO != null)
				adGO.SetActive (true);
		} else {
			imageAnchorGO.SetActive (true);
			if (adGO != null)
				adGO.SetActive (false);
		}
		
		Debug.Log ("Updating Color: " + color.ToString());


		Debug.Log (color.ToString());

		imageAnchorGO.GetComponent<MeshRenderer> ().material.SetColor ("_Color", color);
	}
}
