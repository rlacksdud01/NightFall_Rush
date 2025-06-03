using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;

    public float hp;
    public float speed;
    public Scanner scanner;

    public float attackTick;
    public float hitTick;
    public bool onHit = false;
    public bool changeSprite = false;
    public List<string> hitables = new List<string> { "Enemy", "PlayerHitable", "AllHitable" };

    Rigidbody2D rigid;
    SpriteRenderer sprite;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        ChangeSprite();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        if (inputVec.x > 0)
        {
            sprite.flipX = true;
        }

        else if (inputVec.x < 0)
        {
            sprite.flipX = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive) return;

        if (GameManager.instance.hp < 1)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            GameManager.instance.GameOver();
        }

        if (hitables.Contains(collision.gameObject.tag))
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                Hit((int)enemy.power);
            }
            else Hit(1);
        }
    }
    void Hit(int damage)
    {
        if (onHit) return;
        hitTick = 0f;
        onHit = true;
        if (damage > 0) changeSprite = true;
        GameManager.instance.hp -= damage;
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
        Invoke("HitOut", 1f);
    }

    void HitOut()
    {
        hitTick = 0;
        sprite.color = Color.white;
        onHit = false;
        changeSprite = false;
    }

    void ChangeSprite()
    {
        if (changeSprite)
        {
            hitTick += Time.deltaTime;
            sprite.color = new Color(1, 1, 1) * (Mathf.Cos(hitTick * 18.5f) * 0.3f + 1) + new Color(0, 0, 0, 1);
        }
    }
}
