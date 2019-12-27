using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float movementSpeed;
    protected bool facingRight;
    [SerializeField]
    protected int health;
    [SerializeField]
    private EdgeCollider2D swordCollider;
    [SerializeField]
    private List<string> damageSources;
    public abstract bool IsDead { get; }
    public bool TakingDamage { get; set; }
    public bool Attack { get; set; }
    public Animator MyAnimator { get; private set; }
    public EdgeCollider2D SwordCollider
    {
        get
        {
          return swordCollider;
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        facingRight = true;
        MyAnimator = GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public abstract IEnumerator TakeDamage();
    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }
    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSources.Contains(other.tag))
        { 
            StartCoroutine(TakeDamage()); 
        }
    }

}
