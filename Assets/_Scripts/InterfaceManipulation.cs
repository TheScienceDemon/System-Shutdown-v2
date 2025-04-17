using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InterfaceManipulation : MonoBehaviour {
    public static InterfaceManipulation Singleton { get; private set; }

    [SerializeField] GameObject countdownDisplay;
    [SerializeField] GameObject engageButton;

    List<DG.Tweening.Core.TweenerCore<Color, Color,
        DG.Tweening.Plugins.Options.ColorOptions>> currentTweens =
        new List<DG.Tweening.Core.TweenerCore<Color, Color,
            DG.Tweening.Plugins.Options.ColorOptions>>();

    const float FADE_DURATION = 4f;

    void Awake() {
        Singleton = this;
    }

    public void HideEngageButton() {
        KillCurrentTweens();

        var tween1 = engageButton.GetComponent<Image>()
            .DOFade(0f, FADE_DURATION);

        var tween2 = engageButton.GetComponentInChildren<TMP_Text>()
            .DOFade(0f, FADE_DURATION);

        currentTweens.Add(tween1);
        currentTweens.Add(tween2);

        tween1.OnComplete(() => {
            currentTweens.Remove(tween1);
            currentTweens.Remove(tween2);

            var tween3 = countdownDisplay.GetComponent<TMP_Text>()
                .DOFade(1f, FADE_DURATION);

            currentTweens.Add(tween3);
        });
    }

    public void ShowEngageButton() {
        KillCurrentTweens();

        var tween1 = countdownDisplay.GetComponent<TMP_Text>()
            .DOFade(0f, FADE_DURATION);

        currentTweens.Add(tween1);

        tween1.OnComplete(() => {
            currentTweens.Remove(tween1);

            var tween2 = engageButton.GetComponent<Image>()
                .DOFade(1f, FADE_DURATION);

            var tween3 = engageButton.GetComponentInChildren<TMP_Text>()
                .DOFade(1f, FADE_DURATION);

            currentTweens.Add(tween1);
            currentTweens.Add(tween2);
        });
    }

    void KillCurrentTweens() {
        foreach (var tween in currentTweens) {
            tween.Kill();
        }

        currentTweens.Clear();
    }
}
