using UnityEngine;
using DG.Tweening;
using TMPro;

public class CardElements : MonoBehaviour
{
	public Transform[] buttons;
	public float duration = 1f;
	public float delayBetween = 0.1f;

	private Vector3[] targetPositions;

	public RectTransform nameText;
	public CanvasGroup nameCanvasGroup;
	public float slideDistance = 300f;
	private Vector2 originalNamePosition;
	public TMP_Text jobText;
	private string jobTitleString = "XR Developer";
	private float typingDuration = 1f;

	private void Awake()
	{
		targetPositions = new Vector3[buttons.Length];
		for (int i = 0; i < buttons.Length; i++)
		{
			targetPositions[i] = buttons[i].localPosition;
		}

		originalNamePosition = nameText.anchoredPosition;
		ResetName();
	}

	public void ShowAll()
	{
		ShowButtons();

		float delay = 0.5f + (buttons.Length - 1) * delayBetween;
		DOVirtual.DelayedCall(delay, AnimateNameIn);
	}

	private void ShowButtons()
	{

		HideButtons();
		ResetName();

		for (int i = 0; i < buttons.Length; i++)
		{
			int index = i;
			float delay = i * delayBetween;
			var button = buttons[i];
			button.DOKill();
			button.DOScale(1f, duration).SetDelay(delay).SetEase(Ease.OutBack);
			button.DOLocalMove(targetPositions[index], duration).SetDelay(delay).SetEase(Ease.OutCubic);

			CanvasGroup cg = button.GetComponent<CanvasGroup>();
			if (cg != null)
			{
				cg.DOKill();
				cg.DOFade(1f, duration).SetDelay(delay);
			}
		}
	}
	private void AnimateNameIn()
	{
		Sequence s = DOTween.Sequence();

		s.Append(nameText.DOAnchorPos(originalNamePosition, duration).SetEase(Ease.OutCubic));
		s.Join(nameCanvasGroup.DOFade(1f, duration));
		s.AppendInterval(0.2f);
		s.AppendCallback(AnimateJobTitle);
	}
	private void AnimateJobTitle()
	{
		jobText.DOKill();
		jobText.text = "";
		jobText.DOText(jobTitleString, typingDuration).SetEase(Ease.Linear);
	}


	public void HideAll()
	{
		HideButtons();
		ResetName();
		jobText.text = "";

	}
	private void HideButtons()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			var button = buttons[i];
			button.DOKill();
			button.localPosition = Vector3.zero;

			CanvasGroup cg = button.GetComponent<CanvasGroup>();
			if (cg != null) cg.alpha = 0;

			button.localScale = Vector3.zero;
		}
	}
	private void ResetName()
	{
		nameText.anchoredPosition = originalNamePosition + Vector2.left * slideDistance;
		nameCanvasGroup.alpha = 0f;
	}

}
