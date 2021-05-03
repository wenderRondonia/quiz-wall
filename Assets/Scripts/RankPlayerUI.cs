using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RankPlayerUI : MonoBehaviour
{
    public Image avatarPic;
    public Text bankAmount;
    public Text nickname;
    public Image rankNumber;
    public Text rankNumberText;
    [SerializeField] Sprite goldSprite;
    [SerializeField] Sprite silverSprite;
    [SerializeField] Sprite bronzeSprite;

    public void SetupRank(int index)
    {
        
        rankNumberText.text = "";
        rankNumber.gameObject.SetActive(true);

        switch (index)
        {
            case 0: rankNumber.sprite = goldSprite;  break;
            case 1: rankNumber.sprite = silverSprite; break;
            case 2: rankNumber.sprite = bronzeSprite; break;
            default:
                rankNumber.gameObject.SetActive(false);
                rankNumberText.text = (index + 1).ToString(); 
            break;
        }
    }
}
