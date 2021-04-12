using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GlobalSave : MonoBehaviour
{
    public static GlobalSave Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        savingData = new List<Data>();
        LoadFileWithAllSaves();
    }
    

    public class Data
    {
        public ResourcesSavingData saveData = null;    
        public ShtabSavingData shtabSavingData = null;
        public List<PowerPlantSavingData> powerPlantsSaved = new List<PowerPlantSavingData>();
        public List<GarageSavingData> garagesSaved = new List<GarageSavingData>();
        public List<MineShaftSavingData> mineShaftsSaved = new List<MineShaftSavingData>();
        public List<ShieldGeneratorSavingData> shieldGeneratorsSaved = new List<ShieldGeneratorSavingData>();
        public List<TurretSavingData> turretsSaved = new List<TurretSavingData>();
        public AntenneSavingData antenneSavingData = null;
        public AntenneLogicSavingData antenneLogicSavingData = null;
        public List<UnitSavingData> unitsSaved = new List<UnitSavingData>();

        public EnemySpawnerSavingData spawnerSavingData = new EnemySpawnerSavingData();
        public List<EnemyBomberSavingData> bombersSaved = new List<EnemyBomberSavingData>();
    }

    public List<Data> savingData;
    Data data;
    string path = "C:\\Users\\BigBoss\\Desktop\\backup\\AllSaves.json";

    public void LoadFileWithAllSaves()
    {
        using (StreamReader streamReader = new StreamReader(path))
        {
            string globalData = streamReader.ReadToEnd();
            savingData = JsonConvert.DeserializeObject<List<Data>>(globalData);
        }

        if (savingData != null)
        {
            Debug.Log(savingData.Count);
        }
        else
        {
            Debug.Log("File is empty!");
        }
    }
















    public void InitDataFromFileWithIndex(int index)
    {
        GameHendler.Instance.saveData = savingData[index].saveData;    
        GameHendler.Instance.shtabSavingData = savingData[index].shtabSavingData;  
        GameHendler.Instance.powerPlantsSaved = savingData[index].powerPlantsSaved;  
        GameHendler.Instance.garagesSaved = savingData[index].garagesSaved;  
        GameHendler.Instance.mineShaftsSaved = savingData[index].mineShaftsSaved;  
        GameHendler.Instance.shieldGeneratorsSaved = savingData[index].shieldGeneratorsSaved;  
        GameHendler.Instance.turretsSaved = savingData[index].turretsSaved;  
        GameHendler.Instance.antenneSavingData = savingData[index].antenneSavingData;  
        GameHendler.Instance.antenneLogicSavingData = savingData[index].antenneLogicSavingData;  
        GameHendler.Instance.unitsSaved = savingData[index].unitsSaved;  
        GameHendler.Instance.spawnerSavingData = savingData[index].spawnerSavingData;  
        GameHendler.Instance.bombersSaved = savingData[index].bombersSaved;  
    }





    public void SaveCurrentInfoAbaoutEveruthingToList()
    {
        // Init GameHendler (Particular scene data)
        GameHendler.Instance.SaveCurrentSceneData();


        // Create template for data
        data = new Data();


        // Save all data
        data.saveData = GameHendler.Instance.saveData;    
        data.shtabSavingData = GameHendler.Instance.shtabSavingData;
        data.powerPlantsSaved = GameHendler.Instance.powerPlantsSaved;
        data.garagesSaved = GameHendler.Instance.garagesSaved;
        data.mineShaftsSaved = GameHendler.Instance.mineShaftsSaved;
        data.shieldGeneratorsSaved = GameHendler.Instance.shieldGeneratorsSaved;
        data.turretsSaved = GameHendler.Instance.turretsSaved;
        data.antenneSavingData = GameHendler.Instance.antenneSavingData;
        data.antenneLogicSavingData = GameHendler.Instance.antenneLogicSavingData;
        data.unitsSaved = GameHendler.Instance.unitsSaved;
        data.spawnerSavingData = GameHendler.Instance.spawnerSavingData;
        data.bombersSaved = GameHendler.Instance.bombersSaved;


        // Add data to list
        savingData.Add(data);


        // Resave list with new info
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            string globalData = JsonConvert.SerializeObject(savingData, Formatting.Indented);
            
            streamWriter.Write(globalData);
        }
    }





    public void LoadParticularSaveWithIndexFromFile(int index)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        StartCoroutine(InitAllDataInNewScene(index));
    }

    IEnumerator InitAllDataInNewScene(int index)
    {
        yield return null;

        InitDataFromFileWithIndex(index);

        GameHendler.Instance.LoadGameWithPreviouslyInitializedData();

        Time.timeScale = 1f;

        AstarPath.active.Scan();
    }
}