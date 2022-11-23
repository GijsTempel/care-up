using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*public class PDFFullScreenViewer : MonoBehaviour
{
    [HideInInspector]
    public bool tutorial_closedFullScrPDF = false;

    public Paroxe.PdfRenderer.PDFViewer FS_PDF;
	public Paroxe.PdfRenderer.PDFViewer IPad_PDF;

	private void FullScreenOff()
	{
		float ScrollPos = FS_PDF.gameObject.transform.Find("_Internal/VerticalScrollbar").GetComponent<Scrollbar>().value;
		IPad_PDF.gameObject.transform.Find("_Internal/VerticalScrollbar").GetComponent<Scrollbar>().value = ScrollPos;
		Invoke("CloseFS_PDF", 0.1f);

        tutorial_closedFullScrPDF = true;
    }
    
	private void CloseFS_PDF()
	{
		Destroy(gameObject);
	}
    
	void Start () {
		FS_PDF = transform.Find("FS_PDFViewer").GetComponent<Paroxe.PdfRenderer.PDFViewer>();
		IPad_PDF = GameObject.Find("PDFViewer").GetComponent<Paroxe.PdfRenderer.PDFViewer>();
		FS_PDF.PDFAsset = IPad_PDF.PDFAsset;
		FS_PDF.ReloadDocument();
		FS_PDF.CurrentPageIndex = IPad_PDF.CurrentPageIndex;
		float ScrollPos = IPad_PDF.gameObject.transform.Find("_Internal/VerticalScrollbar").GetComponent<Scrollbar>().value;
        FS_PDF.gameObject.transform.Find("_Internal/VerticalScrollbar").GetComponent<Scrollbar>().value = ScrollPos;
	}
}*/
