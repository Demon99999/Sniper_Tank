using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Services.CoroutineRunnerServices
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
    }
}
