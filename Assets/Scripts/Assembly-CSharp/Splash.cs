using UnityEngine;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
	public MeshRenderer presentsTextRenderer;

	public Image overlay;

	public AudioSource helicopterSound;

	public AnimationCurve helicopterVolume;

	private Action fadeInAction = new Action(2f);

	private Action presentsTitleAction = new Action(4f);

	private Action endAction = new Action(20f);

	private void Start()
	{
		fadeInAction.Start();
		presentsTitleAction.Start();
		float duration = Object.FindObjectOfType<GotoMenu>().duration;
		endAction.StartLifetime(duration);
		Invoke("PlayFlyby", 4.1f);
	}

	private void PlayFlyby()
	{
		GetComponent<AudioSource>().Play();
	}

	private void Update()
	{
		Color color = new Color(1f, 1f, 1f, 1f - Mathf.Clamp01(3f - 3f * presentsTitleAction.Ratio()));
		presentsTextRenderer.material.color = color;
		UpdateOverlay();
		helicopterSound.volume = helicopterVolume.Evaluate(endAction.Ratio());
	}

	private void UpdateOverlay()
	{
		Color black = Color.black;
		black.a = Mathf.Clamp01(2f - 2f * fadeInAction.Ratio()) + (1f - Mathf.Clamp01(endAction.Remaining() - 1f));
		overlay.color = black;
	}
}
