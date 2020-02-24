using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;
    public int curHp;
    public int maxHp;
    public int xpToGive;

    [Header("Attack")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;

    [Header("Target")]
    public float chaseRange;
    public float attackRange;
    public PlayerScript player;

    private Rigidbody2D rig;
    private SpriteRenderer sr;

    void Awake()
    {
        player = FindObjectOfType<PlayerScript>();
        rig = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        var playerDistance = Vector2.Distance(transform.position, player.transform.position);

        if (playerDistance <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackRate)
                Attack();

            rig.velocity = Vector2.zero;
        } else if (playerDistance <= chaseRange)
        {
            Chase();
        }
        else
        {
            rig.velocity = Vector2.zero;
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;

        player.TakeDamage(damage);
    }

    private void Chase()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;

        rig.velocity = dir * moveSpeed;
    }

    public void TakeDamage(int damageTaken)
    {
        curHp -= damageTaken;

        if (curHp <= 0)
            Die();
    }

    private void Die()
    {
        player.AddXp(xpToGive);
        Destroy(gameObject);
    }
}
