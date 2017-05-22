using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Game_PlayerControl : MonoBehaviour
{
    public bool isControllable = true;
    public float movementSpeed = 0.5f;

    public Animator mAnimator;
    public bool animatorUse8DirectionsInstead = false;

    //private Rigidbody2D mRigidbody;
    private AudioSource mAudioSource;

    void Start()
    {
        //mRigidbody = GetComponent<Rigidbody2D>();
        mAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameData.data.playerHealth <= 0)
        {
            return;
        }

        if (isControllable)
        {
            Movement();
        }
        else
        {
            mAnimator.SetBool("move", false);
        }
    }

    void Movement()
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection += Vector2.right;
        }

        if (moveDirection.magnitude > 1f) moveDirection.Normalize();
        float moveSpeed = movementSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed /= 3f;
        }

        //Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        //mRigidbody.MovePosition((currentPosition + moveDirection) * moveSpeed);
        transform.position += new Vector3(moveDirection.x, moveDirection.y) * moveSpeed;

        AnimateSprite(moveDirection, moveDirection.magnitude > Mathf.Epsilon);

        GameData.data.playerFacingUp = moveDirection.y > Mathf.Epsilon && Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x);
        GameData.data.playerFacingDown = moveDirection.y < -Mathf.Epsilon && Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x);
        GameData.data.playerFacingLeft = moveDirection.x < -Mathf.Epsilon && Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y);
        GameData.data.playerFacingRight = moveDirection.x > Mathf.Epsilon && Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y);
    }

    void AnimateSprite(Vector2 dir, bool isMoving = false)
    {
        if (mAnimator == null) return;

        dir.Normalize();
        if (!animatorUse8DirectionsInstead)
        {
            mAnimator.SetBool("up", dir.y > Mathf.Epsilon && Mathf.Abs(dir.y) > Mathf.Abs(dir.x));
            mAnimator.SetBool("down", dir.y < -Mathf.Epsilon && Mathf.Abs(dir.y) > Mathf.Abs(dir.x));
            mAnimator.SetBool("left", dir.x < -Mathf.Epsilon && Mathf.Abs(dir.x) > Mathf.Abs(dir.y));
            mAnimator.SetBool("right", dir.x > Mathf.Epsilon && Mathf.Abs(dir.x) > Mathf.Abs(dir.y));
        }
        else
        {
            mAnimator.SetBool("up", dir.y > Mathf.Epsilon && Mathf.Abs(dir.x) < 0.6666f);
            mAnimator.SetBool("down", dir.y < -Mathf.Epsilon && Mathf.Abs(dir.x) < 0.6666f);
            mAnimator.SetBool("left", dir.x < -Mathf.Epsilon && Mathf.Abs(dir.y) < 0.6666f);
            mAnimator.SetBool("right", dir.x > Mathf.Epsilon && Mathf.Abs(dir.y) < 0.6666f);
        }

        mAnimator.SetBool("move", isMoving);
    }
    public void SetAnimatorCharacterPlayerMoveDirectionBehavior(Vector3 behavior)
    {
        SetAnimatorCharacterPlayerMoveDirectionBehavior(new Vector2(behavior.x, behavior.y));
    }

    /// <summary>
    /// Play a sound effect using this object's AudioSource component.
    /// </summary>
    /// <param name="clip">The sound effect audio clip to be played.</param>
    public void PlaySoundEffect(AudioClip clip)
    {
        // Play the sound effect. It couldn't be more simpler than this.
        if (mAudioSource != null && clip != null)
        {
            mAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("ERROR: Sound effect was not played.");
            if (clip == null)
            {
                Debug.LogError("There was no clip assigned.");
            }
            if (mAudioSource == null)
            {
                Debug.LogError("The HubWorldControl does not an AudioSource component attached.");
            }
        }
    }
}
