using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_Loading : MonoBehaviour
{
    #region �̱���
    private static GameManager_Loading instance;
    public static GameManager_Loading Instance //��𼭵� �θ� �� �ְ� �̱��� ����
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
        => Instantiate(Resources.Load<GameManager_Loading>("Canvas_LoadingManager")); //Resources��ο��� ������ ã��
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
        SceneManager.sceneLoaded += OnSceneLoaded; //��������Ʈ ü�� �߰�
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());//�񵿱�� �� �ҷ����� ����
    }
    private IEnumerator LoadSceneProcess()//�񵿱�� �� �ҷ�����
    {
        progressBar.fillAmount = 0f; //����� �ʱ�ȭ
        yield return StartCoroutine(Fade(true)); //�Լ� ����, ������ ������ ����

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);//�񵿱�� �� �θ��� //ȣ��� ���ÿ� ��������Ʈü�� ����
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)//�񵿱Ⱑ �Ϸᰡ �ƴҶ�
        {
            yield return null;
            if (op.progress < 0.9f)//���� �������
                progressBar.fillAmount = op.progress;
            else
            {
                timer += Time.unscaledDeltaTime;//Ÿ�ӽ����Ͽ� ������ ���� ����
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) //���ε� �� ������ ����
    {
        if(arg0.name == loadSceneName) //�ε�� �� �̸��� ������ ��
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;//�ݹ� ���� //��������Ʈ ü�� ����
            Time.timeScale = 1;
        }
    }

    private IEnumerator Fade(bool isFadeIn) //�ڷ�ƾ �Ϻ� �����ϱ�
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null; //�� ���ڱ� ��?
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if(!isFadeIn)
            gameObject.SetActive(false);
    }
}
