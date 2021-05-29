using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    public EnemyController enemyController;

    public RectTransform containerRectTransform;

    public GameObject target;
    public Camera gameplayCamera;
    public RectTransform canvasRectTransform;

    public RectTransform outer;
    public DOTweenAnimation rotate;
    public DOTweenAnimation scale;
    public Button button;

    public GameObject miss;
    public GameObject great;
    Sequence sequence;
    public void SetDuration(float duration)
    {
        sequence = DOTween.Sequence();
        sequence.Append(this.outer.DOScale(1.0f, duration).SetEase(Ease.Linear));
        sequence.Join(this.outer.DOLocalRotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360).SetEase(Ease.InOutSine));
        sequence.SetAutoKill(true);
        sequence.AppendInterval(0.016f * 10);
        sequence.Play();
        sequence.onComplete = () => OnFinish(false);
        //SetDurationAndPlay(duration, rotate);
    }

    public void SetDurationAndPlay(float duration, DOTweenAnimation animation)
    {
        animation.duration = duration;
        animation.CreateTween();
        animation.tween.Play();
    }

    private void LateUpdate()
    {
        if (enemyController)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(gameplayCamera, enemyController.transform.position);
            containerRectTransform.anchoredPosition = screenPoint - canvasRectTransform.sizeDelta / 2f;
        }
    }

    private async void OnFinish(bool hit)
    {
        Debug.Log("HIT" + hit);
        button.interactable = false;
        target.gameObject.SetActive(false);
        great.gameObject.SetActive(hit);
        miss.gameObject.SetActive(!hit);
        await UniTask.Delay(500);
        Destroy(this.gameObject);
    }

    public void OnTap()
    {
        button.interactable = false;
        OnFinish(this.outer.localScale.x <= 1.5f);
        Debug.Log("TAP " + this.outer.localScale.x);
        //sequence.Complete(false);
        sequence.Kill(false);
    }
}