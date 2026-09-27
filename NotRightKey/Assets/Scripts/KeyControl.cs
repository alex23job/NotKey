using UnityEngine;

public class KeyControl : MonoBehaviour
{
    [SerializeField] private int _keyID = 0;
    [SerializeField] private int _keyMode = 0;

    public int KeyID { get { return _keyID; } }
    /// <summary>
    /// режим клавиши: 0 - без эффектов и бонусов, 1 - с эффектом, 2 - с бонусом, 3 - с эффектом и бонусом
    /// </summary>
    public int KeyMode { get { return _keyMode; } }

    public EffectType TypeEffect { get { return (_keyEffect != null ? _keyEffect.Type : EffectType.None); } }

    public bool IsEnemy { get; set; }

    private IKeyEffect _keyEffect = null;

    private Animator _animator;

    private MeshRenderer _meshRenderer;
    private Material _oldMat = null;
    private Material _effectMat = null;

    private GameObject _bonus = null;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        if ( _meshRenderer != null )
        {
            Material[] materials = _meshRenderer.materials;
            _oldMat = materials[materials.Length - 1];
        }
    }

    private void OnEnable()
    {
        EffectSignals.OnEnemyLoss += ResetEnemy;
    }
    private void OnDisable()
    {
        EffectSignals.OnEnemyLoss -= ResetEnemy;
    }

    private void ResetEnemy(int code)
    {
        int id = code & 0xffff;
        //print($"ResetEnemy code=0x{code:X04} id=0x{id:X04} KeyID=0x{_keyID:X04}");
        if (id == _keyID)
        {
            print($"ResetEnemy code=0x{code:X04} id=0x{id:X04} KeyID=0x{_keyID:X04}");
            IsEnemy = false;
        }
    }

    public void PlayPush()
    {
        //print($"Push {name}");
        Invoke("StartPush", 0.25f);
    }

    private void StartPush()
    {
        _animator.SetBool("IsPush", true);
        Invoke("EndPush", 0.2f);
    }

    private void EndPush()
    {
        _animator.SetBool("IsPush", false);
    }

    public void SetKeyMode(int mode)
    {
        _keyMode = mode;
    }

    public void SetParams(Material effMat)
    {
        _effectMat = effMat;
        _keyEffect = GetComponent<IKeyEffect>();        
    }

    public void SetBonus(GameObject bonus = null)
    {
        Vector3 pos = transform.position;
        pos.y += 2.5f;
        _bonus = Instantiate(bonus, pos, Quaternion.identity); 
    }

    public void AcceptEffect()
    {
        if (_keyEffect != null)
        {
            _keyEffect.Execute(transform);
        }
    }

    public void ViewEffectMat(bool value)
    {
        if ((_meshRenderer != null) && (_effectMat != null) && (_oldMat != null))
        {
            Material[] materials = _meshRenderer.materials;
            materials[materials.Length - 1] = (value) ? _effectMat : _oldMat;
            _meshRenderer.materials = materials;
        }
    }
}
