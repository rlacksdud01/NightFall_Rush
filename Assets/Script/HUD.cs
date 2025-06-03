using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health, RemainHp}
    public InfoType type;

    Text myText;
    Slider mySlider;

    float timer;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;

            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level) ;
                break;

            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;

            case InfoType.Health:
                float curHp = GameManager.instance.hp;
                float maxHp = GameManager.instance.maxHp;
                mySlider.value = curHp / maxHp;
                break;

            case InfoType.RemainHp:
                float remainHp = GameManager.instance.hp;
                myText.text = string.Format("{0:F0}", remainHp);
                break;

            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2} : {1:D2}", min, sec);

                if (remainTime < 20)
                {
                    timer += Time.deltaTime;
                    myText.color = Color.red;
                    if (timer > 0.8f)
                    {
                        myText.color = new Color(255, 0, 0, 0);
                        if (timer > 1.6f)
                        {
                            myText.color = new Color(255, 0, 0, 255);
                            timer = 0;
                        }
                    }
                }
                break;
        }
    }
}
