using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToglePostProcessingEffect : MonoBehaviour
{
    public static ToglePostProcessingEffect instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnablePostProcessing()
    {
        gameObject.SetActive(true);
        
    }
    public void DisablePostProcessing()
    {
        gameObject.SetActive(false);

    }
}
