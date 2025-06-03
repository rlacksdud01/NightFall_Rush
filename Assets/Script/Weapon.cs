using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;

    public float damage;
    public int count;
    public float speed;
    public float dmgTmp;
    public float speedTmp;

    public float timer;
    public float secTimer;
    public float thirdTimer;
    Player player;

    private Transform playerTransform;

    void Awake()
    {
        player = GameManager.instance.player;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        //dmgTmp = this.damage;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);

                timer += Time.deltaTime;

                if (timer > 1)
                {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Axe);
                    timer = 0;
                }
                
                break;

            case 1:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }

                break;

            case 2:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Sword);
                }
                break;

            case 3:

                timer += Time.deltaTime;               

                if (timer >= speed)
                {
                    secTimer += Time.deltaTime;
                    transform.Find("Garlic(Clone)").gameObject.GetComponent<CircleCollider2D>().enabled = true;
                    if (secTimer > 0.5f)
                    {
                        timer = 0;
                        secTimer = 0f;
                    }
                }

                else transform.Find("Garlic(Clone)").gameObject.GetComponent<CircleCollider2D>().enabled = false;

                break;

            case 4:
                timer += Time.deltaTime;

                if (timer >= speed)
                {
                    timer = 0f;

                    Vector3 playerPosition = GameManager.instance.player.transform.position;

                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Lightning);

                    for (int i = 0; i < count; i++)
                    {
                        Transform lightning = GameManager.instance.pool.Get(prefabId).transform;

                        Vector3 randomPosition = new Vector3(Random.Range(playerPosition.x - 8f, playerPosition.x + 8f), Random.Range(playerPosition.y - 8f, playerPosition.y + 8f), 0f);

                        lightning.position = randomPosition;
                        lightning.rotation = Quaternion.identity;

                        StartCoroutine(DeactivateLightning(lightning, 0.5f));

                        lightning.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
                    }
                }
                break;

            case 5:
                thirdTimer += Time.deltaTime;
                if (thirdTimer >= speed)
                {
                    thirdTimer = 0f;

                    Vector3 playerPosition = GameManager.instance.player.transform.position;

                    Transform bomb = GameManager.instance.pool.Get(prefabId).transform;

                    Vector3 randomPosition = new Vector3(Random.Range(playerPosition.x - 10f, playerPosition.x + 10f), Random.Range(playerPosition.y - 10f, playerPosition.y + 10f), 0f);

                    bomb.position = randomPosition;
                    bomb.rotation = Quaternion.identity;

                    StartCoroutine(DeactivateLightning(bomb, 1f));

                    bomb.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
                }
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;
        this.speed = speedTmp;

        if (id == 0)
        {
            SetWeapon();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150;
                speedTmp = 150;
                SetWeapon();

                break;

            case 1:
                speed = 0.3f;
                speedTmp = 0.3f;
                break;

            case 2:
                speed = 7f;
                speedTmp = 7f;
                break;

            case 3:
                speed = 1f;
                speedTmp = 1f;
                SetWeapon();
                break;

            case 4:
                speed = 3;
                speedTmp = 3f;
                SetWeapon();
                break;

            case 5:
                speed = 10;
                speedTmp = 10;
                SetWeapon();
                break;
            default:
                
                break;
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void SetWeapon()
    {
        switch(id)
        {
            case 0:
                for (int index = 0; index < count; index++)
                {
                    Transform weapon;

                    if (index < transform.childCount)
                    {
                        weapon = transform.GetChild(index);
                    }

                    else
                    {
                        weapon = GameManager.instance.pool.Get(prefabId).transform;
                        weapon.parent = transform;
                    }

                    weapon.localPosition = Vector3.zero;
                    weapon.localRotation = Quaternion.identity;

                    Vector3 rotVec = Vector3.forward * 360 * index / count;
                    weapon.Rotate(rotVec);
                    weapon.Translate(weapon.up * 1.5f, Space.World);

                    weapon.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
                }

                break;

            case 3:
                for (int index = 0; index < count; index++)
                {
                    Transform weapon;

                    if (index < transform.childCount)
                    {
                        weapon = transform.GetChild(index);
                    }

                    else
                    {
                        weapon = GameManager.instance.pool.Get(prefabId).transform;
                        weapon.parent = transform;
                    }

                    weapon.localPosition = Vector3.zero;
                    weapon.localRotation = Quaternion.identity;

                    weapon.Translate(weapon.localPosition, Space.World);

                    weapon.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
                }

                break;

            case 4:
                for (int i = 0; i < count; i++)
                {
                    Vector3 playerPos = GameManager.instance.player.transform.position;

                    Transform lightning = GameManager.instance.pool.Get(prefabId).transform;

                    Vector3 randomPos = new Vector3(Random.Range(playerPos.x - 8f, playerPos.x + 8f), Random.Range(playerPos.y - 8f, playerPos.y + 8f), 0f);

                    lightning.position = randomPos;
                    lightning.rotation = Quaternion.identity;

                    StartCoroutine(DeactivateLightning(lightning, 0.5f));

                    lightning.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
                }                    
                break;

            case 5:
                Vector3 playerPosition = GameManager.instance.player.transform.position;

                Transform bomb = GameManager.instance.pool.Get(prefabId).transform;

                Vector3 randomPosition = new Vector3(Random.Range(playerPosition.x - 5f, playerPosition.x + 5f), Random.Range(playerPosition.y - 5f, playerPosition.y + 5f), 0f);

                bomb.position = randomPosition;
                bomb.rotation = Quaternion.identity;

                StartCoroutine(DeactivateLightning(bomb, 1f));

                bomb.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);

                break;
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform weapon = GameManager.instance.pool.Get(prefabId).transform;
        weapon.position = transform.position;
        weapon.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        weapon.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.EnergyBall);
    }

    IEnumerator DeactivateLightning(Transform lightning, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (lightning != null)
        {            
            GameManager.instance.pool.Return(lightning.gameObject);
        }
    }
}
