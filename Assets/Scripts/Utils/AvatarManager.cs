using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarManager : SingletonPersistance<AvatarManager>{
    

    public GameObject panel;
    public Image avatar;
    public Button close;
    public Sprite defaultAvatar;
    
    
    public GridLayoutGroup gridLayoutGroup;
    public List<Sprite> avatars;
    List<Image> avatarsButtons=new List<Image>();
    int selected;

    
    Button urlAvatarButton;
    Color colorNormal = new Color(0,0,0,0.5f);
    Color colorSelected = new Color(0,0.5f,0,0.5f);

    public OnClosePanel onClosePanel;
    public delegate void OnClosePanel();

    void Start(){
        avatar.gameObject.SetActive(false);
        close.onClick.AddListener(OnClickClose);
        for(int i=0;i<avatars.Count;i++){
            GenerateAvatarButton( avatars[i]);
        }

    }

    void UpdateUI(){        
        var url = Prefs.GetAvatarUrl;
        if(!string.IsNullOrEmpty(url) && urlAvatarButton==null)
            ImageDownloader.GetPic(url,sprite=>{
                urlAvatarButton = GenerateAvatarButton(sprite);
                
                urlAvatarButton.image.color = colorNormal;
            });
    }
    public void Show(){
        canvas.enabled=true;
        UpdateUI();
        FadePanel.FadeIn(panel);
        SelectAvatar(Prefs.GetCustomAvatar);
    }


    public void GenerateAvatarUrl(){
        var url = Prefs.GetAvatarUrl;
        if(!string.IsNullOrEmpty(url))
            ImageDownloader.GetPic(url,sprite=>{
                DeleteAvatarButton();
                urlAvatarButton = GenerateAvatarButton(sprite);
            });
    }

    public void DeleteAvatarButton(){
        if(urlAvatarButton!=null){
            Destroy(urlAvatarButton.gameObject);
            int removed = avatarsButtons.RemoveAll(g=>g==null || g.gameObject.IsDestroyed());
        }
    }

    Button GenerateAvatarButton(Sprite sprite){
        var index = avatarsButtons.Count;
        var newAvatar = GameObject.Instantiate(avatar,avatar.transform.parent.position,Quaternion.identity,avatar.transform.parent);
        newAvatar.gameObject.SetActive(true);
        newAvatar.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        
        var button = newAvatar.GetComponent<Button>();
        button.onClick.AddListener(()=>OnClickAvatar(index));
        avatarsButtons.Add(newAvatar);
        return button;
    }

    void OnClickAvatar(int index){
        SoundManager.PlayClick();
        avatarsButtons[index].GetComponent<Button>().Focus();
        SelectAvatar(index);

        if(selected==avatars.Count)
            selected=-1;
        
        AnalyticsManager.Raise("click_avatar","avatar",selected);

        Prefs.SetCustomAvatar(selected);
        //PlayFabPrefs.UploadMyCustomAvatar();
       // PlayerPerfil.UpdatePlayerPerfil();
       
    }

    void SelectAvatar(int index){
        if(index ==-1)   
            index = avatarsButtons.Count-1;

        index = Mathf.Clamp(index,0,avatarsButtons.Count);
        
            
        selected = index;
        avatarsButtons.ForEach(a=>{
            
            a.color = colorNormal;
        });

        
        avatarsButtons[index].color = colorSelected;
        
    }

    void OnClickClose(){
        SoundManager.PlayClick();
        close.Focus();
        FadePanel.FadeOut(panel);

        this.ExecuteIn(1,()=>{
            canvas.enabled=false;
        });

        if (onClosePanel != null)
        {
            onClosePanel();
        }
    }


    public Sprite GetAvatarSprite()
    {
        if (Prefs.GetCustomAvatar==-1)
        {
            return defaultAvatar;
        }
        return instance.avatars[Prefs.GetCustomAvatar];
    }
    
}
