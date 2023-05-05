using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelUpSystem _levelUpSystem;
    [SerializeField] private TalentSystem _talentSystem;
    private TalentPointsBinding talentPointsBinding;

    private void Start()
    {
    }
}
