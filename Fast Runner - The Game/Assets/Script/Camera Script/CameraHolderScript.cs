using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolderScript : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform targetHead;
    [SerializeField] private bool shouldFollow = true;

    private void Update()
    {
        if(shouldFollow == true)
        {
            gameObject.GetComponent<Transform>().position = targetHead.position;
            gameObject.GetComponent<Transform>().eulerAngles = new Vector3(gameObject.GetComponent<Transform>().eulerAngles.x, targetHead.eulerAngles.y, 0f);
        }
    }
}
