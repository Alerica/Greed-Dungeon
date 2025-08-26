public enum BattleState { PlayerTurn, EnemyTurn, Victory, Defeat }

public class BattleManager {
    Player player;
    Enemy enemy;
    BattleState state;

    public void NextTurn() {
        if (state == BattleState.PlayerTurn) {
            state = BattleState.EnemyTurn;
            enemy.Attack(player);
            state = BattleState.PlayerTurn;
            player.Energy = 5; // reset energy
            player.DrawCards(5); // refill hand
        }
    }
}
