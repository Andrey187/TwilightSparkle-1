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
    private Dictionary<ActiveDoT, DoTTimers> doTTimers = new Dictionary<ActiveDoT, DoTTimers>();

    private struct DoTTimers
    {
        public float timer;
        public float interval;

        public DoTTimers(float timer, float interval)
        {
            this.timer = timer;
            this.interval = interval;
        }
    }

    // � ������ RegisterDoT ��������� ���������� � ������� � ��������� � �������
    public void RegisterDoT(IDoTEffect doTEffect, BaseEnemy target, int amount)
    {
        ActiveDoT activeDoT = new ActiveDoT(doTEffect, target, amount);
        activeDoTs.Add(activeDoT);

        // ���������, ���� �� ��� ����� ���� � �������
        if (!doTTimers.ContainsKey(activeDoT))
        {
            doTTimers.Add(activeDoT, new DoTTimers(activeDoT.Timer, activeDoT.Interval));
        }
    }

    // � ������ UnregisterDoT ������� ���������� �� �������
    public void UnregisterDoT(ActiveDoT activeDoT)
    {
        doTTimers.Remove(activeDoT);
        activeDoTs.Remove(activeDoT);
    }


    private void OnDestroy()
    {
        activeDoTs.Clear();
    }

    private void Update()
    {
        UnityEngine.Profiling.Profiler.BeginSample("updateDot");

        // ������� ������ ��� ���������, ������� ����� �������
        List<ActiveDoT> doTsToRemove = new List<ActiveDoT>();

        for (int i = 0; i < activeDoTs.Count; i++)
        {
            ActiveDoT activeDoT = activeDoTs[i];

            // �������� ���������� � �������� � ���������� �� �������
            DoTTimers timers;
            if (doTTimers.TryGetValue(activeDoT, out timers))
            {
                // ��������� ��������
                timers.interval -= Time.deltaTime;

                // ��������� ������ ��� ������� ActiveDoT
                timers.timer -= Time.deltaTime;

                // ���� �������� �����, ��������� ��������
                if (timers.interval <= 0)
                {
                    ApplyDoTDamage(activeDoT);
                    // ���������� ��������
                    timers.interval = activeDoT.Interval;
                }

                // ���� ������ �����, �������� ������� � ������ ��� ��������
                if (timers.timer <= 0)
                {
                    doTsToRemove.Add(activeDoT);
                }
                else
                {
                    // ��������� ���������� � �������
                    doTTimers[activeDoT] = timers;
                }
            }
            else
            {
                // ���� �� ������� �������� ���������� � ��������, ������� ������� �� ������
                doTsToRemove.Add(activeDoT);
            }
        }

        // ������� �������� ����� ���������� �����
        foreach (var doT in doTsToRemove)
        {
            // ������� ������� �� �������
            doTTimers.Remove(doT);
            // ������ ����� ������ ������ ������� �� ������ activeDoTs
            activeDoTs.Remove(doT);
        }

        UnityEngine.Profiling.Profiler.EndSample();
    }

    private void ApplyDoTDamage(ActiveDoT activeDoT)
    {
        if (activeDoT.Target != null)
        {
            int periodicDamageAmount = activeDoT.DoTEffect.ApplyDoT(activeDoT.Target.CurrentHealth, activeDoT.Amount);
            activeDoT.Target.CurrentHealth -= periodicDamageAmount;

            DamageDoTNumberPool.Instance.InitializeGetObjectFromPool(periodicDamageAmount, activeDoT.Target.transform, activeDoT.DoTEffect);

            if (activeDoT.Target.CurrentHealth <= 0)
            {
                UnregisterDoT(activeDoT);
                activeDoT.Target.Die();
            }
        }
    }

    public void UnregisterDoTsForTarget(BaseEnemy target)
    {
        activeDoTs.RemoveAll(activeDoT => activeDoT.Target == target);
    }
}
