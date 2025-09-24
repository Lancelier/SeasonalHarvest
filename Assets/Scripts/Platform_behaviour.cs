using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class Platform_behaviour : MonoBehaviour
{
    public enum Season
    {
        Spring,
        Summer,
        Winter,
    }

    [SerializeField] private Season season;
    [SerializeField] private float winterPlatformDelayToBreak;
    [SerializeField] private float winterPlatformRespawnDelay;

    [Space]
    [Header("Colors")]
    [SerializeField] private Color springColor;
    [SerializeField] private Color summerColor;
    [SerializeField] private Color winterColor;

    private float timer = 0;
    private bool stoodOnWinterBlock = false;
    private bool isWinterBlockDestroyed = false;

    private BoxCollider2D boxCollider2D;
    private SpriteRenderer sprite;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }


    void Update()
    {
        if (season == Season.Spring)
        {
            sprite.material.color = springColor;
        }

        if (season == Season.Summer)
        {
            sprite.material.color = summerColor;
            
        }

        if (season == Season.Winter)
        {
            sprite.material.color = winterColor;

            if (stoodOnWinterBlock)
            {
                timer+= Time.deltaTime;
                if (timer >= winterPlatformDelayToBreak)
                {
                    boxCollider2D.enabled = false;
                    sprite.enabled = false;

                    timer = 0;
                    stoodOnWinterBlock = false;
                    isWinterBlockDestroyed = true;
                }
            }
            else if (isWinterBlockDestroyed)
            {
                timer += Time.deltaTime;
                if(timer >= winterPlatformRespawnDelay)
                {
                    boxCollider2D.enabled = true;
                    sprite.enabled = true;

                    timer = 0;
                    stoodOnWinterBlock = false;
                    isWinterBlockDestroyed = false;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (season == Season.Spring)
        {
            collision.gameObject.TryGetComponent(out Rigidbody2D rb);
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }

        if (season == Season.Summer)
        {
            //collision.gameObject.SetActive(false);
        }

        if (season == Season.Winter)
        {
            stoodOnWinterBlock = true;
        }

    }

}
