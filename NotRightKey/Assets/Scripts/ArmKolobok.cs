using UnityEngine;

public class ArmKolobok : MonoBehaviour
{
    private int damage = 10;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyControl enemyControl = other.gameObject.GetComponent<EnemyControl>();
            if (enemyControl != null)
            {
                enemyControl.Damage(damage);
            }
        }
    }
}
