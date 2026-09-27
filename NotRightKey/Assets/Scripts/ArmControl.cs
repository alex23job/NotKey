using UnityEngine;

public class ArmControl : MonoBehaviour
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
    }
}
