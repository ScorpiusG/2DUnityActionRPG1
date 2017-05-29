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
        FollowBetweenPlayerAndTarget,
        MoveToPoint
    }

    public static Game_GameControl control;

    public Camera cameraMain;
    public CameraState cameraMainState = CameraState.FollowPlayer;
    public Vector3 positionCameraMovePoint = Vector3.zero;
    public float floatCameraLerpRate = 16f;
    public GameObject objectCameraMainTarget;

    public GameObject objectPlayer;

    public Game_EnemyCore objectPlayerTarget = null;
    public GameObject objectPlayerTargetReticle;
    public Animator animatorPlayerTargetReticle;
    private Game_EnemyCore[] objectEnemyList;
    public float floatPlayerTargetRange = 3f;
    public int intTargetUpdateDelayFrames = 3;
    private int intTargetUpdateDelayFramesCurrent = 0;

    void Start()
    {
        control = this;
        objectPlayerTargetReticle.SetActive(false);

        objectEnemyList = FindObjectsOfType<Game_EnemyCore>();

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

        // Anything below this will not run if player is "dead".
        if (GameData.data.playerHealth <= 0)
        {
            return;
        }

        PlayerTargeting();
        EnergyGeneration();
    }

    void PlayerTargeting()
    {
        // Refresh and use targeting system once every few frames.
        if (intTargetUpdateDelayFramesCurrent == 0)
        {
            foreach (Game_EnemyCore x in objectEnemyList)
            {
                if (x.hitpointCurrent > 0 && Vector3.Distance(objectPlayer.transform.position, x.transform.position) < floatPlayerTargetRange)
                {
                    // Check for each living enemy within range.
                    if (objectPlayerTarget == null)
                    {
                        // If there was no target before, set this one as the target.
                        objectPlayerTarget = x;
                    }
                    else if (objectPlayerTarget != x &&
                        Vector3.Distance(objectPlayer.transform.position, x.transform.position) < Vector3.Distance(objectPlayer.transform.position, objectPlayerTarget.transform.position))
                    {
                        // If there is a closer target, set this one as the target instead.
                        objectPlayerTarget = x;
                    }
                }
            }
        }
        // Check current target.
        if (objectPlayerTarget != null)
        {
            // Check distance
            if (objectPlayerTarget.hitpointCurrent > 0 && Vector3.Distance(objectPlayer.transform.position, objectPlayerTarget.transform.position) < floatPlayerTargetRange)
            {
                // Set reticle onto the target
                if (!objectPlayerTargetReticle.activeSelf) objectPlayerTargetReticle.SetActive(true);
                objectPlayerTargetReticle.transform.SetParent(objectPlayerTarget.transform);
                objectPlayerTargetReticle.transform.localPosition = Vector3.zero;
                objectPlayerTargetReticle.transform.localScale = Vector3.one;
                // Animate target reticle begin animation
                if (animatorPlayerTargetReticle != null)
                {
                    animatorPlayerTargetReticle.SetBool("on", true);
                }
            }
            else
            {
                // Target becomes null if its too far from player or is no longer living.
                objectPlayerTarget = null;
                // Animate target reticle end animation
                if (animatorPlayerTargetReticle != null)
                {
                    animatorPlayerTargetReticle.SetBool("on", false);
                }
            }
        }

        // Move frame forward.
        intTargetUpdateDelayFramesCurrent++;
        if (intTargetUpdateDelayFramesCurrent >= intTargetUpdateDelayFrames)
        {
            intTargetUpdateDelayFramesCurrent = 0;
        }
    }

    void EnergyGeneration()
    {
        // Regenerate energy and cap energy at 100.
        GameData.data.playerEnergy += Time.deltaTime;
        if (GameData.data.playerEnergy > 100f) GameData.data.playerEnergy = 100f;
    }

    void CameraMovement()
    {
        // Move camera based on state and target/position.
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
            case CameraState.FollowBetweenPlayerAndTarget:
                if (objectPlayer != null && objectCameraMainTarget != null) positionCameraMovePoint = (objectPlayer.transform.position + objectCameraMainTarget.transform.position) / 2;
                else if (objectPlayer != null) positionCameraMovePoint = objectPlayer.transform.position;
                else if (objectCameraMainTarget != null) positionCameraMovePoint = objectCameraMainTarget.transform.position;
                else return;
                break;
        }

        positionCameraMovePoint.z = -1f;
        cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, positionCameraMovePoint, floatCameraLerpRate * Time.deltaTime);
    }
}
