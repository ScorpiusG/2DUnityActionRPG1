using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_GameControl : MonoBehaviour
{
    public enum CameraState
    {
        Static,
        FollowPlayer,
        FollowTarget,
        MoveToPoint
    }

    public static Game_GameControl control;

    public Camera cameraMain;
    public CameraState cameraMainState = CameraState.FollowPlayer;
    public Vector3 positionCameraMovePoint = Vector3.zero;
    public float floatCameraLerpRate = 16f;
    public GameObject objectCameraMainTarget;

    public GameObject objectPlayer;

    void Start()
    {
        control = this;

        /*
        if (control == null)
        {
            control = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        */
    }

    void Update()
    {
        CameraMovement();

        if (GameData.data.playerHealth <= 0)
        {
            return;
        }

        EnergyGeneration();
    }

    void EnergyGeneration()
    {
        GameData.data.playerEnergy += Time.deltaTime;
        if (GameData.data.playerEnergy > 100f) GameData.data.playerEnergy = 100f;
    }

    void CameraMovement()
    {
        switch (cameraMainState)
        {
            default:
            case CameraState.Static:
                return;
            case CameraState.FollowPlayer:
                if (objectPlayer != null) positionCameraMovePoint = objectPlayer.transform.position;
                else return;
                break;
            case CameraState.MoveToPoint:
                break;
            case CameraState.FollowTarget:
                if (objectCameraMainTarget != null) positionCameraMovePoint = objectCameraMainTarget.transform.position;
                else return;
                break;
        }

        positionCameraMovePoint.z = -1f;
        cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, positionCameraMovePoint, floatCameraLerpRate * Time.deltaTime);
    }
}
