using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TableCleaningAction : MonoBehaviour
{
    public Image progressImage;
    public GameObject cleanColliderPrefab;
    public Vector3 posOffset = new Vector3(0.05f, 0, -0.05f);
    public int xNum = 10;
    public int yNum = 10;
    int numberOfColliders;
    float rationToClean = 0.35f;
    public GameObject StartPoint;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = StartPoint.transform.localPosition;

        for (int i = 0; i < xNum; i++)
        {
            for (int j = 0; j < yNum; j++)
            {
                Vector3 newPos = startPos + new Vector3(i * posOffset.x, 0, j * posOffset.z);

                GameObject newInstance = Instantiate(cleanColliderPrefab, transform) as GameObject;
                newInstance.transform.localPosition = newPos;
                newInstance.GetComponent<CleaningCollider>().cleaningMaster = this;
                numberOfColliders++;
            }
        }
    }

    public bool IsCleanActionAllowed()
    {
        return true;
    }

    public void CleanActionCount()
    {
        int cleaned = 0;
        foreach(CleaningCollider c in transform.GetComponentsInChildren<CleaningCollider>())
        {
            if (c.isCleaned)
                cleaned++;
        }

        float currentCleanRatio = Remap((float)cleaned / (float)numberOfColliders, 0, rationToClean, 0f, 1f);
        progressImage.fillAmount = currentCleanRatio;
    }


    float Remap(float source, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
    {
        return targetFrom + (source-sourceFrom)*(targetTo-targetFrom)/(sourceTo-sourceFrom);
    }

}
