//#define ADS

using UnityEngine;
using System;

#if ADS
using GoogleMobileAds.Api;
#endif


public class AdsManager : SingletonPersistanceAuto<AdsManager> {

    public Action OnCloseVideoReward;
    public Action OnVideoReward;
    public Action OnFailLoadVideo;
    public Action OnLoadVideo;

    #if ADS
 
    BannerView bannerView;
    InterstitialAd interstitial;
    RewardBasedVideoAd rewardBasedVideo;

    #endif

    public override void Awake() {
        base.Awake();

        Init();
    }

    void Init(){

#if ADS && UNITY_ANDROID

        MobileAds.Initialize(status=>{
            Debug.Log("MobilesAds.Initialize successful");
        });

        rewardBasedVideo = RewardBasedVideoAd.Instance;
        
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoLoadedFailed;
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
#endif



    }

#if ADS

    void HandleRewardBasedVideoClosed(object sender,EventArgs args){
        Debug.Log("HandleRewardBasedVideoClosed event received with message: " + args);
        if(OnCloseVideoReward!=null)
            OnCloseVideoReward();
    }

    void HandleRewardBasedVideoLoadedFailed(object sender, AdFailedToLoadEventArgs args){
        Debug.Log("HandleRewardBasedVideoLoadedFailed event received with message: " + args.Message);
        if(OnFailLoadVideo!=null)
            OnFailLoadVideo();
    }
    void HandleRewardBasedVideoLoaded(object sender, EventArgs args){
        Debug.Log("HandleRewardBasedVideoLoaded event received");
        if(OnFailLoadVideo!=null)
            OnFailLoadVideo();
    }

    void HandleRewardBasedVideoRewarded(object sender, Reward args){
        string type = args.Type;
        double amount = args.Amount;
        
        Debug.Log("HandleRewardBasedVideoRewarded event received amount=" + amount.ToString() + " type=" + type);

        if(OnVideoReward!=null)
            OnVideoReward();
    }

#endif

    public void HideBanner(){
        #if ADS
        if(bannerView!=null)
            bannerView.Hide();
        #endif
    }

    public void ShowBanner(){
        #if ADS
        if(bannerView!=null)
            bannerView.Show();
        #endif
    }

    public void RequestBanner(){
        
        #if ADS

        if(bannerView!=null){
            bannerView.Destroy();
            bannerView = null;
        }
        Debug.Log("RequestBanner");


        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-5345312338198409/8007152188";
        #elif UNITY_IPHONE
            string adUnitId = "unexpected_platform";
        #else
            string adUnitId = "unexpected_platform";
        #endif
    
        //TEST
        if(Debug.isDebugBuild)
            adUnitId = "ca-app-pub-3940256099942544/6300978111";
        
        //var bannerHeight = (int)(Screen.height*0.15f);
        //Debug.Log("banner height="+bannerHeight);
        //AdSize adSize = new AdSize(AdSize.FullWidth,bannerHeight);
        
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
        
        AdRequest request = new AdRequest.Builder().Build();      
        request.TestDevices.Add("B08AD6A6F8749E0266FA68AB654B0C6C");
        bannerView.LoadAd(request);
        #endif
        
    }

    public bool IsVideoLoaded(){
        #if ADS
        return rewardBasedVideo!=null && rewardBasedVideo.IsLoaded();
        #else
        return false;
        #endif
    }

    public bool IsInterstitialLoaded(){
        #if ADS
        return interstitial!=null && interstitial.IsLoaded();
        #else
        return false;
        #endif
    } 

   

    public void RequestRewardedVideo(){
        
        #if ADS

        Debug.Log("RequestRewardedVideo");

        
      
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-5345312338198409/1250172145";
#elif UNITY_IPHONE
            string adUnitId = "unexpected_platform";
#else
            string adUnitId = "unexpected_platform";
#endif

        //TEST
        if (Debug.isDebugBuild)
            adUnitId = "ca-app-pub-3940256099942544/5224354917";

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        request.TestDevices.Add("B08AD6A6F8749E0266FA68AB654B0C6C");
        // Load the rewarded video ad with the request.
        rewardBasedVideo.LoadAd(request, adUnitId);
        if(Application.isEditor && OnLoadVideo!=null){
            OnLoadVideo();
        }

        #endif

    }

    public void RequestInterstitial(){

        #if ADS

        Debug.Log("RequestInterstitial");

        if(interstitial!=null){
            interstitial.Destroy();
            interstitial=null;
        }
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-5345312338198409/7815580498";
        #elif UNITY_IPHONE
            string adUnitId = "unexpected_platform";
        #else
            string adUnitId = "unexpected_platform";
        #endif
        //TEST
        if(Debug.isDebugBuild)
            adUnitId = "ca-app-pub-3940256099942544/1033173712";
        
        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        interstitial.OnAdClosed+=HandleInterstitialClose;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        request.TestDevices.Add("B08AD6A6F8749E0266FA68AB654B0C6C");
        // Load the interstitial with the request.
        interstitial.LoadAd(request);

        #endif

    }

    public void HandleInterstitialClose(object sender, EventArgs args){
        Debug.Log("HandleInterstitialClose event received");

    }
	

	public void ShowInterstitial(){
        #if ADS
        if(interstitial==null){
            Debug.Log("ShowInterstitial failed null not initialized");

            return;
        }
        if(!interstitial.IsLoaded()){
            Debug.Log("ShowInterstitial failed not loaded");

            return;
        }      


        interstitial.Show();

        #endif

    }
 



    public void ShowRewardVideo(){
        
        #if ADS

        if(!rewardBasedVideo.IsLoaded()){
            Debug.Log("ShowRewardVideo failed not loaded");
            if(OnCloseVideoReward!=null) OnCloseVideoReward();
            return;
        }
            
        rewardBasedVideo.Show();
        if(Application.isEditor && OnVideoReward!=null){
            OnVideoReward();
        }
        
        #endif

    }

   
}
