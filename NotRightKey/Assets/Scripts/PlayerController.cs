using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; // Обязательно для кликов UI
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerData))]
public class PlayerController : MonoBehaviour //, IPointerClickHandler
{
    [SerializeField] private KeysBoard keysBoard;
    [SerializeField] private LevelUI levelUI;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private LayerMask obstacleLayer;
    
    [Header("References")]
    [SerializeField] private Transform modelTransform;
    private Animator _animator;
    private Rigidbody _rb;
    private GameControls _controls; // Наш сгенерированный класс
    private bool _isBusy;

    [Header("Game Rules")][SerializeField]
    private bool forbidRepeatingMoves = true; // Включатель правила на случай теста

    // Состояние последнего шага
    private Vector3 _lastMoveDirection = Vector3.zero;
    private const float Epsilon = 0.01f; // Для сравнения float-векторов

    private PlayerData _playerData;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
        _playerData = GetComponent<PlayerData>();
        
        // Инициализация НОВОЙ схемы управления
        _controls = new GameControls();
    }
    
    private void OnEnable()
    {
        // --- КЛАВИАТУРА ---
        _controls.Player.MoveUp.performed += ctx => TryMove(Vector3.forward);
        _controls.Player.MoveDown.performed += ctx => TryMove(Vector3.back);
        _controls.Player.MoveLeft.performed += ctx => TryMove(Vector3.left);
        _controls.Player.MoveRight.performed += ctx => TryMove(Vector3.right);
        _controls.Player.AttackShiftLeft.performed += ctx => StartCoroutine(AttackRoutine(true));
        _controls.Player.AttackCtrlLeft.performed += ctx => StartCoroutine(AttackRoutine(false));
        _controls.Player.Enable();
    }
    
    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    public void OnButtonClick(string btnName)
    {
        //if (_isBusy || eventData.button != PointerEventData.InputButton.Left) return;
        //string btnName = eventData.selectedObject.name;
        switch (btnName)
        {
            case "BtnUp": TryMove(Vector3.forward); levelUI.ArrowClick(0); break;
            case "BtnDown": TryMove(Vector3.back); levelUI.ArrowClick(1); break;
            case "BtnLeft": TryMove(Vector3.left); levelUI.ArrowClick(2); break;
            case "BtnRight": TryMove(Vector3.right); levelUI.ArrowClick(3); break;
            case "BtnLeftShift": StartCoroutine(AttackRoutine(true)); levelUI.ArrowClick(-1); break;
            case "BtnLeftAlt": StartCoroutine(AttackRoutine(false)); levelUI.ArrowClick(-1); break;
            case "MenuBtn": LoadMenu(); break;
        }
    }

    private void TryMove(Vector3 direction)
    {
        if (_isBusy || keysBoard == null) return;

        // 2. ПРАВИЛО ИГРЫ: Проверка повторяющегося направления
        if (forbidRepeatingMoves && IsSameDirection(direction, _lastMoveDirection))
        {
            //Debug.Log($"Нельзя нажимать '{GetKeyName(direction)}' два раза подряд!");
            return; // Выходим из метода, ничего не происходит
        } // Если проверка пройдена - обновляем состояние ДО начала движения
        
        _lastMoveDirection = direction;
        if (direction == Vector3.forward) levelUI.ArrowClick(0);
        if (direction == Vector3.back) levelUI.ArrowClick(1);
        if (direction == Vector3.left) levelUI.ArrowClick(2);
        if (direction == Vector3.right) levelUI.ArrowClick(3);

        //if (_playerData.DecrementViewDelay()) keysBoard;

        // 3. Поворачиваем модель до начала движения
        if (modelTransform != null)
        {
            modelTransform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        
        // 2. Запрашиваем координату цели у Менеджера Доски
        Vector3 destination = keysBoard.GetTargetPosition(transform.position, direction);
        destination.y += 2f;
        
        // 3. Запускаем корутину перемещения
        StartCoroutine(MoveRoutine(destination, direction));
        
        /*Vector3 targetPos = transform.position + direction * moveDistance;

        // Проверка препятствий Сферой (лучше работает в 3D)
        Collider[] hits = Physics.OverlapSphere(targetPos, 0.4f, obstacleLayer);
        if (hits.Length == 0)
        {
            StartCoroutine(MoveRoutine(targetPos, direction));
        }*/
    }

    /// <summary>
    /// Вспомогательный метод для сравнения двух векторов с учетом погрешности float
    /// </summary>
    private bool IsSameDirection(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < Epsilon;
    }    

    private IEnumerator MoveRoutine(Vector3 destination, Vector3 lookDirection)
    {
        _isBusy = true;
        _animator.SetFloat("Speed", 1f);
        //print($"dest={destination}   dir={lookDirection}");
        float duration = 0.4f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, destination, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = destination;

        _animator.SetFloat("Speed", 0f);
        _animator.gameObject.transform.localPosition = Vector3.zero;
        _animator.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(-90F, 0, -270F));

        yield return new WaitForSeconds(0.2f);

        _isBusy = false;
    }
    
    private IEnumerator AttackRoutine(bool isLight)
    {
        if (_isBusy) yield break;
        levelUI.ArrowClick(-1);
        _lastMoveDirection = Vector3.zero;
        _isBusy = true;
        //_animator.SetTrigger(isLight ? "DoLightAttack" : "DoHeavyAttack");
        _animator.SetBool(isLight ? "DoLightAttack" : "DoHeavyAttack", true);
        AnimatorClipInfo[] clipInfos = _animator.GetCurrentAnimatorClipInfo(0);
        print($"len={clipInfos.Length} {clipInfos[0].clip.name} {clipInfos[0].clip.length}");
        //float animationLength = clipInfos.Length > 0 ? clipInfos[0].clip.length : 0.5f;
        float animationLength = isLight ? 0.2f : 0.4f;
        yield return new WaitForSeconds(animationLength);
        _animator.SetBool("DoLightAttack", false);
        _animator.SetBool("DoHeavyAttack", false);
        _animator.gameObject.transform.localPosition = Vector3.zero;
        _animator.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(-90F, 0, -270F));
        _isBusy = false;
    }
    
    public void LoadMenu()
    {
        Debug.Log("Переход в меню..."); // SceneManager.LoadScene("MainMenu");
    }
}
