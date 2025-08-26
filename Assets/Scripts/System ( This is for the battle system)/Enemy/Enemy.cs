public class Enemy
{
    public string Name;
    public int HP;
    public int AttackDamage;

    public void Attack(Player player)
    {
        player.HP -= AttackDamage;
    }
}
