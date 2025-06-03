using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;
    public float timer;
    //public float hpTmp = 30;

    public void Init(ItemData data)
    {
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.ASpeed:
                ASpeedUp();
                break;

            case ItemData.ItemType.MSpeed:
                MSpeedUp();
                break;

            case ItemData.ItemType.Power:
                PowerUp();
                break;

            case ItemData.ItemType.HpRegen:
                HpRegen();
                break;

            case ItemData.ItemType.MaxHp:
                MaxHp();
                break;
        }
    }

    void ASpeedUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0:
                    weapon.speed = weapon.speedTmp + (150 * rate);
                    break;

                default:
                    weapon.speed =  weapon.speedTmp * rate;
                    break;
            }
        }
    }

    void MSpeedUp()
    {
        float speed = 5;
        GameManager.instance.player.speed = speed + (speed * rate);
    }

    void PowerUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                default:                    
                    weapon.damage = weapon.dmgTmp * rate;  //기본 공격력 + rate만큼 증가
                    break;
            }
        }
    }

    void HpRegen()
    {
        if (GameManager.instance.hp < GameManager.instance.maxHp)
        {
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                GameManager.instance.hp = GameManager.instance.hp + rate;
                timer = 0f;
            }
        }
    }

    void MaxHp()
    {
        GameManager.instance.maxHp = GameManager.instance.maxHp + rate;
        GameManager.instance.hp = GameManager.instance.hp + rate;
    }
}
