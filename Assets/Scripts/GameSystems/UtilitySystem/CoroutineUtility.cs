using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem
{
    public class CoroutineUtility : MonoBehaviour, IUtilityReferenceHandler
    {
        public IEnumerator WaitForAll(IEnumerable<IEnumerator> coroutines)
        {
            var done = new List<bool>();
            int count = 0;

            foreach (var coroutine in coroutines)
            {
                int idx = count++;
                done.Add(false);
                StartCoroutine(RunAndMark(coroutine, () => done[idx] = true));
            }

            while (done.Exists(flag => !flag))
                yield return null;
        }

        private IEnumerator RunAndMark(IEnumerator coroutine, Action onDone)
        {
            yield return coroutine;
            onDone?.Invoke();
        }
    }
}