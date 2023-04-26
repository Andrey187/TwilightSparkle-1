using UnityEngine;

[RequireComponent(typeof(AbilityRunner))]
public class AbilityChanger : AbilityInitialization
{
    [SerializeField] private KeyCode _fireBallButton;
    [SerializeField] private KeyCode _frostBallButton;

    private void Start()
    {
        _abilityRunner.CurrentAbility = FireBallAbility;
        _abilityRunner.DoTEffect = FireDoTEffect;

        Debug.Log("FireBall button  " + _fireBallButton);
        Debug.Log("FrostBall button  " + _frostBallButton);
    }

    protected override void Run()
    {
        IAbility currentAbility = _abilityRunner.CurrentAbility;

        if (Input.GetKeyDown(_fireBallButton) && !(currentAbility is FireBallAbility))
        {
            _abilityRunner.CurrentAbility = FireBallAbility;
            _abilityRunner.DoTEffect = FireDoTEffect;
        }
        if (Input.GetKeyDown(_frostBallButton) && !(currentAbility is FrostBallAbility))
        {
            _abilityRunner.CurrentAbility = new FrostBallAbility();
        }
    }
}
