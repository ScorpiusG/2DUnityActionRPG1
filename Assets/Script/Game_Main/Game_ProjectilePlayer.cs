using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Game_ProjectilePlayer : MonoBehaviour
{
    public float movementSpeed = 2f;
    public float distanceFromPlayerDespawn = 15f;

    public int projectileDamage = 1;
    public float lifeSpan = -1f;
    public bool disallowDespawnOnContact = false;

    public string animatorLoopClipName = "loop";
    Animator anim;

	void Update ()
    {
        transform.position += transform.up * movementSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, Game_PlayerControl.control.transform.position) > distanceFromPlayerDespawn)
        {
            Despawn();
        }
        if (lifeSpan > 0f)
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
