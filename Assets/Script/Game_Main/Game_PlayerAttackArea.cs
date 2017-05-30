using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_PlayerAttackArea : MonoBehaviour
{
    public int attackPower = 0;
    public Vector2 attackAreaOfEffect = Vector2.zero;
    public float knockbackPower = 0;
    public List<string> listStringOther = new List<string>();

    public GameObject[] spriteHitAreaDebug;
    private const bool showHitAreaDebug = true;

    private IEnumerator HitEnemy()
    {
#if UNITY_EDITOR
        yield return null;
        if (showHitAreaDebug)
        {
            Vector3 corner1 = transform.position + attackAreaOfEffect.x * Vector3.right + attackAreaOfEffect.y * Vector3.up;
            Vector3 corner2 = transform.position + attackAreaOfEffect.x * Vector3.right + attackAreaOfEffect.y * Vector3.down;
            Vector3 corner3 = transform.position + attackAreaOfEffect.x * Vector3.left + attackAreaOfEffect.y * Vector3.down;
            Vector3 corner4 = transform.position + attackAreaOfEffect.x * Vector3.left + attackAreaOfEffect.y * Vector3.up;
            spriteHitAreaDebug[0].SetActive(true);
            spriteHitAreaDebug[0].transform.position = corner1;
            spriteHitAreaDebug[1].SetActive(true);
            spriteHitAreaDebug[1].transform.position = corner2;
            spriteHitAreaDebug[2].SetActive(true);
            spriteHitAreaDebug[2].transform.position = corner3;
            spriteHitAreaDebug[3].SetActive(true);
            spriteHitAreaDebug[3].transform.position = corner4;
        }
#else
        foreach (GameObject x in spriteHitAreaDebug) if (x != null) Destroy(x);
#endif
        yield return null;
        foreach (Game_EnemyCore x in Game_GameControl.control.objectEnemyList)
        {
            if (x.transform.position.x - x.hitboxDamage.x < transform.position.x + attackAreaOfEffect.x &&
                x.transform.position.x + x.hitboxDamage.x > transform.position.x - attackAreaOfEffect.x &&
                x.transform.position.y - x.hitboxDamage.y < transform.position.y + attackAreaOfEffect.y &&
                x.transform.position.y + x.hitboxDamage.y > transform.position.y - attackAreaOfEffect.y )
            {
                x.TakeDamage(attackPower, (x.transform.position - transform.position).normalized * knockbackPower);
            }
        }

        foreach (GameObject x in spriteHitAreaDebug) x.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine("HitEnemy");
    }
    private void OnEnable()
    {
        StartCoroutine("HitEnemy");
    }

    /*
    void OnDrawGizmos()
    {
        Vector3 corner1 = transform.position + attackAreaOfEffect.x * Vector3.right + attackAreaOfEffect.y * Vector3.up;
        Vector3 corner2 = transform.position + attackAreaOfEffect.x * Vector3.right + attackAreaOfEffect.y * Vector3.down;
        Vector3 corner3 = transform.position + attackAreaOfEffect.x * Vector3.left + attackAreaOfEffect.y * Vector3.down;
        Vector3 corner4 = transform.position + attackAreaOfEffect.x * Vector3.left + attackAreaOfEffect.y * Vector3.up;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(corner1, corner2);
        Gizmos.DrawLine(corner2, corner3);
        Gizmos.DrawLine(corner3, corner4);
        Gizmos.DrawLine(corner4, corner1);
    }
    */
}