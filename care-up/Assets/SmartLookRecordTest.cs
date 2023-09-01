using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SmartLookRecordTest : MonoBehaviour
{
    float COUNT_VALUE = 10f;
    float count_down_timer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count_down_timer -= Time.deltaTime;

        if (count_down_timer < 0)
        {
            GetComponent<Text>().text = "Is recording: " + SmartlookUnity.Smartlook.IsRecording().ToString() +
                " [" + Random.Range(0, 999).ToString() + "]";
            if (SmartlookUnity.Smartlook.IsRecording())
            {
                GetComponent<Text>().text += "\nDashboardSessionUrl:  " + SmartlookUnity.Smartlook.GetDashboardSessionUrl(false);
                GetComponent<Text>().text += "\nDashboardVisitorUrl:  " + SmartlookUnity.Smartlook.GetDashboardVisitorUrl();
            }
            count_down_timer = COUNT_VALUE;
        }
    }
}
