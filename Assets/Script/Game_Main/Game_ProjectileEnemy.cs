using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Game_ProjectileEnemy : MonoBehaviour
{
    public float movementSpeed = 2f;
    public float distanceFromPlayerDespawn = 15f;

    public float hitboxRadius = 0.1f;
    public int projectileDamage = 1;
    public float lifeSpan = -1f;

    public string animatorLoopClipName = "loop";
    Animator anim;

	void Update ()
    {
        transform.position += transform.up * movementSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, Game_PlayerControl.control.transform.position) < hitboxRadius && GameData.data.playerHealthCurrent > 0)
        {
            Game_PlayerControl.control.TakeDamage(projectileDamage);
            Despawn();
        }
        else if (Vector3.Distance(transform.position, Game_PlayerControl.control.transform.position) > distanceFromPlayerDespawn)
        {
            Despawn();
        }
        else if (lifeSpan > 0f)
        {
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0f)
            {
                Despawn();
            }
        }
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }

    void Initialize()
    {
        if (anim == null) anim = GetComponent<Animator>();

        if (animatorLoopClipName != "") anim.Play(animatorLoopClipName);
    }

    void Start() { Initialize(); }
    void OnEnable() { Initialize(); }
}
