using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class PanicCanvas : MonoBehaviour
{
    [TitleGroup("Increase Panic Gauge")]
    [PropertyTooltip("Sets the number of seconds before the panic gauge increments")]
    public float increaseSeconds = 0;
    [TitleGroup("Increase Panic Gauge")]
    [PropertyTooltip("Sets the panic gauge increment value every x seconds")]
    public float normalIncreaseValue = 0;
    [TitleGroup("Increase Panic Gauge")]
    [PropertyTooltip("Sets the panic gauge increment value every x seconds when the player is int the giant's eye zone")]
    public float giantZoneIncreaseValue = 0;
    [TitleGroup("Increase Panic Gauge")]
    [PropertyTooltip("Sets the panic gauge increment value every x seconds when the player leave the giant's eye zone and that he hasn't done rock balancing ")]
    public float beforeRbIncreaseValue = 0;

    [TitleGroup("Decrease Panic Gauge")]
    [PropertyTooltip("Sets the number of seconds before the panic gauge decrement")]
    public float decreaseSeconds = 0;
    [TitleGroup("Decrease Panic Gauge")]
    [PropertyTooltip("Sets the panic gauge decrement value every x seconds")]
    public float decreaseValue = 0;

    [TitleGroup("Panic effect")]
    public float startPanicEffectValue = 0;
    [TitleGroup("Panic effect")]
    public float startBigPanicEffectValue = 0;

    [TitleGroup("Canvas effect")]
    [SerializeField]
    private CanvasGroup _largePanicEffect;
    [TitleGroup("Canvas effect")]
    [SerializeField]
    private CanvasGroup _smallPanicEffect;

    private float _timer;
    private bool _increaseIsPlaying = false;
    private bool _decreaseIsPlaying = false;

    [TitleGroup("Bool")]
    [SerializeField]
    private bool giantZone = false;
    [TitleGroup("Bool")]
    [SerializeField]
    private bool beforeRb = false;

    private PlayerParameters _playerParameters;

    public static PanicCanvas Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _playerParameters = PlayerParameters.Instance;
    }

    private void Update()
    {
        IncreasePanicGauge();
    }

    #region Gauge
    public void IncreasePanicGauge()
    {
        if (!_increaseIsPlaying)
        {
            _increaseIsPlaying = true;
            float increment = _playerParameters.panicGauge;

            if (giantZone)
                increment += giantZoneIncreaseValue;
            else if (!giantZone && beforeRb)
                increment += beforeRbIncreaseValue;
            else
                increment += normalIncreaseValue;

            DOTween.To(() => _playerParameters.panicGauge, x => _playerParameters.panicGauge = x, increment, increaseSeconds).OnComplete(()=> _increaseIsPlaying = false);
        }
    }

    public void DecreasePanicGauge()
    {
        if (_timer >= 0f)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            DOTween.To(() => _playerParameters.panicGauge, x => _playerParameters.panicGauge = x, _playerParameters.panicGauge -= decreaseValue, 1.5f);
            _timer = increaseSeconds;
        }
    }
    #endregion
}
