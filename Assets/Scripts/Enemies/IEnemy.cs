using UnityEngine;

public interface IEnemy
{
    void TakeDamage(int damage);

    void Freeze(float freezeTime);

    void SetPlayer(GameObject player);

    bool isDying { get; }

    void resetEnemy();
}
