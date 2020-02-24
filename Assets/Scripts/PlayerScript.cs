using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;
    public int curHp;
    public int maxHp;
    public int damage;
    public float interactRange;
    public List<string> Inventory = new List<string>();

    [Header("Combat")]
    public KeyCode attackKey;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;

    [Header("Experience")] 
    public int currentLevel;
    public int currentXp;
    public int xpToNextLevel;
    public float levelXpModifier;

    private Vector2 facingDirection;

    [Header("Sprites")]
    public Sprite downSprite;
    public Sprite upSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private Rigidbody2D rig;
    private SpriteRenderer sr;
    private ParticleSystem hitEffect;
    private PlayerUIScript playerUI;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        hitEffect = gameObject.GetComponentInChildren<ParticleSystem>();
        playerUI = FindObjectOfType<PlayerUIScript>();
    }

    private void Start()
    {
        playerUI.UpdateXpBarFill();
        playerUI.UpdateLevelText();
        playerUI.UpdateHealthBarFill();
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(attackKey))
        {
            if (Time.time - lastAttackTime >= attackRate)
                Attack();
        }

        CheckInteract();
    }

    private void CheckInteract()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, interactRange, 1 << 9);

        if (hit.collider != null)
        {
            var interactable = hit.collider.gameObject.GetComponent<Interactable>();
            playerUI.SetInteractText(hit.collider.transform.position, interactable.InteractionDescription);

            if (Input.GetKeyDown(attackKey))
                interactable.Interact();
        }
        else
        {
            playerUI.DisableInteractText();
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
        playerUI.UpdateHealthBarFill();

        if (curHp <= 0)
            Die();
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void AddXp(int xp)
    {
        currentXp += xp;
        playerUI.UpdateXpBarFill();

        if (currentXp > xpToNextLevel)
            LevelUp();
    }

    private void LevelUp()
    {
        currentXp -= xpToNextLevel;
        currentLevel++;

        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * levelXpModifier);

        playerUI.UpdateLevelText();
        playerUI.UpdateXpBarFill();
    }

    public void AddToInventory(string item)
    {
        Inventory.Add(item);
        playerUI.UpdateInventory();
    }
}