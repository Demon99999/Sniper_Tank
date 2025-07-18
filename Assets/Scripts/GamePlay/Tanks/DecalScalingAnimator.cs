using System.Collections;
using Assets.Scripts.MenuScene;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Tanks
{
    public class DecalScalingAnimator : TankAnimator
    {
        private const float StartScale = 0.05f;

        protected override IEnumerator Animator()
        {
            float progress = 0;
            float passedTime = 0;

            while (progress < 1)
            {
                passedTime += Time.deltaTime;
                progress = passedTime / AnimationsConfig.DecalScalingDuration;

                transform.localScale = Vector3.one * StartScale * AnimationsConfig.DecalScalingAnimationCurve.Evaluate(progress);

                yield return null;
            }
        }
    }
}
