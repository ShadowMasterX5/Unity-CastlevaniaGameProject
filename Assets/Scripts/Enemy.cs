using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Enemy : Character
{
    private IEnemyState currentState;

    private Material matWhite;
    private Material matDefault;
    //private UnityEngine.Object explosionRef;
    SpriteRenderer sr;
    public GameObject Target { get; set; }

    [SerializeField]
    private float meleeRange;

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }

            return false;
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;
        //explosionRef = Resources.Load("Explosion");
        base.Start();
        ChangeState(new IdleState());
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack)
        {
            MyAnimator.SetFloat("speed", 1);

            transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
        }
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
            sr.material = matWhite;
            if (health <= 0)
            {
                KillSelf();
            }
            else
            {
                Invoke("ResetMaterial", .1f);
            }
        }
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    void ResetMaterial()
    {
        sr.material = matDefault;
    }

    private void KillSelf()
    {
        MyAnimator.SetTrigger("die");
        //GameObject explosion = (GameObject)Instantiate(explosionRef);
        //explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);
        Destroy(gameObject, 1f);
    }

    public override IEnumerator TakeDamage()
    {
        //int damageAmount = UnityEngine.Random.Range(100, 200);
        //bool isCritical = UnityEngine.Random.Range(0, 100) < 30;
        //if (isCritical) damageAmount *= 2;

        //DamageText.Create(sr.transform.position, damageAmount, isCritical);
        health -= 10;

        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetTrigger("die");
            yield return null;
        }
    }
    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }
}
