using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_Loading : MonoBehaviour
{
    #region 싱글톤
    private static GameManager_Loading instance;
    public static GameManager_Loading Instance //어디서든 부를 수 있게 싱글톤 패턴
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<GameManager_Loading>();
                if(obj != null)
                    instance = obj;
                else
                    instance = Create();
            }
            return instance;
        }
    }
    private static GameManager_Loading Create() 
        => Instantiate(Resources.Load<GameManager_Loading>("Canvas_LoadingManager")); //Resources경로에서 프리팹 찾기
    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image progressBar;
    private string loadSceneName;

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded; //델리게이트 체인 추가
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());//비동기로 씬 불러오기 시작
    }
    private IEnumerator LoadSceneProcess()//비동기로 씬 불러오기
    {
        progressBar.fillAmount = 0f; //진행바 초기화
        yield return StartCoroutine(Fade(true)); //함수 실행, 끝나면 다음줄 실행

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);//비동기로 씬 부르기 //호출과 동시에 델리게이트체인 실행
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)//비동기가 완료가 아닐때
        {
            yield return null;
            if (op.progress < 0.9f)//현재 진행상태
                progressBar.fillAmount = op.progress;
            else
            {
                timer += Time.unscaledDeltaTime;//타임스케일에 영향을 받지 않음
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) //씬로드 후 마지막 진행
    {
        if(arg0.name == loadSceneName) //로드된 씬 이름이 같은지 비교
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;//콜백 제거 //델리게이트 체인 제거
            Time.timeScale = 1;
        }
    }

    private IEnumerator Fade(bool isFadeIn) //코루틴 완벽 이해하기
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null; //왜 갑자기 널?
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if(!isFadeIn)
            gameObject.SetActive(false);
    }
}
