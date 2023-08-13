using System.ComponentModel;
using UnityEngine;

public class PickUpFirstAidChallenge : BaseChallenge
{
    [SerializeField] private FirstAidDrop _firstAid;
    [SerializeField] private DropManager _dropManager;
    [SerializeField] private int _increaseChance;
    private int _pickUPCount;
    protected internal override event PropertyChangedEventHandler PropertyChanged;

    protected internal override int Reward { get => _increaseChance; set => _increaseChance = value; }
    protected internal override int CurrentCountValue { get => _pickUPCount; set => _pickUPCount = value; }
    protected internal override int MaxCountValue { get => _maxCountValue; set => _maxCountValue = value; }

    protected override void Awake()
    {
        base.Awake();
        _dropManager = FindObjectOfType<DropManager>();
        DropEventManager.Instance.FirstAidPickUpDrop += PickUp;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    private void UnsubscribeEvents()
    {
        DropEventManager.Instance.FirstAidPickUpDrop -= PickUp;
    }

    private void PickUp()
    {
        _pickUPCount++;
        OnPropertyChanged(nameof(CurrentCountValue));
    }

    protected internal override void ChallengeReward()
    {
        CurrentCountValue = 0;
        _dropManager.ÑountForFirstAid -= _increaseChance;
        OnPropertyChanged(nameof(CurrentCountValue));
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
