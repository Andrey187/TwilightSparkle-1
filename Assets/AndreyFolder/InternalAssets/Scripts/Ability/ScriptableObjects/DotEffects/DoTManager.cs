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

    // В методе RegisterDoT добавляем информацию о таймере и интервале в словарь
    public void RegisterDoT(IDoTEffect doTEffect, BaseEnemy target, int amount)
    {
        ActiveDoT activeDoT = new ActiveDoT(doTEffect, target, amount);
        activeDoTs.Add(activeDoT);

        // Проверяем, есть ли уже такой ключ в словаре
        if (!doTTimers.ContainsKey(activeDoT))
        {
            doTTimers.Add(activeDoT, new DoTTimers(activeDoT.Timer, activeDoT.Interval));
        }
    }

    // В методе UnregisterDoT удаляем информацию из словаря
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

        // Создаем список для элементов, которые нужно удалить
        List<ActiveDoT> doTsToRemove = new List<ActiveDoT>();

        for (int i = 0; i < activeDoTs.Count; i++)
        {
            ActiveDoT activeDoT = activeDoTs[i];

            // Получаем информацию о таймерах и интервалах из словаря
            DoTTimers timers;
            if (doTTimers.TryGetValue(activeDoT, out timers))
            {
                // Обновляем интервал
                timers.interval -= Time.deltaTime;

                // Обновляем таймер для данного ActiveDoT
                timers.timer -= Time.deltaTime;

                // Если интервал истек, выполняем действие
                if (timers.interval <= 0)
                {
                    ApplyDoTDamage(activeDoT);
                    // Сбрасываем интервал
                    timers.interval = activeDoT.Interval;
                }

                // Если таймер истек, помещаем элемент в список для удаления
                if (timers.timer <= 0)
                {
                    doTsToRemove.Add(activeDoT);
                }
                else
                {
                    // Обновляем информацию в словаре
                    doTTimers[activeDoT] = timers;
                }
            }
            else
            {
                // Если не удалось получить информацию о таймерах, удаляем элемент из списка
                doTsToRemove.Add(activeDoT);
            }
        }

        // Удаляем элементы после завершения цикла
        foreach (var doT in doTsToRemove)
        {
            // Удаляем элемент из словаря
            doTTimers.Remove(doT);
            // Теперь можно убрать данный элемент из списка activeDoTs
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
