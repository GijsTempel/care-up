using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TableCleaningAction : MonoBehaviour
{
    public GameObject cleanUpUI;
    public Image progressImage;
    public GameObject cleanColliderPrefab;
    public Vector3 posOffset = new Vector3(0.05f, 0, -0.05f);
    public int xNum = 10;
    public int yNum = 10;
    int numberOfColliders;
    float rationToClean = 0.35f;
    public GameObject StartPoint;
    private Vector3 startPos;
    private GameObject colliderHolder;
    ActionExpectant actionExpectant;

    private bool locakedAction = true;
    // Start is called before the first frame update
    void Start()
    {
        colliderHolder = transform.Find("ColliderHolder").gameObject;
        startPos = StartPoint.transform.localPosition;

        for (int i = 0; i < xNum; i++)
        {
            for (int j = 0; j < yNum; j++)
            {
                Vector3 newPos = startPos + new Vector3(i * posOffset.x, 0, j * posOffset.z);

                GameObject newInstance = Instantiate(cleanColliderPrefab, colliderHolder.transform) as GameObject;
                newInstance.transform.localPosition = newPos;
                newInstance.GetComponent<CleaningCollider>().cleaningMaster = this;
                numberOfColliders++;
            }
        }
    }

    void EnableActionComponents(bool toEnable)
    {
        cleanUpUI.SetActive(toEnable);
        colliderHolder.SetActive(toEnable);

    }

    void Update()
    {
        if (locakedAction)
        {
            if (actionExpectant == null)
                actionExpectant = transform.GetComponentInChildren<ActionExpectant>();

            if (actionExpectant != null)
            {
                if (actionExpectant.isCurrentAction)
                {
                    EnableActionComponents(true);
                }

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
