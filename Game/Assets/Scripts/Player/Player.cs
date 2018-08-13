using System.Collections;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _parts; // For fading
    [SerializeField] private LookDirectionIndicator _lookDirectionIndicator;

    #region Injected

    private InputManager _inputManager;
    private PlayerMovement _movement; // todo: just use GetComponent<PlayerMovement>(); ?
    private ControllerSettings _controllerSettings;
    private PlayerSettings _settings;
    private GameplaySettings _gameplaySettings;
    private BulletManager _bulletManager;
    [InjectOptional] private FogManager _fogManager;
    [InjectOptional] private GameManager _gameManager;

    #endregion

    [Header("_DEBUG")]
    public PlayerSettings Stats;
    public Vector2 LookDirection { get; private set; }
    public bool Enabled {
        get { return _movement.enabled; }
        set
        {
            _movement.enabled = value;
            _lookDirectionIndicator.Showing = value;
        }
    }

    public bool IsInFog { get; private set; }
    private float _fogCorruption; // range of 0 - 1 (above 1 = death)
    public float FogCorruption {
        get { return _fogCorruption; }
        private set
        {
            _fogCorruption = value;
            if (value >= 1 && _gameManager.State == GameState.Playing)
                _gameManager.GameOver();
        } 
    }

    private float _lastShot;

    [Inject]
    private void Construct(InputManager inputManager, PlayerMovement movement, ControllerSettings controllerSettings, 
        GameplaySettings gameplaySettings, BulletManager bulletManager)
    {
        _inputManager = inputManager;
        _movement = movement;
        _controllerSettings = controllerSettings;
        _settings = gameplaySettings.PlayerSettings;
        _gameplaySettings = gameplaySettings;
        _bulletManager = bulletManager;
        //_fogManager = fogManager;
    }

    private void Start()
    {
        Stats = Instantiate(_gameplaySettings).PlayerSettings;
    }

    private void Update()
    {
        if (!Enabled)
            return;

        LookDirection = GetLookDirection();
        _lookDirectionIndicator.LookDirection = LookDirection;        

        if (Input.GetButton(InputAxes.Fire1) || Input.GetAxisRaw(InputAxes.Fire1) > _controllerSettings.DeadZone)
            Shoot();
        
        if (IsInFog)
        {
            FogCorruption += Time.deltaTime / Stats.FogSurvivalTime;
        }
        else if (FogCorruption > 0)
        {
            FogCorruption -= Stats.FogCorruptionRestoreRate * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tag.Fog.ToString())
            IsInFog = true;

        if (other.tag == Tag.Enemy.ToString())
        {
            var enemy = other.GetComponent<Enemy>();
            enemy.Attack(this);
        }

        if (other.tag == Tag.Bullet.ToString())
        {
            // Bullet hit
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null && bullet.Owner == BulletOwner.Enemy && !bullet.DespawnPending)
            {
                // Player was hit by a enemy bullet
                FogCorruption += 0.05f;
                _fogManager.TakeTime(bullet.Damage);

                bullet.Hit(transform);
            }
        }

        if (other.tag == Tag.Powerup.ToString())
        {
            var powerup = other.GetComponent<Powerup>();
            if (powerup != null && !powerup.Used)
            {
                powerup.Use();

                switch (powerup.Type)
                {
                    case PowerupType.FireRate:
                        Stats.FireRate -= 0.05f;
                        if (Stats.FireRate < 0.05f)
                            Stats.FireRate = 0.05f;
                        break;
                    case PowerupType.Damage:
                    default:
                        Stats.BulletDamage += _gameplaySettings.PowerupStatIncrease;
                        break;                        
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Tag.Fog.ToString())
            IsInFog = false;

        if (other.tag == Tag.Enemy.ToString())
        {
            var enemy = other.GetComponent<Enemy>();
            enemy.StopAttack(this);
        }
    }

    public void Corrupt(float amount)
    {
        FogCorruption += amount + Stats.FogCorruptionRestoreRate * Time.deltaTime; // + restore compensation
    }

    public void FadeIn(float waitTime, float duration)
    {
        var originalAlphas = new float[_parts.Length];
        for (var i = 0; i < _parts.Length; i++)
        {
            var part = _parts[i];
            originalAlphas[i] = part.color.a; // save alpha value for later (to restore back)
            part.color = new Color(part.color.r, part.color.g, part.color.b, 0);
        }

        StartCoroutine(FadeInCoroutine(waitTime, duration, originalAlphas));
    }

    private IEnumerator FadeInCoroutine(float waitTime, float duration, float[] originalAlphas)
    {
        yield return new WaitForSeconds(waitTime);

        for (var i = 0; i < _parts.Length; i++)
        {
            var part = _parts[i];
            part.DOFade(originalAlphas[i], duration);
        }
    }

    private Vector2 GetLookDirection()
    {
        switch (_inputManager.CurrentInputType)
        {
            case InputType.Controller:
                return GetLookDirectionController();
            case InputType.KeyboardAndMouse:
            default:
                return GetLookDirectionMouse();
        }
    }

    private Vector2 GetLookDirectionController()
    {
        var xAxisRight = Input.GetAxisRaw(InputAxes.HorizontalRight);
        var yAxisRight = Input.GetAxisRaw(InputAxes.VerticalRight);
        var axesRight = new Vector2(xAxisRight, yAxisRight);

        if (axesRight.magnitude > 0.2f)
            return axesRight;
        return LookDirection;
    }

    private Vector2 GetLookDirectionMouse()
    {
        var mouseScreenPosition = Input.mousePosition;
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        var relativePositionToMouse = mouseWorldPosition - transform.position;
        return relativePositionToMouse.normalized;
    }

    private void Shoot()
    {
        if (Time.time - _lastShot < Stats.FireRate)
            return;

        _lastShot = Time.time;

        _bulletManager.Create(BulletOwner.Player, transform.position, LookDirection.normalized * _settings.BulletSpeed);
    }
}
