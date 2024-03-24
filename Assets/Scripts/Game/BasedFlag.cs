using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasedFlag : MonoBehaviour
{
    public UnityEvent playerFlagCaptureEvent;
    public UnityEvent enemyFlagCaptureEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (this.CompareTag("Player Flag") && other.CompareTag("Player"))
        {
            playerFlagCaptureEvent.Invoke();
            this.gameObject.SetActive(false);
        }
        
        if(this.CompareTag("Enemy Flag") && other.CompareTag("Enemy"))
        {
            enemyFlagCaptureEvent.Invoke();
        }
    }
}
