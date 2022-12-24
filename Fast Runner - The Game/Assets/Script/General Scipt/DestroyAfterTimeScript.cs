using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimeScript : MonoBehaviour
{
    [SerializeField] private float destroyTime = 0.25f;
    private float currentTime = 0f;

    private void Update()
    {
        if(currentTime < destroyTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
