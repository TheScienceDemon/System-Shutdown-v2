using System.Diagnostics; // Required, don't remove !!!
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class WarheadController : MonoBehaviour {
    public static WarheadController Singleton { get; private set; }

    [SerializeField] TMP_Text countdownDisplay;
    [SerializeField] Button cancelButtonComp;
    [SerializeField] float displayTimeAfter;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip detonationSequenceClip;
    [SerializeField] AudioClip last20Secs;
    [SerializeField] AudioClip cancelClip;

    // const int TIME_UNTIL_DETONATION = 30;
    const string TIME_UNTIL_DETONATION_SUB = "[Zeit zur Detonation]";

    float timeUntilDetonation;
    bool isRunning;
    bool counting20Secs;
    bool shutdownInProgress;

    #region Getter
    public bool GetIsRunning() {
        return isRunning;
    }

    public float GetTimeUntilDetonation() {
        return timeUntilDetonation;
    }
    #endregion

    void Awake() {
        Singleton = this;
    }

    void Start() {
        ResetTimeUntilDetonation();
        InvokeRepeating(nameof(UpdateDiscordActivity), 0f, 4f);
    }

    public void EngageWarhead() {
        InterfaceManipulation.Singleton.HideEngageButton();
        isRunning = true;
        counting20Secs = false;

        source.clip = detonationSequenceClip;
        source.Play();
    }

    public void CancelWarhead(bool playCancelSound) {
        if (!isRunning) {
            return;
        }

        ResetTimeUntilDetonation();
        DiscordManager.Singleton.SetStartupActivity();

        InterfaceManipulation.Singleton.ShowEngageButton();
        isRunning = false;
        source.Stop();

        if (playCancelSound) {
            source.clip = cancelClip;
            source.Play();
        }
    }

    #region Update + related
    void Update() {
        CountDown();
        UpdateCountdownDisplay();
    }

    void UpdateDiscordActivity() {
        if (!isRunning) { return; }

        DiscordManager.Singleton.SetDetonationActivity();
    }

    void CountDown() {
        if (!isRunning) { return; }

        timeUntilDetonation -= Time.deltaTime;

        if (cancelButtonComp.interactable && timeUntilDetonation <= 25f) {
            cancelButtonComp.interactable = false;
        }

        if (!counting20Secs && timeUntilDetonation <= 20f + 1f) {
            CountLast20Secs();
        }

        if (!shutdownInProgress && timeUntilDetonation <= 0f) {
            SystemShutdown();
        }
    }

    void UpdateCountdownDisplay() {
        countdownDisplay.text = RemainingTimeToString(timeUntilDetonation);
    }
    #endregion

    void CountLast20Secs() {
        counting20Secs = true;

        source.PlayOneShot(last20Secs);
    }

    void SystemShutdown() {
        shutdownInProgress = true;

#if !UNITY_EDITOR
        var shutdownProcess = new ProcessStartInfo("shutdown", "/s /t 0") {
            CreateNoWindow = true,
            UseShellExecute = false
        };

        Process.Start(shutdownProcess);
#else
        Debug.Log($"[{nameof(WarheadController)}] System now shutting down!");
#endif
    }

    string RemainingTimeToString(float remainingTime) {
        if (remainingTime <= 0f) {
            return
                $"00:00:000";
        }

        if (remainingTime > displayTimeAfter) {
            return RemainingTimeToString(displayTimeAfter);
        }

        var mins = Mathf.Floor(remainingTime / 60 % 60);
        var secs = Mathf.Floor(remainingTime % 60);
        var millisecs = Mathf.Floor(remainingTime * 1000 % 1000);

        return $"{mins:00}:{secs:00}:{millisecs:000}";
    }

    void ResetTimeUntilDetonation() {
        timeUntilDetonation = detonationSequenceClip.length;
    }
}