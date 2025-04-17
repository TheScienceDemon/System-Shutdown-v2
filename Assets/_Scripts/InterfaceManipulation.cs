using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class InterfaceManipulation : MonoBehaviour {
    public static InterfaceManipulation Singleton { get; private set; }

    [SerializeField] GameObject countdownDisplay;
    [SerializeField] GameObject engageButton;

    const float FADE_DURATION = 4f;

    void Awake() {
        Singleton = this;
    }

    public void HideEngageButton() {
        var tween = engageButton.GetComponent<Image>().DOFade(0f, FADE_DURATION);
        engageButton.GetComponentInChildren<TMP_Text>().DOFade(0f, FADE_DURATION);

        tween.OnComplete(() => {
            countdownDisplay.GetComponent<TMP_Text>().DOFade(1f, FADE_DURATION);
        });
    }

    public void ShowEngageButton() {
        var tween = countdownDisplay.GetComponent<TMP_Text>().DOFade(0f, FADE_DURATION);

        tween.OnComplete(() => {
            engageButton.GetComponent<Image>().DOFade(1f, FADE_DURATION);
            engageButton.GetComponentInChildren<TMP_Text>().DOFade(1f, FADE_DURATION);
        });
    }
}
