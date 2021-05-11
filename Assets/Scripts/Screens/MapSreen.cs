using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MapSreen : MonoBehaviour
{
    [SerializeField] Button back;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Sprite spriteLocked;
    [SerializeField] Sprite[] spriteUnlocked;


    public Transform LevelButtonsParent;

    List<Button> LevelButtons;

    public static int LevelIndexSelected = -1;
    void Start()
    {
        back.onClick.AddListener(OnClickBack);
        SoundManager.PlayMusicMap();

        LevelButtons = LevelButtonsParent.GetComponentsInChildren<Button>().ToList();

        foreach (var LevelButton in LevelButtons)
        {
            int index = LevelButtons.IndexOf(LevelButton);
            LevelButton.onClick.AddListener(()=>OnButtonLevel(index));
            LevelButton.image.sprite = IsLevelUnLocked (index) ? spriteUnlocked[Prefs.GetLevelCompleted(index)] : spriteLocked;
        }

        scrollRect.verticalNormalizedPosition = 0;

        LevelIndexSelected = -1;
    }

    bool IsLevelUnLocked(int index)
    {
        if (index==0)
        {
            return true;
        }

        return Prefs.GetLevelCompleted(index-1) >= 3;
    }

    void OnButtonLevel(int index)
    {
        var sprite = LevelButtons[index].image.sprite;
        if (sprite == spriteUnlocked[0] || sprite==spriteUnlocked[3] || sprite==spriteUnlocked[4])
        {
            return;
        }

        LevelIndexSelected = index;
        SoundManager.PlayClick();
        LoadScreenManager.instance.LoadSceneScreen("Game");
        SoundManager.StopMusicMap();
    }

    void OnClickBack(){

        SoundManager.PlayClick();
        back.Focus();
        
        LoadScreenManager.instance.LoadSceneScreen("Menu");
        SoundManager.StopMusicMap();

    }

}
