using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy instance;

    public float hp;
    public float Maxhp;
    public float speed;
    public float power;

    public Rigidbody2D target;

    public float hitTick;
    public bool onHit = false;
    public bool changeSprite = false;

    bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Collider2D coll;

    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        ChangeSprite();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;

        if (!isLive) return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive) return;

        if (!isLive) return;

        sprite.flipX = target.position.x > rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        hp = Maxhp;
        coll.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Weapon") || !isLive) return;

        hp -= collision.GetComponent<Bullet>().damage;

        if (hp > 0) 
        {
            if (onHit) return;
            hitTick = 0f;
            onHit = true;
            changeSprite = true;

            if (target.transform.position.x > transform.position.x && target.transform.position.y > transform.position.y)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1000f, -1000f));
            }

            else if (target.transform.position.x > transform.position.x && target.transform.position.y < transform.position.y)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1000f, 1000f));
            }

            else if (target.transform.position.x < transform.position.x && target.transform.position.y < transform.position.y)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000f, 1000));
            }

            else if (target.transform.position.x < transform.position.x && target.transform.position.y > transform.position.y)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000f, -1000));
            }

            else if (target.transform.position.x > transform.position.x && target.transform.position.y == transform.position.y)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1000f, 0));
            }

            else if (target.transform.position.x < transform.position.x && target.transform.position.y == transform.position.y)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000f, 0));
            }

            else if (target.transform.position.x == transform.position.x && target.transform.position.y < transform.position.y)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000));
            }

            else if (target.transform.position.x == transform.position.x && target.transform.position.y > transform.position.y)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1000));
            }

            Invoke("HitOut", 0.5f);

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }

        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            sprite.sortingOrder = 1;
            Dead();

            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
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

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
