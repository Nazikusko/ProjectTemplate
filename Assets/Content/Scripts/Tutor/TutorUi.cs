using DG.Tweening;
using UnityEngine;
using Zenject;

public class TutorUi : MonoBehaviour
{
    [field: SerializeField] public TutorHand TutorHand { get; private set; }

    [Inject] private SceneObjectsHolder _sceneObjectsHolder;
    [Inject] private CameraManager _cameraManager;

    public void PointedHandWorldObject(Vector3 position)
    {
        ShowHand();
        var screenPoint = _sceneObjectsHolder.Camera.WorldToScreenPoint(position);
        TutorHand.GetComponentInChildren<Animator>().SetTrigger("PointedTrigger");

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _sceneObjectsHolder.UiCanvas.GetComponent<RectTransform>(),
            screenPoint,
            _cameraManager.UiCamera,
            out var canvasPosition);

        var handRectTransform = TutorHand.GetComponent<RectTransform>();
        handRectTransform.anchoredPosition = canvasPosition;
    }

    public void PointedHandCanvasObject(Transform targetTransform)
    {
        ShowHand();
        TutorHand.GetComponentInChildren<Animator>().SetTrigger("PointedTrigger");
        TutorHand.transform.position = targetTransform.position;
    }

    public void MovePointerBetween2Positions(Vector3 startPosition, Vector3 endPosition, float duration = 1f)
    {
        ShowHand();
        var animator = TutorHand.GetComponentInChildren<Animator>();
        animator.SetTrigger("UpStopTrigger");
        TutorHand.transform.position = startPosition;

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(0.3f);
        sequence.AppendCallback(() =>
        {
            animator.SetTrigger("DownStopTrigger");
        });
        sequence.AppendInterval(0.2f);
        sequence.Append(TutorHand.transform.DOMove(endPosition, duration).SetEase(Ease.InOutSine));
        sequence.AppendCallback(() =>
        {
            animator.SetTrigger("UpStopTrigger");
        });
        sequence.AppendInterval(0.5f);

        sequence.SetLoops(-1).SetId(this);
    }

    public void HideHand()
    {
        TutorHand.gameObject.SetActive(false);
    }

    public void ShowHand()
    {
        TutorHand.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
