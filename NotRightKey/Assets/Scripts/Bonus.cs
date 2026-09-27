using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private int type = 0;
    [SerializeField] private int value = 0;

    /// <summary>
    /// 0 - +HP, 1 - +Exp, 2 - +Many, 3 - +Energy, 4 - Antidot, 5 - Shild, 6 - Water, 7 - Eye 
    /// </summary>
    public int BonusType { get { return type; } }
    public int BonusValue { get { return value; } }

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SizeMin()
    {
        _anim.SetTrigger("IsMin");
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SizeMin();
            PlayerData playerData = other.gameObject.GetComponent<PlayerData>();
            if (playerData != null )
            {
                playerData.AcceptBonus(GetComponent<Bonus>());
            }
        }
    }
}
