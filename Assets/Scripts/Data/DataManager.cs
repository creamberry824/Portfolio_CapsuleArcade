using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static GameObject container;

    //싱글톤
    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    //게임 데이터 파일이름 설정
    string GameDataFileName = "CapsuleArcade.json";

    //저장용 클래스 변수
    public Data data = new Data();

    //데이터가 있다면 로드
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        //저장된 게임이 있다면
        if (File.Exists(filePath))
        {
            //저장된 파일 읽어오고 Json을 클래스 형식으로 전환해서 할당
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            Debug.Log("불러오기 완료");
        }
    }

    //데이터 저장
    public void SaveGameData()
    {
        //클래스를 Json 형식으로 전환 (true : 가독성 좋게 작성)
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        //이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장
        File.WriteAllText(filePath, ToJsonData);

        //올바르게 저장됐는지 확인
        Debug.Log("저장 완료");
    }
}