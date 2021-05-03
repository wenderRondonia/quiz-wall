using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RankingScreen : Singleton<RankingScreen>
{
	[SerializeField] GameObject panel;
	[SerializeField] Transform parentPlayers;



	[SerializeField] Button close;

	List<RankPlayerUI> rankPlayers;


	void Start()
    {
		close.onClick.AddListener(OnClickClose);
		parentPlayers.gameObject.SetActive(false);


		StartCoroutine(RetrievingData());
	}


	void SetupPlayers()
    {
		parentPlayers.gameObject.SetActive(true);
		rankPlayers = parentPlayers.GetComponentsInChildren<RankPlayerUI>().ToList();

        foreach (var rankPlayer in rankPlayers)
        {
			int playerIndex = rankPlayers.IndexOf(rankPlayer);
			rankPlayer.SetupRank(playerIndex);

		}
	}

	void OnClickClose()
	{
		SoundManager.PlayClick();
		close.Focus();

		Hide();
	}


	public void Show()
	{
		FadePanel.FadeIn(panel);
	}

	public void Hide()
	{
		FadePanel.FadeOut(panel);
	}


	IEnumerator RetrievingData()
    {

		//LoadSpinManager.instance.Show();

		yield return new WaitForSeconds(3);

		SetupPlayers();

		//LoadSpinManager.instance.Hide();

	}
}
