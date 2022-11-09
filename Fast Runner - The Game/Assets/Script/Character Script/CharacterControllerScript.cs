using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class CharacterControllerScript : MonoBehaviour
{
    [Header("Rotation Speed")]
    [SerializeField] private float horizontalSpeed = 2f;
    [SerializeField] private float verticalSpeed = 2f;
    [SerializeField] private float[] maxYAxis;
    [SerializeField] private float[] maxXAxis;

    [Header("Transform")]
    private Transform _transform;
    [SerializeField] private Transform _camera;

    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
    }

    protected internal void Rotate(Vector2 axis)
    {
        Vector3 rotate = new Vector3(-verticalSpeed * axis.x * Time.deltaTime * 50f, horizontalSpeed * axis.y * Time.deltaTime * 50f, 0f);

        float finalRotationY = _transform.eulerAngles.y + rotate.y;
        float finalRotationX = _camera.eulerAngles.x + rotate.x;

        if(finalRotationX > maxXAxis[0] && finalRotationX < maxXAxis[1])
        {
            finalRotationX -= rotate.x;
        }
        if(finalRotationY > maxYAxis[0] && finalRotationY < maxYAxis[1])
        {
            finalRotationY -= rotate.y;
        }

        _transform.eulerAngles = new Vector3( _transform.eulerAngles.x, finalRotationY, 0f);
        _camera.eulerAngles = new Vector3(finalRotationX, _camera.eulerAngles.y, 0f);
    }
}
