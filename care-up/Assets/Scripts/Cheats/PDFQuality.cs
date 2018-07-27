using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PDFQuality : MonoBehaviour {

    // never used for some builds
	//Paroxe.PdfRenderer.PDFViewer pdfViewer;
	      
	void Start ()
    {
        //pdfViewer = gameObject.GetComponent<Paroxe.PdfRenderer.PDFViewer>();
        #if UNITY_WEBGL
		    gameObject.GetComponent<Paroxe.PdfRenderer.PDFViewer>().MaxZoomFactorTextureQuality = 1f;
        #endif
    }


}
