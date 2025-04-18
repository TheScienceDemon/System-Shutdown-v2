using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManipulation : MonoBehaviour {
    public static InterfaceManipulation Singleton { get; private set; }

    [SerializeField] GameObject countdownText;
    [SerializeField] GameObject countdownDisplay;
    [SerializeField] GameObject engageButton;

    void Awake() {
        Singleton = this;
    }

    public void HideEngageButton() {
        KillCurrentTweens();

        engageButton.GetComponent<Image>()
            .DOFade(0f, 3f);

        engageButton.GetComponentInChildren<TMP_Text>()
            .DOFade(0f, 3f)

        .OnComplete(() => {
            DOVirtual.DelayedCall(4f, () => {
                countdownText.GetComponentInChildren<TMP_Text>()
                    .DOFade(1f, 4.5f)

                .OnComplete(() => {
                    countdownDisplay.GetComponent<TMP_Text>()
                        .DOFade(1f, 1f);
                });
            });
        });    
    }

    public void ShowEngageButton() {
        KillCurrentTweens();

        countdownDisplay.GetComponent<TMP_Text>()
            .DOFade(0f, 1f);

        countdownText.GetComponent<TMP_Text>()
            .DOFade(0f, 1f)
        
        .OnComplete(() => {
            DOVirtual.DelayedCall(2.5f, () => {
                engageButton.GetComponent<Image>()
                    .DOFade(1f, 1f);

                engageButton.GetComponentInChildren<TMP_Text>()
                    .DOFade(1f, 1f);
            });
        });
    }

    void KillCurrentTweens() {
        DOTween.KillAll();
    }
}
