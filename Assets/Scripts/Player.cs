using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpforce;

    public Rigidbody2D MyRigidbody { get; set; }

    public bool Crouch { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;

    private bool flashActive;
    [SerializeField]
    private float flashLength = 0f;
    private float flashCounter = 0f;
    private SpriteRenderer playerSprite;

    public Ghost ghost;

    //public CameraFollow cF;


    private UnityEngine.Object dashEffect;

    // Start is called before the first frame update
    public override void Start()  {
        base.Start();
        MyRigidbody = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()    {

        if (!TakingDamage && !IsDead)
        {
            HandleInput();
        }
    }

    // Update is called once per frame
    void FixedUpdate()    {

        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");

            OnGround = IsGrounded();

            HandleMovement(horizontal);

            Flip(horizontal);

            HandleLayers();

            Dash(horizontal); 
        }

        if (flashActive)
        {
            if (flashCounter > flashLength * .99f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .82f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            }
            else if (flashCounter > flashLength * .66f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .49f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            }
            else if (flashCounter > flashLength * .33f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .16f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            }
            else if (flashCounter > 0f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
                flashActive = false;
            }
            flashCounter -= Time.deltaTime;
        }
    }

    private void Dash(float horizontal)
    {

            if (dashTime <= 0)
            {
                Flip(horizontal);
                dashTime = startDashTime;
            }
            else
            {
                dashTime -= Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.O))
                {
                dashEffect = Resources.Load("Particle System");
                MyRigidbody.velocity = new Vector2(horizontal * dashSpeed, MyRigidbody.velocity.y);
                GameObject dash = (GameObject)Instantiate(dashEffect);
                dash.transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);
                Destroy(dash, 1f);

            }
            }
        }
    

    private void HandleMovement(float horizontal){

        if(MyRigidbody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }
        if(!Attack && !Crouch && (OnGround || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }
        if(Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpforce));
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            MyAnimator.SetTrigger("attack");
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            MyAnimator.SetTrigger("crouch");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MyAnimator.SetTrigger("jump");
        }

    }

    private void Flip(float horizontal)
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ghost.makeGhost = true;
            ChangeDirection();
        }
        else if(horizontal == 0)
        {
            ghost.makeGhost = false;
        }
    }

    private bool IsGrounded()
    {
        if(MyRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if(!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;
        flashActive = true;
        flashCounter = flashLength;
        if (!IsDead)
        {
            
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
            MyAnimator.SetTrigger("die"); 
        }
        yield return null;
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }
}
