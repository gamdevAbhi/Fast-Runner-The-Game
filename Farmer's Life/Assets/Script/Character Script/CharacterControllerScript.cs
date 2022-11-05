using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class CharacterControllerScript : MonoBehaviour
{
    [Header("Rotation Speed")]
    [SerializeField] private float horizontalSpeed = 2f;
    [SerializeField] private float verticalSpeed = 2f;
    [SerializeField] private float maxYAxis = 180f;
    [SerializeField] private float maxXAxis = 90f;


    [Header("Transform")]
    private Transform _transform;
    [SerializeField]private Transform _cameraTransform;

    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
    }

    protected internal void HorizontalRotation(float yAxisRaw)
    {
        float rotation = horizontalSpeed * yAxisRaw * Time.deltaTime * 50f;

        if(_transform.rotation.y + rotation > maxYAxis)
        {
            _transform.Rotate(_transform.localRotation.x, maxYAxis, _transform.localRotation.z);
        }
        else if(_transform.rotation.y + rotation < -maxYAxis)
        {
            _transform.Rotate(_transform.localRotation.x, -maxXAxis, _transform.localRotation.z);
        }
        else
        {
            _transform.Rotate(_transform.localRotation.x, rotation + _transform.localRotation.y, _transform.localRotation.z);
        }
    }

    protected internal void VerticalRotation(float xAxisRaw)
    {
        float rotation = -verticalSpeed * xAxisRaw * Time.deltaTime * 50f;

        if(_cameraTransform.eulerAngles.x + rotation > 85f && _cameraTransform.eulerAngles.x + rotation < 275f)
        {
            _cameraTransform.eulerAngles = new Vector3(_cameraTransform.eulerAngles.x, _cameraTransform.eulerAngles.y, 0);
        }
        else
        {
            _cameraTransform.eulerAngles = new Vector3(rotation + _cameraTransform.eulerAngles.x, _cameraTransform.eulerAngles.y, 0);
        }
    }


}
