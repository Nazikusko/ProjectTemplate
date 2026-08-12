using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FillPanel : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Transform _circleTransform;
    [SerializeField] private RectTransform _sliceRectTransform;

    public void Init()
    {
        _fillImage.color = Color.black;
        _fillImage.gameObject.SetActive(false);
        _circleTransform.gameObject.SetActive(false);
        _sliceRectTransform.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }

    public void ShowSlice(float duration, Action onComplete = null)
    {
        _sliceRectTransform.gameObject.SetActive(true);
        DOTween.Kill(this);
        var sliceWidth = _sliceRectTransform.rect.width;
        _sliceRectTransform.anchoredPosition = new Vector2(-sliceWidth, _sliceRectTransform.anchoredPosition.y);
        _sliceRectTransform.DOAnchorPosX(0, duration).SetId(this).SetEase(Ease.OutSine).OnComplete(() =>
        {
            _sliceRectTransform.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    public void HideSlice(float duration, Action onComplete = null)
    {
        _sliceRectTransform.gameObject.SetActive(true);
        DOTween.Kill(this);
        var sliceWidth = _sliceRectTransform.rect.width;
        _sliceRectTransform.anchoredPosition = new Vector2(0, _sliceRectTransform.anchoredPosition.y);
        _sliceRectTransform.DOAnchorPosX(sliceWidth, duration).SetId(this).SetEase(Ease.InSine).OnComplete(() =>
        {
            _sliceRectTransform.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    public void ShowCircle(float duration = -1, Action onComplete = null)
    {
        DOTween.Kill(this);

        _circleTransform.gameObject.SetActive(true);
        _fillImage.gameObject.SetActive(true);

        if (duration > 0)
        {
            _fillImage.color = Color.clear;
            _fillImage.DOColor(Color.black, duration * 0.5f).SetId(this);

            _circleTransform.localScale = Vector3.one;
            _circleTransform.DOScale(30f, duration).SetEase(Ease.InSine).SetId(this)
                .OnComplete(() => onComplete?.Invoke());
        }
        else
        {
            _circleTransform.localScale = Vector3.one * 30f;
            _fillImage.color = Color.black;
        }
    }

    public void HideCircle(float duration = -1, Action onComplete = null)
    {
        DOTween.Kill(this);
        _fillImage.color = Color.black;

        if (duration > 0)
        {
            _fillImage.gameObject.SetActive(true);
            _circleTransform.gameObject.SetActive(true);
            _circleTransform.localScale = Vector3.one * 30f;

            _fillImage.DOColor(Color.clear, duration * 0.5f).SetDelay(duration * 0.2f).SetId(this);

            _circleTransform.DOScale(1f, duration).SetEase(Ease.InSine).SetId(this).OnComplete(() =>
            {
                _circleTransform.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
        else
        {
            _circleTransform.gameObject.SetActive(false);
            _fillImage.gameObject.SetActive(false);
        }
    }

    public void ShowFill(float duration = -1, Action onComplete = null)
    {
        DOTween.Kill(this);

        if (duration > 0)
        {
            _fillImage.gameObject.SetActive(true);
            _fillImage.color = Color.clear;
            _fillImage.DOColor(Color.black, duration).SetId(this).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
        else
        {
            _fillImage.gameObject.SetActive(true);
            _fillImage.color = Color.black;
        }
    }

    public void HideFill(float duration = -1, Action onComplete = null)
    {
        DOTween.Kill(this);

        if (duration > 0)
        {
            _fillImage.color = Color.black;
            _fillImage.gameObject.SetActive(true);
            _fillImage.DOColor(Color.clear, duration).SetId(this).OnComplete(() =>
            {
                _fillImage.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
        else
        {
            _fillImage.color = Color.clear;
            _fillImage.gameObject.SetActive(false);
        }
    }
    
    public void Hide()
    {
        _fillImage.color = Color.clear;
        _fillImage.gameObject.SetActive(false);
    }

    public void Show()
    {
        _fillImage.color = Color.black;
        _fillImage.gameObject.SetActive(true);
    }
}