using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;
    public int curHp;
    public int maxHp;
    public int damage;

    [Header("Combat")]
    public KeyCode attackKey;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;

    private Vector2 facingDirection;

    [Header("Sprites")]
    public Sprite downSprite;
    public Sprite upSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private Rigidbody2D rig;
    private SpriteRenderer sr;
    private ParticleSystem hitEffect;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        hitEffect = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(attackKey))
        {
            if (Time.time - lastAttackTime >= attackRate)
                Attack();
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, attackRange, 1 << 8);

        if(hit.collider != null)
        {
            hit.collider.GetComponent<EnemyScript>().TakeDamage(damage);

            hitEffect.transform.position = hit.collider.transform.position;
            hitEffect.Play();
        }
    }

    private void Move()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var vel = new Vector2(x, y);

        if (vel.magnitude != 0)
            facingDirection = vel;

        UpdateSpriteDirection();

        rig.velocity = vel * moveSpeed;
    }
    
    private void UpdateSpriteDirection()
    {
        if (facingDirection == Vector2.up)
            sr.sprite = upSprite;
        else if (facingDirection == Vector2.down)
            sr.sprite = downSprite;
        else if (facingDirection == Vector2.left)
            sr.sprite = leftSprite;
        else if (facingDirection == Vector2.right)
            sr.sprite = rightSprite;
    }

    public void TakeDamage(int damageTaken)
    {
        curHp -= damageTaken;

        if (curHp <= 0)
            Die();
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}