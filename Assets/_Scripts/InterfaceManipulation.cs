using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManipulation : MonoBehaviour {
    public static InterfaceManipulation Singleton { get; private set; }

    [SerializeField] GameObject countdownText;
    [SerializeField] GameObject countdownDisplay;
    [SerializeField] GameObject engageButton;

    /* // :tr: ///////////////////////////////////////////////////////
    List<DG.Tweening.Core.TweenerCore<Color, Color,
        DG.Tweening.Plugins.Options.ColorOptions>> currentTweens =
        new List<DG.Tweening.Core.TweenerCore<Color, Color,
            DG.Tweening.Plugins.Options.ColorOptions>>();
    */ ///////////////////////////////////////////////////////////////

    const float FADE_DURATION = 4f;

    void Awake() {
        Singleton = this;
    }

    public void HideEngageButton() {
        KillCurrentTweens();

        var tween1 = engageButton.GetComponent<Image>()
            .DOFade(0f, FADE_DURATION);

        engageButton.GetComponentInChildren<TMP_Text>()
            .DOFade(0f, FADE_DURATION);

        tween1.OnComplete(() => {
            var tween2 = countdownText.GetComponentInChildren<TMP_Text>()
                .DOFade(1f, FADE_DURATION);

            tween2.OnComplete(() => {
                countdownDisplay.GetComponent<TMP_Text>()
                    .DOFade(1f, FADE_DURATION);
            });
        });
    }

    public void ShowEngageButton() {
        KillCurrentTweens();

        var tween1 = countdownDisplay.GetComponent<TMP_Text>()
            .DOFade(0f, FADE_DURATION);

        tween1.OnComplete(() => {
            var tween2 = countdownText.GetComponent<TMP_Text>()
                .DOFade(0f, FADE_DURATION);

            tween2.OnComplete(() => {
                engageButton.GetComponent<Image>()
                    .DOFade(1f, FADE_DURATION);

                engageButton.GetComponentInChildren<TMP_Text>()
                    .DOFade(1f, FADE_DURATION);
            });
        });
    }

    void KillCurrentTweens() {
        DOTween.KillAll();
    }
}
