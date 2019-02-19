using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveData : MonoBehaviour
{
    [SerializeField]
    private DefaultScriptableObject defaultScriptableObject;
    private string savefilePath;
    private string stagesavefilePath;
    [SerializeField]
    private SaveScriptableObject2 saveScriptableObject;
    [SerializeField]
    private CreateStageData createStageData;
    private string savedataPath = "SaveData";
    private string stagedataPath = "CreateStage";
    private int stageID = 0;
    [SerializeField]
    private NodeData[] nodeDatas = new NodeData[4];
    [SerializeField]
    private GimmicData gimmicDatas;
    public class FromJsonSaveData
    {
        public List<NodeDataClass> statusNode;          //ステータスノードのデータ
        public List<NodeDataClass> straightNode;        //ストレートノードのデータ
        public List<NodeDataClass> flickDodgeNode;      //ステータスノードのデータ
        public List<NodeDataClass> snipeCannonNode;     //ステータスノードのデータ

        public int statusNodePoint;                     //ステータスノードのポイント
        public int statusNodeAmountPoint;               //ステータスノードの総ポイント
        public int straightNodePoint;                   //ストレートノードのポイント
        public int straightNodeAmountPoint;             //ストレートノードの総ポイント
        public int flickDodgeNodePoint;                 //フリックドッジノードのポイント
        public int flickDodgeNodeAmountPoint;           //フリックドッジノードの総ポイント
        public int snipeCannonNodePoint;                //スナイプカノンノードのポイント
        public int snipeCannonNodeAmountPoint;          //スナイプカノンノードの総ポイント

        public List<bool> gimmicPossession;             //ギミックの所持状態

        public int missionProgress;                     //オフラインミッションの進捗
        public string playerName;                       //プレイヤーの名前
        public int flagCredit;                          //通貨
    }

    public class FromJsonStageData
    {
        public int[] gimmicID = new int[100];      //配置されているギミックのID
        public int[] gimmicRotate = new int[100];  //配置されているギミックの角度
        public int totalCost = 0;                      //ステージの総コスト
        public string stageName;                       //ステージ名
        public string stageDetails;                    //ステージ詳細
        public bool isEdit = true;
    }

    void Awake()
    {
        saveScriptableObject = Resources.Load(savedataPath) as SaveScriptableObject2;
        savefilePath = UnityEngine.Application.persistentDataPath+@"/"+savedataPath+".json";
        Debug.Log(savefilePath);
        stagesavefilePath = UnityEngine.Application.persistentDataPath + @"/" + stagedataPath;
        if (DataCheck()) Load();
        else DataInitialization();
    }

    public void Save(int saveMode=-1)
    {
        FileStream m_file = FileOpen(FileMode.Open, FileAccess.ReadWrite);
        //更新されたのデータを書き込む
        if (saveMode == -1)
        {
            string m_json = JsonUtility.ToJson(saveScriptableObject);
            Debug.Log(m_json);
            File.WriteAllText(savefilePath, m_json);
            saveScriptableObject.isChanged = false;
        }
        else if(saveMode >= 0 || saveMode <= 2)
        {
            string m_json = JsonUtility.ToJson(createStageData);
            Debug.Log(m_json);
            File.WriteAllText(stagesavefilePath + saveMode +".json", m_json);
        }
        else
        {
            Debug.Log("不正な値です");
            return;
        }
        Debug.Log("セーブ成功");
    }

    public void Load(int saveMode = -1)
    {
        FileStream m_file = FileOpen(FileMode.Open, FileAccess.Read);
        if (saveMode == -1)
        {
            string m_json = File.ReadAllText(savefilePath);
            Debug.Log(m_json);
            FromJsonSaveData m_saveClass = JsonUtility.FromJson<FromJsonSaveData>(m_json);
            saveScriptableObject.UpdateScriptableObject(m_saveClass);
            ElementSet();
        }
        else if (saveMode >= 0 || saveMode <= 2)
        {
            string m_json = File.ReadAllText(stagesavefilePath + saveMode + ".json");
            Debug.Log(m_json);
            FromJsonStageData m_saveClass = JsonUtility.FromJson<FromJsonStageData>(m_json);
            createStageData.UpdateScriptableObject(m_saveClass);

        }
        Debug.Log("ロード成功");
    }

    /// <summary>
    /// データの初期化処理
    /// 新たな作成し、デフォルトのデータを書き込む。
    /// ※データの上書きを行うので確認を必ず行う事※
    /// </summary>
    public void DataInitialization()
    {
        FileStream m_file = FileOpen(FileMode.CreateNew,FileAccess.ReadWrite);
        if (m_file == null) return;
        //デフォルトのデータを書き込む
        string m_json = JsonUtility.ToJson(defaultScriptableObject);
        Debug.Log(m_json);
        File.WriteAllText(savefilePath, m_json);
        saveScriptableObject.DefaultLoadData(defaultScriptableObject);
        ElementSet();
        Scene m_nowScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(m_nowScene.name);
        Debug.Log("作成成功");
    }

    /// <summary>
    /// セーブデータの有無の確認を行う
    /// </summary>
    /// <returns>セーブデータがある場合trueを返す</returns>
    public bool DataCheck()
    {
        if(System.IO.File.Exists(savefilePath))
            return true;
        else
            return false;
    }

    /// <summary>
    /// セーブデータの消去を行う
    /// 無慈悲に行くのでホント注意
    /// </summary>
    public void DeleteSaveData()
    {
        File.Delete(savefilePath);
        Debug.LogWarning("削除処理完了");
        DataInitialization();
    }

    /// <summary>
    /// ファイルを読み込む際の処理からリソースの開放までを行う
    /// 例外処理が発生した場合エラーを返す
    /// </summary>
    /// <param name="filemode">ファイルの取得モード</param>
    /// <param name="fileAccess">ファイルのアクセスモード</param>
    /// <returns>読み込んだファイルのFileStreamを返す</returns>
    FileStream FileOpen(FileMode filemode,FileAccess fileAccess)
    {
        FileStream m_file = null;
        try
        {
            //ファイルを取得、存在しなければエラーを返す
            m_file = File.Open(savefilePath, filemode, fileAccess);
        }
        catch (IOException m_error)
        {
            Debug.LogWarning(m_error.Message);
        }
        finally
        {
            if (m_file != null)
            {
                //最後にリソースを開放する
                try
                {
                    m_file.Dispose();
                }
                catch (IOException m_error2)
                {
                    Debug.LogWarning(m_error2.Message);
                }
            }
        }
        return m_file;
    }

    /// <summary>
    /// 要素数の更新
    /// </summary>
    void ElementSet()
    {
        if (saveScriptableObject.statusNode.Count - 1 < nodeDatas[0].nodelist.Length)
            for (int n = saveScriptableObject.statusNode.Count; saveScriptableObject.statusNode.Count < nodeDatas[0].nodelist.Length; n++)
            {
                saveScriptableObject.statusNode.Add(new NodeDataClass());
            }
        if (saveScriptableObject.straightNode.Count - 1 < nodeDatas[1].nodelist.Length)
            for (int n = saveScriptableObject.straightNode.Count; saveScriptableObject.straightNode.Count < nodeDatas[1].nodelist.Length; n++)
            {
                saveScriptableObject.straightNode.Add(new NodeDataClass());
            }
        if (saveScriptableObject.flickDodgeNode.Count - 1 < nodeDatas[2].nodelist.Length)
            for (int n = saveScriptableObject.flickDodgeNode.Count; saveScriptableObject.flickDodgeNode.Count < nodeDatas[2].nodelist.Length; n++)
            {
                saveScriptableObject.flickDodgeNode.Add(new NodeDataClass());
            }
        if (saveScriptableObject.snipeCannonNode.Count - 1 < nodeDatas[3].nodelist.Length)
            for (int n = saveScriptableObject.snipeCannonNode.Count; saveScriptableObject.snipeCannonNode.Count < nodeDatas[3].nodelist.Length; n++)
            {
                saveScriptableObject.snipeCannonNode.Add(new NodeDataClass());
            }
        if (saveScriptableObject.gimmicPossession.Count - 1 < gimmicDatas.gimmicList.Length)
            for (int n = saveScriptableObject.gimmicPossession.Count; saveScriptableObject.gimmicPossession.Count < gimmicDatas.gimmicList.Length; n++)
            {
                saveScriptableObject.gimmicPossession.Add(new bool());
            }
    }
}
