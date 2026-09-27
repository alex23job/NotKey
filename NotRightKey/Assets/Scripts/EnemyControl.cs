using UnityEngine;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private GameObject arm;
    [SerializeField] private Image imgHp;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private ParticleSystem _bum;

    private Animator _anim = null;
    private int _damage = 10;
    private int _hp = 50;
    private int _hpMax = 50;
    private int _value = 0;
    private int _keyID = -1;
    //private ParticleSystem _bum;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _bum.gameObject.SetActive(false);
    }

    public void SetParams(/*ParticleSystem ps,*/ int hpMax, int dmg, int val, int id)
    {
        //_bum = ps;
        _hpMax = hpMax;
        _hp = hpMax;
        _damage = dmg;
        _value = val;
        _keyID = id;
        RotateModel(Vector3.left);
    }

    public void RotateModel(Vector3 direction)
    {
        modelTransform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public void Damage(int dmg)
    {
        _hp -= dmg;
        if (_hp < 0) _hp = 0;
        imgHp.fillAmount = (float)_hp / (float)_hpMax;
        if (_hp == 0)
        {
            EffectSignals.RaiseEnemyLoss((_value << 16) + _keyID);
            //_bum.transform.position = transform.position;
            _bum.gameObject.SetActive(true);
            _bum.Play();
            _anim.SetBool("IsSlash", false);
            _anim.SetBool("IsLoss", true);
            Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player"))
        {
            Invoke("StartAttack", 0.5f);
            Vector3 dir = other.transform.position - transform.position;dir.y = 0;
            RotateModel(dir);
        }        
    }

    private void StartAttack()
    {
        _anim.SetBool("IsSlash", true);
        Invoke("EndAttack", 1.1f);
    }

    private void EndAttack()
    {
        _anim.SetBool("IsSlash", false);
    }
}
