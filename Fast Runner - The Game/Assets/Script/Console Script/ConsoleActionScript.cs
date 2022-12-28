using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConsoleActionScript : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform patrolPoint;
    [SerializeField] private Transform bulletParent;

    [Header("Enemy")]
    [SerializeField] private GameObject coreFlyEnemy;
    private Vector3 resetPos;

    [Header("Script")]
    [SerializeField] private CharacterScript characterScript;
    [SerializeField] private CameraShakeScript cameraShakeScript;
    
    private Scene currentScene;
    
    private void Awake()
    {
        resetPos = playerTransform.position;
        currentScene = SceneManager.GetActiveScene();
    }

    public void ResetPos(string x)
    {
        playerTransform.position = resetPos;
    }

    public void ChangeMovementSpeed(string coord)
    {
        string[] coordToAxis = coord.Split(',');

        Vector3 vector = new Vector3(float.Parse(coordToAxis[0]), float.Parse(coordToAxis[1]), float.Parse(coordToAxis[2]));

        characterScript.SendMessage("ChangeMovement", vector);
    }

    public void ChangeSprintSpeed(string value)
    {
        float valueToFloat = float.Parse(value);

        characterScript.SendMessage("ChangeSprint", valueToFloat);
    }

    public void ChangeJumpSpeed(string value)
    {
        float valueToFloat = float.Parse(value);

        characterScript.SendMessage("ChangeJump", valueToFloat);
    }

    public void ChangeLocation(string coord)
    {
        string[] coordToAxis = coord.Split(',');

        Vector3 vector = new Vector3(float.Parse(coordToAxis[0]), float.Parse(coordToAxis[1]), float.Parse(coordToAxis[2]));

        playerTransform.localPosition += vector;
    }

    public void ChangeDrag(string value)
    {
        playerTransform.GetComponent<Rigidbody>().drag = float.Parse(value) / 10f;
    }

    public void ChangeMaxSpeed(string value)
    {
        characterScript.SendMessage("ChangeMaxSpeed", float.Parse(value));
    }

    public void ChangeDashSpeed(string value)
    {
        characterScript.SendMessage("ChangeDashSpeed", float.Parse(value));
    }

    public void ChangeDashDamage(string value)
    {
        characterScript.SendMessage("ChangeDashDamage", float.Parse(value));
    }

    public void ChangeMaxDash(string value)
    {
        characterScript.SendMessage("ChangeMaxDash", int.Parse(value));
    }

    public void CameraShake(string value)
    {
        if(value == "ON")
        {
            cameraShakeScript.ChangeCameraShake(true);
        }
        else if(value == "OFF")
        {
            cameraShakeScript.ChangeCameraShake(false);
        }
    }

    public void ChangeOffsetSpeed(string value)
    {
        characterScript.SendMessage("ChangeOffsetSpeed", float.Parse(value));
    }

    public void ResetLevel(string x)
    {
        SceneManager.LoadSceneAsync(currentScene.name, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void WallRunSpeed(string value)
    {
        characterScript.SendMessage("ChangeWallRunSpeed", float.Parse(value));
    }

    public void SpawnEnemy(string value)
    {
        GameObject enemy = null;

        if(value == "COREFLY")
        {
            enemy = Instantiate(coreFlyEnemy, patrolPoint.GetChild(0).position, Quaternion.identity, enemyParent);

            enemy.GetComponent<CoreFlyEnemyScript>().Initialized(patrolPoint, playerTransform, bulletParent);
        }
    }

    public void Quit(string x)
    {
        Application.Quit();
    }
}