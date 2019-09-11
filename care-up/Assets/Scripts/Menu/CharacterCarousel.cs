using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCarousel : MonoBehaviour
{
    void Start()
    {
        
    }
    public void Turn(int dir)
    {
        if (dir == 1)
        {
            GetComponent<Animator>().SetTrigger("next");
        }
        else if(dir == -1)
        {
            GetComponent<Animator>().SetTrigger("prev");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
