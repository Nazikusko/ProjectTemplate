using DG.Tweening;
using UnityEngine;
using Zenject;

public class TutorHand : MonoBehaviour
{
    [Inject] private SceneObjectsHolder _sceneObjectsHolder;
    [Inject] private CameraManager _cameraManager;

    public void PointedHandWorldObject(Vector3 position)
    {
        ShowHand();
        var screenPoint = _sceneObjectsHolder.Camera.WorldToScreenPoint(position);
        GetComponentInChildren<Animator>().SetTrigger("PointedTrigger");

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _sceneObjectsHolder.UiCanvas.GetComponent<RectTransform>(),
            screenPoint,
            _cameraManager.UiCamera,
            out var canvasPosition);

        var handRectTransform = GetComponent<RectTransform>();
        handRectTransform.anchoredPosition = canvasPosition;
    }

    public void PointedHandCanvasObject(Transform targetTransform)
    {
        ShowHand();
        GetComponentInChildren<Animator>().SetTrigger("PointedTrigger");
        transform.position = targetTransform.position;
    }

    public void MovePointerBetween2Positions(Vector3 startPosition, Vector3 endPosition, float duration = 1f)
    {
        ShowHand();
        var animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("UpStopTrigger");
        transform.position = startPosition;

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(0.3f);
        sequence.AppendCallback(() =>
        {
            animator.SetTrigger("DownStopTrigger");
        });
        sequence.AppendInterval(0.2f);
        sequence.Append(transform.DOMove(endPosition, duration).SetEase(Ease.InOutSine));
        sequence.AppendCallback(() =>
        {
            animator.SetTrigger("UpStopTrigger");
        });
        sequence.AppendInterval(0.5f);

        sequence.SetLoops(-1).SetId(this);
    }

    public void HideHand()
    {
        gameObject.SetActive(false);
    }

    public void ShowHand()
    {
        gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
