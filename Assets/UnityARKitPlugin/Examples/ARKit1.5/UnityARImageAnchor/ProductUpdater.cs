using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProductUpdater : MonoBehaviour {

	private ProductStore _store = new ProductStore();
	private IDigiModule _digiModule = new InterpolatingModule ();
	private Product _product;

	// Use this for initialization
	void Start () {
		_product = _store.GetProducts().Where(x => x.ImageName == gameObject.name).First();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Changing color.");
		Debug.Log (gameObject.GetComponent<Renderer> ());
		Debug.Log (gameObject.GetComponent<MeshRenderer> ());
		var color = _digiModule.Filter.CalculateOverlayColor (_product);
		gameObject.GetComponent<Renderer> ().material.color = color;
		gameObject.GetComponent<MeshRenderer> ().material.color = color;

	}
}
