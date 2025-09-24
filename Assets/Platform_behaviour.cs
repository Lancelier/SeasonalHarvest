using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class Platform_behaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public bool fire_mode;
    public bool ice_mode;
    public bool jump_mode;
    private bool standing = false;
    public float max_time = 5;
    private float timer = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fire_mode)
        {
            Debug.Log("Fire_mode on");
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            
        }
        if (jump_mode)
        {
            Debug.Log("Jump_mode on");
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        if (ice_mode)
        {
            Debug.Log("Ice_mode on");
            gameObject.GetComponent<Renderer>().material.color = Color.cyan;
            if (standing)
            {
                timer+= Time.deltaTime;
                if (timer >= max_time)
                {
                    standing = false;
                    timer = 0;
                    Destroy(gameObject);
                }

            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (fire_mode)
        {
            collision.gameObject.SetActive(false);
        }
        if (jump_mode)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            }
        }
        if (ice_mode)
        {
            standing = true;
            /*Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x * 1.5f, rb.velocity.y);
            }*/
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (ice_mode && standing)
        {
            standing = false;
            /*Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x / 1.5f, rb.velocity.y);
            }*/
        }
    }

}
