using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InterfaceManipulation : MonoBehaviour {
    public static InterfaceManipulation Singleton { get; private set; }

    [Header("Main Display")]
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text countdownText;
    [SerializeField] TMP_Text countdownDisplay;
    [SerializeField] GameObject engageButton;

    [Header("Second Display/s")]
    [SerializeField] GameObject displayInstancePrefab;
    [SerializeField] Transform displayInstanceHolder;

    [SerializeField] List<DisplayInstance> otherDisplays;

    [SerializeField, System.Obsolete] TMP_Text infoText2;
    [SerializeField, System.Obsolete] GameObject countdownText2;
    [SerializeField, System.Obsolete] GameObject countdownDisplay2;

    TMP_Text[] InfoTexts {
        get {
            var texts = new TMP_Text[otherDisplays.Count + 1];
            texts[0] = infoText;

            for (int i = 0; i < otherDisplays.Count; i++) {
                texts[i + 1] = otherDisplays[i].Infobox;
            }

            return texts;
        }
    }

    TMP_Text[] CountdownTexts {
        get {
            var texts = new TMP_Text[otherDisplays.Count + 1];
            texts[0] = countdownText;

            for (int i = 0; i < otherDisplays.Count; i++) {
                texts[i + 1] = otherDisplays[i].CountdownText;
            }

            return texts;
        }
    }

    public TMP_Text[] CountdownDisplays {
        get {
            var displays = new TMP_Text[otherDisplays.Count + 1];
            displays[0] = countdownDisplay;

            for (int i = 0; i < otherDisplays.Count; i++) {
                displays[i + 1] = otherDisplays[i].CountdownDisplay;
            }

            return displays;
        }
    }

    const float FADE_DURATION = 4f;
    const string WARHEAD_OS_VERSION_TEMPLATE = "Running WarheadOS v[version]";

    /* // :tr: ///////////////////////////////////////////////////////
    List<DG.Tweening.Core.TweenerCore<Color, Color,
        DG.Tweening.Plugins.Options.ColorOptions>> currentTweens =
        new List<DG.Tweening.Core.TweenerCore<Color, Color,
            DG.Tweening.Plugins.Options.ColorOptions>>();
    */ ///////////////////////////////////////////////////////////////

    [System.Obsolete]
    public TMP_Text CountdownText {
        get => countdownDisplay.GetComponent<TMP_Text>();
    }

    int DisplaysInUse {
        get {
#if UNITY_EDITOR
            return 2;
#else
            return Display.displays.Length > 1;
#endif
        }
    }

    #region Unity Messages
    void Awake() {
        Singleton = this;
    }

    void Start() {
        for (int i = 1; i < DisplaysInUse; i++) {
            var newDisplay = Instantiate(displayInstancePrefab, displayInstanceHolder);
            newDisplay.name = $"Display Instance - {i + 1}";

            var script = newDisplay.GetComponent<DisplayInstance>();

            otherDisplays.Add(script);

            script.SetupInstance(i);
        }

        foreach (var text in InfoTexts) {
            text.text =
                WARHEAD_OS_VERSION_TEMPLATE.Replace(
                    "[version]",
                    WarheadController.WARHEAD_OS_VERSION);
        }
    }
    #endregion

    public void HideEngageButton() {
        KillCurrentTweens();

        var tween1 = engageButton.GetComponent<Image>()
            .DOFade(0f, FADE_DURATION);

        engageButton.GetComponentInChildren<TMP_Text>()
            .DOFade(0f, FADE_DURATION);

        tween1.OnComplete(() => {
            // kms
            DG.Tweening.Core.TweenerCore
                <Color, Color, DG.Tweening.Plugins.Options.ColorOptions>
                    tween2 = null;

            for (int i = 0; i < CountdownTexts.Length; i++) {
                if (i == 0) {
                    tween2 = CountdownTexts[i].DOFade(1f, FADE_DURATION);
                } else {
                    CountdownTexts[i].DOFade(1f, FADE_DURATION);
                }
            }

            tween2.OnComplete(() => {
                foreach (var countdownDisplay in CountdownDisplays) {
                    countdownDisplay.DOFade(1f, FADE_DURATION);
                }
            });
        });
    }

    public void ShowEngageButton() {
        KillCurrentTweens();

        // kms^2
        DG.Tweening.Core.TweenerCore
            <Color, Color, DG.Tweening.Plugins.Options.ColorOptions>
                tween1 = null;

        for (int i = 0; i < CountdownDisplays.Length; i++) {
            if (i == 0) {
                tween1 = CountdownDisplays[i].DOFade(0f, FADE_DURATION);
            } else {
                CountdownDisplays[i].DOFade(0f, FADE_DURATION);
            }
        }

        tween1.OnComplete(() => {
            // kms^3
            DG.Tweening.Core.TweenerCore
                <Color, Color, DG.Tweening.Plugins.Options.ColorOptions>
                    tween2 = null;

            for (int i = 0; i < CountdownTexts.Length; i++) {
                if (i == 0) {
                    tween2 = CountdownTexts[i].DOFade(0f, FADE_DURATION);
                } else {
                    CountdownTexts[i].DOFade(0f, FADE_DURATION);
                }
            }

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
