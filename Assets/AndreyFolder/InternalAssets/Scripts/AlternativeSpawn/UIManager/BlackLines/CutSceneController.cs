using System.Collections;
using UnityEngine;
using Zenject;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] private Animator _camAnim;
    [SerializeField] private float _timeToActivateCutscene = 5f;
    [SerializeField] private float _timeUntilShutdown = 7f;
    [SerializeField] private Canvas _canvasBlackLines;
    [SerializeField] private GameObject _portal;
    [SerializeField] private GameObject _boss;
    [Inject] private ICounters _countTimer;
    [Inject] private IGamePause _gamePause;
    [Inject] private IWaveSpawner _waveSpawn;
    private bool isCutsceneOn;

    private void Start()
    {
        isCutsceneOn = true;
    }

    private void Update()
    {
        if (_countTimer.Timer >= _timeToActivateCutscene)
        {
            if (isCutsceneOn)
            {
                StartCoroutine(DelayedBossActivation());
            }
        }
    }

    private IEnumerator DelayedBossActivation()
    {
        ActiveCutscene(true);
        WaveDeactivated();
        yield return new WaitForSecondsRealtime(3f);
        BossActivated();

        yield return new WaitForSecondsRealtime(_timeUntilShutdown);
       
        ActiveCutscene(false);
        StopCoroutine(DelayedBossActivation());
    }


    private void ActiveCutscene(bool setActive)
    {
        isCutsceneOn = false;
        _gamePause.SetPause(setActive);
        _gamePause.SetActiveLines(setActive);
        PortalActivated(setActive);
        _camAnim.SetBool("Cutscene1", setActive);
        _canvasBlackLines.gameObject.SetActive(setActive);
    }

    private void PortalActivated(bool setActive)
    {
        _portal.gameObject.SetActive(setActive);
    }

    private void BossActivated()
    {
        _waveSpawn.BossSpawn(_waveSpawn.Waves[1]);
    }

    private void WaveDeactivated()
    {
        for (int i = 2; i < _waveSpawn.Waves.Length; i++)
        {
            _waveSpawn.CancellationTokenSource.Cancel();
        }
    }
}
