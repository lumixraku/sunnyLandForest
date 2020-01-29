using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    // Use this for initialization
    protected Animator animator;
    protected AudioSource deathAudio;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        //deathAudio = GetComponent<AudioSource>();
    }

    public void Death()
    {
        GetComponent<Collider2D>().enabled = false;
        Debug.LogFormat(" {0}  dead!", gameObject.name);  
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        
        animator.SetTrigger("death");
        //deathAudio.Play();
    }
}
