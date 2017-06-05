using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Game_PlayerControl : MonoBehaviour
{
    public static Game_PlayerControl control;

    public bool isControllable = true;
    public float movementSpeed = 0.5f;

    public float attackMeleeCooldown = 0f;
    public float attackMeleeCooldownCurrent = 0f;

    public float attackComboTimerOnHit = 3f;
    public float attackComboTimerDecreasing = 0.1f;
    public float attackComboTimerCurrent = 0f;
    public bool attackComboTimerDecreasingState = false;
    public int attackCombo = 0;

    public Animator mAnimator;
    public bool animatorUse8DirectionsInstead = false;

    //private Rigidbody2D mRigidbody;
    private bool isStartAnimationFinished = false;
    private AudioSource mAudioSource;

    public void AddCombo(int comboAmount = 1, float comboTimerMultiplier = 1f)
    {
        attackCombo += comboAmount;
        attackComboTimerCurrent = attackComboTimerOnHit * comboTimerMultiplier;
        attackComboTimerDecreasingState = false;
    }

    private void Awake()
    {
        control = this;
    }

    void Start()
    {
        //mRigidbody = GetComponent<Rigidbody2D>();
        mAudioSource = GetComponent<AudioSource>();

        StartCoroutine(SceneStartAnimation());
    }

    IEnumerator SceneStartAnimation()
    {
        isControllable = false;
        AnimateSprite(GameData.data.playerFacing);

        yield return new WaitForSeconds(0.5f);

        isControllable = true;
        isStartAnimationFinished = true;
    }

    void Update()
    {
        ComboGauge();

        if (GameData.data.playerHealth <= 0)
        {
            return;
        }

        if (isControllable)
        {
            Movement();
            MeleeWeaponUsage();
        }
        else if (isStartAnimationFinished)
        {
            mAnimator.SetBool("up", false);
            mAnimator.SetBool("down", false);
            mAnimator.SetBool("left", false);
            mAnimator.SetBool("right", false);
        }
    }

    void ComboGauge()
    {
        if (attackCombo > 0)
        {
            attackComboTimerCurrent -= Time.deltaTime;

            if (attackComboTimerCurrent < 0f)
            {
                attackCombo--;
                attackComboTimerCurrent = attackComboTimerDecreasing;
                attackComboTimerDecreasingState = true;
            }
        }
        else
        {
            attackCombo = 0;
            attackComboTimerCurrent = 0f;
            attackComboTimerDecreasingState = false;
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

        AnimateSprite(moveDirection);

        if (moveDirection.magnitude > Mathf.Epsilon) GameData.data.playerFacing = moveDirection;
    }

    void MeleeWeaponUsage()
    {
        attackMeleeCooldown = GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].floatAttackCooldown;
        attackMeleeCooldownCurrent -= Time.deltaTime;

        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J)) && attackMeleeCooldownCurrent <= 0f)
        {
            attackMeleeCooldownCurrent = attackMeleeCooldown;
            Vector3 spawnPos = transform.position;
            Quaternion spawnRot = Quaternion.Euler(Vector3.zero);
            // Down
            if (GameData.data.playerFacing.y < -Mathf.Epsilon)
            {
                spawnPos.y -= GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].floatAreaRadius * 0.5f;
                spawnRot = Quaternion.Euler(Vector3.forward * 180);
            }
            // Left
            if (GameData.data.playerFacing.x < -Mathf.Epsilon)
            {
                spawnPos.x -= GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].floatAreaRadius * 0.5f;
                spawnRot = Quaternion.Euler(Vector3.forward * 90);
            }
            // Right
            if (GameData.data.playerFacing.x > Mathf.Epsilon)
            {
                spawnPos.x += GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].floatAreaRadius * 0.5f;
                spawnRot = Quaternion.Euler(Vector3.forward * 270);
            }
            // Up
            if (GameData.data.playerFacing.y > Mathf.Epsilon)
            {
                spawnPos.y += GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].floatAreaRadius * 0.5f;
                spawnRot = Quaternion.Euler(Vector3.zero);
            }

            Vector2 atkAoE = Vector2.zero;
            // Horizontal AoE
            if (Mathf.Abs(GameData.data.playerFacing.x) > Mathf.Abs(GameData.data.playerFacing.y - Mathf.Epsilon))
            {
                atkAoE.x = GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].floatAreaRadius * 0.5f;
                atkAoE.y = GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].floatAreaWidth * 0.5f;
            }
            // Vertical AoE
            else
            {
                atkAoE.x = GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].floatAreaWidth * 0.5f;
                atkAoE.y = GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].floatAreaRadius * 0.5f;
            }

            Game_PlayerAttackArea newAtk = Pool_PlayerAttackArea.pool.Spawn(spawnPos, spawnRot);
            newAtk.attackPower = GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].intAttackBase +
                GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].intAttackPerUpgrade * GameData.data.playerWeaponMeleeListLevel[GameData.data.playerWeaponMeleeCurrent];
            newAtk.attackAreaOfEffect = atkAoE;
            newAtk.knockbackPower = GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].floatKnockbackDistance;
        }
    }

    void AnimateSprite(Vector2 dir)
    {
        if (mAnimator == null) return;

        dir.Normalize();
        if (!animatorUse8DirectionsInstead)
        {
            mAnimator.SetBool("up", dir.y > Mathf.Epsilon && Mathf.Abs(dir.y) >= Mathf.Abs(dir.x));
            mAnimator.SetBool("down", dir.y < -Mathf.Epsilon && Mathf.Abs(dir.y) >= Mathf.Abs(dir.x));
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
