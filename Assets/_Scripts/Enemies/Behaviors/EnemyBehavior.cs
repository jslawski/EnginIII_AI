public class EnemyBehavior
{
    protected Enemy enemy;

    public void Setup(Enemy thisEnemy)
    {
        this.enemy = thisEnemy;
    }

    public virtual void ExecuteBehavior() {}
}
