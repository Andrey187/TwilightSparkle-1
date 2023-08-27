public interface ILevelUpSystem
{
    public int GainExpMyltiply { get; set; }
    public void AddExperience(EnemyData.ObjectType type, EnemyData enemyType) { }

    public void AddExperienceForChallenge(int gainExp) { }
}
