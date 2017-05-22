using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Game_Projectile : MonoBehaviour
{
    public float movementSpeed = 2f;
    public Vector2 boundaryDespawn = new Vector2(9.5f, 10f);

    public string animatorLoopClipName = "loop";
    Animator anim;

	void Update ()
    {
        transform.position += transform.up * movementSpeed * Time.deltaTime;

        if(Mathf.Abs(transform.position.x) > boundaryDespawn.x || Mathf.Abs(transform.position.y) > boundaryDespawn.y || transform.position.y < -0.5f)
        {
            Despawn();
        }
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }

    void Initialize()
    {
        if (anim == null) anim = GetComponent<Animator>();

        anim.Play(animatorLoopClipName);
    }

    void Start() { Initialize(); }
    void OnEnable() { Initialize(); }
}
