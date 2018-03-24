using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using System.Linq;

public class GenerateImageAnchor : MonoBehaviour {

	private ProductStore _store = new ProductStore();
	private IDigiModule _digiModule = new InterpolatingModule ();
	private Product _product;

	[SerializeField]
	private ARReferenceImage referenceImage;

	[SerializeField]
	private GameObject prefabToGenerate;

	private GameObject imageAnchorGO;

	// Use this for initialization
	void Start () {
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
		_product = _store.GetProducts().Where(x => x.ImageName == prefabToGenerate.name).FirstOrDefault();
		Debug.Log (_product.ImageName);
	}

	void AddImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor added");
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
			Quaternion rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);

			imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, position, rotation);
		}
		UpdateColor ();
	}

	void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor updated");
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			imageAnchorGO.transform.position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
			imageAnchorGO.transform.rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
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
		if (_product == null)
			return;

		var color = _digiModule.Filter.CalculateOverlayColor (_product);
		Debug.Log(imageAnchorGO.GetComponent<Renderer> () == null);

		imageAnchorGO.GetComponent<Renderer> ().material.SetColor ("_Color", color);
	}
}
