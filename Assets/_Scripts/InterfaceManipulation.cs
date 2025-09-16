using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManipulation : MonoBehaviour {
    public static InterfaceManipulation Singleton { get; private set; }

    [Header("Main Display")]
    [SerializeField] GameObject countdownText;
    [SerializeField] GameObject countdownDisplay;
    [SerializeField] GameObject engageButton;

    [Header("Second Display")]
    [SerializeField] GameObject countdownText2;
    [SerializeField] GameObject countdownDisplay2;

    /* // :tr: ///////////////////////////////////////////////////////
    List<DG.Tweening.Core.TweenerCore<Color, Color,
        DG.Tweening.Plugins.Options.ColorOptions>> currentTweens =
        new List<DG.Tweening.Core.TweenerCore<Color, Color,
            DG.Tweening.Plugins.Options.ColorOptions>>();
    */ ///////////////////////////////////////////////////////////////

    public TMP_Text CountdownText {
        get => countdownDisplay.GetComponent<TMP_Text>();
    }

    public TMP_Text CountdownText2 {
        get => countdownDisplay2.GetComponent<TMP_Text>();
    }

    const float FADE_DURATION = 4f;

    bool UseSecondDisplay {
        get => Display.displays.Length > 1;
    }

    void Awake() {
        Singleton = this;
    }

    void Start() {
        if (!UseSecondDisplay) { return; }

        Display.displays[1].Activate();
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

            countdownText2.GetComponentInChildren<TMP_Text>()
                .DOFade(1f, FADE_DURATION);

            tween2.OnComplete(() => {
                countdownDisplay.GetComponent<TMP_Text>()
                    .DOFade(1f, FADE_DURATION);

                countdownDisplay2.GetComponent<TMP_Text>()
                    .DOFade(1f, FADE_DURATION);
            });
        });
    }

    public void ShowEngageButton() {
        KillCurrentTweens();

        var tween1 = countdownDisplay.GetComponent<TMP_Text>()
            .DOFade(0f, FADE_DURATION);

        countdownDisplay2.GetComponent<TMP_Text>()
            .DOFade(0f, FADE_DURATION);

        tween1.OnComplete(() => {
            var tween2 = countdownText.GetComponent<TMP_Text>()
                .DOFade(0f, FADE_DURATION);

            countdownText2.GetComponent<TMP_Text>()
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
