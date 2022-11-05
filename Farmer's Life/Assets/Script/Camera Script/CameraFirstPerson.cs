using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFirstPerson : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform target;

    private void Update()
    {
        gameObject.GetComponent<Transform>().position = target.position;
        gameObject.GetComponent<Transform>().eulerAngles = new Vector3(gameObject.GetComponent<Transform>().eulerAngles.x, target.eulerAngles.y, 0f);
    }
}
