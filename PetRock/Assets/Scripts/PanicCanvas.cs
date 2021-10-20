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

    private float incrementValue;

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

    [TitleGroup("Panic Time Out")]
    [PropertyTooltip("Sets the panic gauge to this value after the dialogue with the pet rock in the panic time out")]
    [SerializeField]
    private float afterDialogueValue = 0;

    [TitleGroup("Canvas effect")]
    [SerializeField]
    private CanvasGroup _largePanicEffect;
    [TitleGroup("Canvas effect")]
    [SerializeField]
    private CanvasGroup _smallPanicEffect;

    private bool _tweenIsPlaying = false;
    
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
        if (!_playerParameters.IsOnRockBalancing())
        {
            if (!_playerParameters.IsOnSafeZone())
            {
                switch (_playerParameters.playerStates)
                {
                    case PlayerParameters.PlayerStates.Regular:
                        incrementValue = normalIncreaseValue;
                        break;

                    case PlayerParameters.PlayerStates.GiantZone:
                        incrementValue = giantZoneIncreaseValue;
                        break;

                    case PlayerParameters.PlayerStates.Stressed:
                        incrementValue = beforeRbIncreaseValue;
                        break;

                    default:
                        incrementValue = normalIncreaseValue;
                        break;
                }

                ModifyPanicGauge(incrementValue, increaseSeconds);
            }
            else
                ModifyPanicGauge(-decreaseValue, decreaseSeconds);
        }

        GameManager.instance.vignettePostProcessing.intensity.value = _playerParameters.panicGauge / 100;
    }

    #region Gauge
    public void ModifyPanicGauge(float value, float duration)
    {
        if (!_tweenIsPlaying)
        {
            float newValue = _playerParameters.panicGauge;

            newValue += value;
            _tweenIsPlaying = true;

            DOTween.To(() => _playerParameters.panicGauge, x => _playerParameters.panicGauge = x, Mathf.Clamp(newValue, 0f, 100f), duration).OnComplete(() => _tweenIsPlaying = false);
        }
    }

    public void PanicAfterDialogue()
    {
        DOTween.To(() => _playerParameters.panicGauge, x => _playerParameters.panicGauge = x, afterDialogueValue, 1f);
    }
    #endregion
}
