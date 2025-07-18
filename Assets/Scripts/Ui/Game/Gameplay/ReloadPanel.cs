using System;
using System.Collections;
using MPUIKIT;
using UnityEngine;

namespace Assets.Scripts.Ui.Game.Gameplay
{
    public class ReloadPanel : MonoBehaviour
    {
        [SerializeField] private MPImage _fill;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void StartReload(float duration, Action callback) =>
            StartCoroutine(Reloader(duration, callback));

        private IEnumerator Reloader(float duration, Action callback)
        {
            float progress;
            float passedTime = 0;

            _fill.fillAmount = 0;
            _canvasGroup.alpha = 1;

            while (_fill.fillAmount != 1)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                _fill.fillAmount = Mathf.Lerp(0, 1, progress);

                yield return null;
            }

            _canvasGroup.alpha = 0;
            callback?.Invoke();
        }
    }
}
