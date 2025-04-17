using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarheadController : MonoBehaviour {
    [SerializeField] TMP_Text countdownDisplay;
    [SerializeField] Button cancelButtonComp;
    [SerializeField] float displayTimeAfter;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip detonationSequenceClip;
    [SerializeField] AudioClip cancelClip;

    // const int TIME_UNTIL_DETONATION = 30;
    const string TIME_UNTIL_DETONATION_SUB = "[Zeit zur Detonation]";

    float timeUntilDetonation;
    bool shouldCountDown;
    bool shutdownInProgress;

    void Awake() {
        ResetTimeUntilDetonation();
    }

    public void EngageWarhead() {
        shouldCountDown = true;

        source.clip = detonationSequenceClip;
        source.Play();
    }

    public void CancelWarhead(bool playCancelSound) {
        ResetTimeUntilDetonation();

        shouldCountDown = false;
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

    void CountDown() {
        if (!shouldCountDown) {
            return;
        }

        timeUntilDetonation -= Time.deltaTime;

        if (cancelButtonComp.interactable && timeUntilDetonation <= 15f) {
            cancelButtonComp.interactable = false;
        }

        if (shutdownInProgress || timeUntilDetonation > 0f) {
            return;
        }

        SystemShutdown();
    }

    void UpdateCountdownDisplay() {
        countdownDisplay.text = RemainingTimeToString(timeUntilDetonation);
    }
    #endregion

    void SystemShutdown() {
        shutdownInProgress = true;

        Debug.Log($"[{nameof(WarheadController)}] System now shutting down!");
    }

    string RemainingTimeToString(float remainingTime) {
        if (remainingTime <= 0f) {
            return
                $"00:00:000\n" +
                $"{TIME_UNTIL_DETONATION_SUB}";
        }

        if (remainingTime > displayTimeAfter) {
            return RemainingTimeToString(displayTimeAfter);
        }

        var mins = Mathf.Floor(remainingTime / 60 % 60);
        var secs = Mathf.Floor(remainingTime % 60);
        var millisecs = Mathf.Floor(remainingTime * 1000 % 1000);

        return
            $"{mins:00}:{secs:00}:{millisecs:000}\n" +
            $"{TIME_UNTIL_DETONATION_SUB}";
    }

    void ResetTimeUntilDetonation() {
        timeUntilDetonation = detonationSequenceClip.length;
    }
}