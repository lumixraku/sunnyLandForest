using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    public Collider2D coll;
    public LayerMask ground;  //指的layer  而不是 sort layer
    public Text textCherryCount;
    public AudioSource jumpAudio;
    //public ScenceManager

    public float speed;
    public float jumpForce;

    private bool justInjure;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        movement();
        SwitchAnimation();
    }
    // Start is called before the first frame update
    void movement()
    {
        float horizonInputValue = Input.GetAxis("Horizontal");
        float verticalInputValue = Input.GetAxis("Vertical");
        float faceDirect = Input.GetAxisRaw("Horizontal"); // only -1 and 1 and 0
        // Debug.LogFormat(" {0} {1} GetAxisRaw !", faceDirect, horizonInputValue);  
        if (justInjure)
        {
            return;
        }
        if (horizonInputValue != 0.0f)
        {
            rb.velocity = new Vector2(horizonInputValue * speed * Time.fixedDeltaTime, rb.velocity.y);
            // animator.SetFloat("running", Mathf.Abs(faceDirect));
        }

        // 朝向
        if (horizonInputValue < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (horizonInputValue > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        // 有时候不准  
        // if (faceDirect != 0)
        // {
        //     transform.localScale = new Vector3(faceDirect, 1, 1);
        // }

        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            // animator.SetBool("jumping", true);
            // jumpAudio.Play();
        }
    }

    private void SwitchAnimation()
    {
        couldRecover();
    }

    void couldRecover() 
    {
        if ( !justInjure ) {
            animator.SetBool("getInjure", false);
        }
    }


    // 碰到设置了 isTrigger Collision 的物体会触发
    // 结论： OnCollisionEnter方法必须是在两个碰撞物体都不勾选isTrigger的前提下才能进入，反之只要勾选一个isTrigger那么就能进入OnTriggerEnter方法。 OnCollisionEnter和OnTriggerEnter是冲突的不能同时存在的。
    // https://gameinstitute.qq.com/community/detail/127572
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.LogFormat("Player OnTriggerEnter2D");
        if (collision.CompareTag("Collection"))
        {
            //Destroy(collision.gameObject);
            // this.collectionFeedback(collision);
            // cherryCount = cherryCount + 1;
            // textCherryCount.text = "" + cherryCount;
        }

        // 掉到世界范围外
        if (collision.CompareTag("DeadLine"))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // 声音全部关闭
            AudioSource[] audios = GetComponents<AudioSource>();
            for (int i = audios.Length - 1; audios.Length > 0 && i >= 0; i--)
            {
                audios[i].enabled = false;
            }
            // Invoke("Restart", 1f);
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.LogFormat("Player OnCollisionEnter2D ");
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (animator.GetBool("falling"))
            {
                enemy.JumpOn();
                //Destroy(collision.gameObject);
                // 杀死敌人还有反弹跳跃的效果
                float shrinkJump = 0.5f;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * shrinkJump * Time.fixedDeltaTime);
                animator.SetBool("jumping", true);
            }
            else
            {
                justInjure = true;
                animator.SetBool("getInjure", justInjure);
                Invoke("afterJustInjure", 0.5f);
                //这里都是玩家受到伤害  //受到伤害会往行进的反反向弹开
                if (transform.position.x < collision.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(-1f * speed * Time.fixedDeltaTime, rb.velocity.y);
                }
                if (transform.position.x > collision.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
                }

            }
        }
    }

    private void afterJustInjure() 
    {   
        justInjure = false;

    }

}
