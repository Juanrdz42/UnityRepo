using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class PhotoController : MonoBehaviour
{
    public static PhotoController Instance;

    [Header("UI")]
    public GameObject photoGamePanel;
    public RawImage videoRawImage;
    public TMP_Text resultText;
    public GameObject retryButton;
    public TMP_Text takePhotoHint;

    [Header("Game UI")]
    public GameObject gameUI;

    [Header("Player")]
    public PlayerMove playerMove;

    [Header("Video")]
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;

    private PhotoSpot currentSpot;
    private PhotoSequenceData currentSequence;

    private bool photoTaken = false;
    private bool videoFinished = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (photoGamePanel != null)
            photoGamePanel.SetActive(false);

        if (retryButton != null)
            retryButton.SetActive(false);

        if (videoRawImage != null && renderTexture != null)
        {
            videoRawImage.texture = renderTexture;
            videoRawImage.enabled = false;
        }

        if (resultText != null)
            resultText.text = "";

        if (takePhotoHint != null)
            takePhotoHint.gameObject.SetActive(false);

        if (videoPlayer != null)
        {
            if (renderTexture != null)
                videoPlayer.targetTexture = renderTexture;

            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;
            videoPlayer.waitForFirstFrame = true;
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        ClearRenderTexture();
    }

    public bool IsPhotoGameOpen()
    {
        return photoGamePanel != null && photoGamePanel.activeSelf;
    }

    public void OpenPhotoGame(PhotoSpot spot)
    {
        if (photoGamePanel != null && photoGamePanel.activeSelf)
            return;

        currentSpot = spot;
        currentSequence = spot.sequenceData;
        photoTaken = false;
        videoFinished = false;

        if (currentSequence == null)
        {
            Debug.LogWarning("currentSequence es null");
            return;
        }

        if (currentSequence.videoClip == null)
        {
            Debug.LogWarning("videoClip es null");
            return;
        }

        SFXManager_JuanRdz.Play("TakingCameraOut");

        if (photoGamePanel != null)
            photoGamePanel.SetActive(true);

        if (gameUI != null)
            gameUI.SetActive(false);

        if (playerMove != null)
            playerMove.SetMovementEnabled(false);

        if (retryButton != null)
            retryButton.SetActive(false);

        if (resultText != null)
            resultText.text = "";

        if (takePhotoHint != null)
            takePhotoHint.gameObject.SetActive(true);

        if (videoRawImage != null)
            videoRawImage.enabled = false;

        ClearRenderTexture();

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.clip = currentSequence.videoClip;
            videoPlayer.time = 0;
            videoPlayer.Prepare();
            StartCoroutine(PlayPreparedVideo());
        }
    }

    private void TakePhoto()
    {
        if (videoPlayer == null || currentSequence == null)
            return;

        photoTaken = true;
        SFXManager_JuanRdz.Play("TakingPhoto");

        if (takePhotoHint != null)
            takePhotoHint.gameObject.SetActive(false);

        videoPlayer.Pause();

        float currentTime = (float)videoPlayer.time;
        PhotoResultType result = EvaluateTiming(currentTime);

        if (resultText != null)
            resultText.text = GetResultText(result);

        if (QuestController_JuanRdz.Instance != null)
        {
            QuestController_JuanRdz.Instance.AddPhotoScore(result);
            QuestController_JuanRdz.Instance.AddPhoto();
        }

        if (result != PhotoResultType.None && currentSpot != null)
        {
            currentSpot.CompleteSpot();
        }

        StartCoroutine(ClosePhotoModeAfterDelay(2f));
    }

    private PhotoResultType EvaluateTiming(float currentTime)
    {
        if (currentSequence == null || currentSequence.timingWindows == null)
            return PhotoResultType.None;

        PhotoResultType bestResult = PhotoResultType.None;

        foreach (PhotoTimingWindow window in currentSequence.timingWindows)
        {
            if (currentTime >= window.startTime && currentTime <= window.endTime)
            {
                if ((int)window.resultType > (int)bestResult)
                    bestResult = window.resultType;
            }
        }

        return bestResult;
    }

    private string GetResultText(PhotoResultType result)
    {
        switch (result)
        {
            case PhotoResultType.Perfect:
                return "¡Foto perfecta!";
            case PhotoResultType.Good:
                return "¡Buena foto!";
            case PhotoResultType.Bad:
                return "Podría ser mejor";
            default:
                return "Fallaste";
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        videoFinished = true;

        if (takePhotoHint != null)
            takePhotoHint.gameObject.SetActive(false);

        if (!photoTaken)
        {
            if (resultText != null)
                resultText.text = "Fallaste";

            if (retryButton != null)
                retryButton.SetActive(true);
        }
    }

    public void RetryVideo()
    {
        if (currentSequence == null || currentSequence.videoClip == null || videoPlayer == null)
            return;

        photoTaken = false;
        videoFinished = false;

        if (retryButton != null)
            retryButton.SetActive(false);

        if (resultText != null)
            resultText.text = "";

        if (takePhotoHint != null)
            takePhotoHint.gameObject.SetActive(true);

        if (videoRawImage != null)
            videoRawImage.enabled = false;

        ClearRenderTexture();

        videoPlayer.Stop();
        videoPlayer.clip = currentSequence.videoClip;
        videoPlayer.time = 0;
        videoPlayer.Prepare();
        StartCoroutine(PlayPreparedVideo());
    }

    public void ClosePhotoGame()
    {
        if (videoPlayer != null)
            videoPlayer.Stop();

        if (photoGamePanel != null)
            photoGamePanel.SetActive(false);

        if (retryButton != null)
            retryButton.SetActive(false);

        if (gameUI != null)
            gameUI.SetActive(true);

        if (playerMove != null)
            playerMove.SetMovementEnabled(true);

        if (resultText != null)
            resultText.text = "";

        if (takePhotoHint != null)
            takePhotoHint.gameObject.SetActive(false);

        if (videoRawImage != null)
            videoRawImage.enabled = false;

        ClearRenderTexture();

        currentSpot = null;
        currentSequence = null;
        photoTaken = false;
        videoFinished = false;

        if (QuestController_JuanRdz.Instance != null &&
            QuestController_JuanRdz.Instance.currentPhotos >= QuestController_JuanRdz.Instance.targetPhotos)
        {
            if (ForestGameController_JuanRdz.Instance != null)
            {
                ForestGameController_JuanRdz.Instance.FinishGameAndGoToResults();
                return;
            }

            if (LakeGameController_JuanRdz.Instance != null)
            {
                LakeGameController_JuanRdz.Instance.FinishGameAndGoToResults();
                return;
            }

            SceneManager.LoadScene("Mini2_Resultados");
        }
    }

    public void TryTakePhoto()
    {
        if (!IsPhotoGameOpen())
            return;

        if (photoTaken || videoFinished)
            return;

        TakePhoto();
    }

    private IEnumerator PlayPreparedVideo()
    {
        while (!videoPlayer.isPrepared)
            yield return null;

        if (videoRawImage != null)
            videoRawImage.enabled = true;

        videoPlayer.Play();
    }

    private IEnumerator ClosePhotoModeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClosePhotoGame();
    }

    private void ClearRenderTexture()
    {
        if (renderTexture == null)
            return;

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = currentRT;
    }
}