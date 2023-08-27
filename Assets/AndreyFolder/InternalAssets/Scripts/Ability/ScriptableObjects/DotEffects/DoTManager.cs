using DamageNumber;
using System.Collections.Generic;
using UnityEngine;

public class DoTManager : MonoBehaviour
{
    private static DoTManager instance;
    public static DoTManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DoTManager>();
            return instance;
        }
    }

    private List<ActiveDoT> activeDoTs = new List<ActiveDoT>();

    public void RegisterDoT(IDoTEffect doTEffect, BaseEnemy target, int amount)
    {
        ActiveDoT activeDoT = new ActiveDoT(doTEffect, target, amount);
        activeDoTs.Add(activeDoT);
    }

    public void UnregisterDoT(ActiveDoT activeDoT)
    {
        activeDoTs.Remove(activeDoT);
    }

    private void OnDestroy()
    {
        activeDoTs.Clear();
    }

    private void Update()
    {
        UnityEngine.Profiling.Profiler.BeginSample("updateDot");

        // список для хранения индексов дот-эффектов, которые нужно удалить
        List<int> indexesToRemove = new List<int>();

        for (int i = activeDoTs.Count - 1; i >= 0; i--)
        {
            ActiveDoT activeDoT = activeDoTs[i];
            if (activeDoT.UpdateTimer(Time.deltaTime))
            {
                ApplyDoTDamage(activeDoT);
                indexesToRemove.Add(i);
            }
        }

        // Удаление помеченных элементов в обратном порядке
        for (int i = indexesToRemove.Count - 1; i >= 0; i--)
        {
            int indexToRemove = indexesToRemove[i];
            activeDoTs.RemoveAt(indexToRemove);
        }

        UnityEngine.Profiling.Profiler.EndSample();
    }

    private void ApplyDoTDamage(ActiveDoT activeDoT)
    {
        if (activeDoT.Target != null)
        {
            int periodicDamageAmount = activeDoT.DoTEffect.ApplyDoT(activeDoT.Target.CurrentHealth, activeDoT.Amount);
            activeDoT.Target.CurrentHealth -= periodicDamageAmount;

            if (activeDoT.Target.CurrentHealth <= 0)
            {
                UnregisterDoT(activeDoT);
                activeDoT.Target.Die();
            }

            DamageDoTNumberPool.Instance.Initialize(periodicDamageAmount, activeDoT.Target.transform, activeDoT.DoTEffect);
        }
    }

    public void UnregisterDoTsForTarget(BaseEnemy target)
    {
        activeDoTs.RemoveAll(activeDoT => activeDoT.Target == target);
    }
}
