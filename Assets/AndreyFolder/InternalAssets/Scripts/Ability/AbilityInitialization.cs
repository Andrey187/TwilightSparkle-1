public class AbilityInitialization : MonoCache
{
    protected AbilityRunner _abilityRunner;

    protected FireBallAbility FireBallAbility;
    protected FrostBallAbility FrostBallAbility;
    public FireDoTEffect FireDoTEffect;

    private void Awake()
    {
        _abilityRunner = GetComponent<AbilityRunner>();
        FireBallAbility = new FireBallAbility();
        FrostBallAbility = new FrostBallAbility();
        FireDoTEffect = new FireDoTEffect();
    }
}
