using UnityEngine;

public class FallingObject : MonoBehaviour
{
    private int damage = 10;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerData playerData = other.gameObject.GetComponent<PlayerData>();
            if (playerData != null)
            {
                playerData.Damage(damage);
            }
        }
        if (other.CompareTag("Enemy"))
        {
            EnemyControl enemyControl = other.gameObject.GetComponent<EnemyControl>();
            if (enemyControl != null)
            {
                enemyControl.Damage(damage);
            }
        }
        Destroy(gameObject, 0.1f);
    }
}
