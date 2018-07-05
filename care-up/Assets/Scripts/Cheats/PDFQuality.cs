using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PDFQuality : MonoBehaviour {

	Paroxe.PdfRenderer.PDFViewer pdfViewer;
	      
	// Use this for initialization
	void Start () {
		pdfViewer = gameObject.GetComponent<Paroxe.PdfRenderer.PDFViewer>();
      
		#if UNITY_WEBGL
		pdfViewer.MaxZoomFactorTextureQuality = 1f;
		#endif

	}
	

}
