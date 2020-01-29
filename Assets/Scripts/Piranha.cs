using UnityEngine;
using System.Collections;
using System;


public class Piranha : Enemy
{

    public event EventHandler OutPiranhaField;


    [SerializeField] private Rigidbody2D rb;
    // [SerializeField] private Animator animator; //父类已经有初始化
    public int direction = 1;  //-1表示面朝左边

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //coll = GetComponent<Collider2D>();

        //Destroy(top.gameObject);
        //Destroy(bottom.gameObject);
    }


    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (this.direction >= 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }


    private void OnCollisionEnter2D(Collision2D other) {
        Debug.LogFormat("Enemy OnCollisionEnter2D" );  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogFormat("Enemy OnTriggerEnter2D" );  
        this.animator.SetBool("InAttackingRange", true);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // other.gameObject.SendMessage
        }
        this.animator.SetBool("InAttackingRange", false);
    }
}
