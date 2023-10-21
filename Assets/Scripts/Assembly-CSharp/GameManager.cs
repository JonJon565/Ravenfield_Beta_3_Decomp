using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public const int MENU_LEVEL_INDEX = 1;

	public static GameManager instance;

	[NonSerialized]
	public bool ingame;

	public GameObject ingameUiPrefab;

	public GameObject playerPrefab;

	public bool reverseMode;

	public bool assaultMode;

	public int victoryPoints = 200;

	public AudioMixerGroup fpMixerGroup;

	private float gameStartTime;

	private void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.sceneLoaded += OnLevelLoaded;
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnLevelLoaded;
	}

	private void Start()
	{
	}

	private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelIndexLoaded(scene.buildIndex);
	}

	private void OnLevelIndexLoaded(int levelIndex)
	{
		if (IngameLevel(levelIndex))
		{
			StartGame();
		}
		else
		{
			ingame = false;
		}
	}

	private bool IngameLevel(int level)
	{
		return level > 1;
	}

	private void StartGame()
	{
		ingame = true;
		UnityEngine.Object.Instantiate(ingameUiPrefab);
		UnityEngine.Object.Instantiate(playerPrefab, new Vector3(0f, 1000f, 0f), Quaternion.identity);
		ActorManager.instance.StartGame();
		CoverManager.instance.StartGame();
		DecalManager.instance.StartGame();
		gameStartTime = Time.time;
	}

	public float ElapsedGameTime()
	{
		return Time.time - gameStartTime;
	}
}
