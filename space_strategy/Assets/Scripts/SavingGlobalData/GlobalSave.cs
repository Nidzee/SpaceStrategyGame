using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;


public class GlobalSave : MonoBehaviour
{
    public static GlobalSave Instance;
    public void Awake()
    {
        Debug.Log("Initializing save system...");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadFileWithAllSaves();
    }
    
    // Data clas which needs to be saved
    public class Data
    {
        public int levelNumber;
        public string slotDescription;

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


    Data data;
    public List<Data> savingData = new List<Data>();
    // string path = "C:\\Users\\BigBoss\\Desktop\\backup\\AllSaves.json";





    // Load all saves from file to cash
    public void LoadFileWithAllSaves()
    {    
        string jsonFileName = "AllSaves.json";
        string path = Path.Combine(Application.persistentDataPath, jsonFileName);

        using (StreamReader streamReader = new StreamReader(path))
        {
            string globalData = streamReader.ReadToEnd();
            savingData = JsonConvert.DeserializeObject<List<Data>>(globalData);
        }

        if (savingData != null)
        {
            Debug.Log("I found:" + savingData.Count + " game SLOTS!");
        }
        else
        {
            Debug.Log("File is empty!");
        }
    }





    // Gets data from file into variables of "GAME HENDLER" manager
    // File -> Game hendler
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
        GameHendler.Instance.particularLevelNumber = savingData[index].levelNumber; 
    }






    // Save current data (scene data) into file "AllSaves.json"
    public void SaveCurrentInfoAbaoutEveruthingToList()
    {
        // Init particular scene data into GAME HENDLER variables
        GameHendler.Instance.SaveCurrentSceneData();



        // Create template for data
        data = new Data();



        // Save all data
        data.levelNumber = GameHendler.Instance.particularLevelNumber;
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
        data.slotDescription = System.DateTime.Now.ToString();



        // Add data to list
        if (savingData == null)
        {
            savingData = new List<Data>();
        }
        savingData.Add(data);



        // Save all info into file
        // Resaving same file with new string of data
        string jsonFileName = "AllSaves.json";
        string path = Path.Combine(Application.persistentDataPath, jsonFileName);
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            string globalData = JsonConvert.SerializeObject(savingData, Formatting.Indented);
            
            streamWriter.Write(globalData);
        }
    }

    // Resave particular SLOT with new data
    public void ReSaveCurrentSave(int saveSlot)
    {
        // Init GameHendler (Particular scene data)
        GameHendler.Instance.SaveCurrentSceneData();


        // Create template for data
        data = new Data();


        // Save all data
        data.levelNumber = GameHendler.Instance.particularLevelNumber;
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
        data.slotDescription = System.DateTime.Now.ToString();



        // Replacing previous SLOT with new data
        savingData[saveSlot] = data;



        // Resave list with new info
        string jsonFileName = "AllSaves.json";
        string path = Path.Combine(Application.persistentDataPath, jsonFileName);
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            string globalData = JsonConvert.SerializeObject(savingData, Formatting.Indented);
            
            streamWriter.Write(globalData);
        }
    }

    // Delete particular slot
    public void DeleteParticularSaveSlot(int slotIndex)
    {
        // Removing current slot from file
        savingData.RemoveAt(slotIndex);



        // Resave list without current slot
        string jsonFileName = "AllSaves.json";
        string path = Path.Combine(Application.persistentDataPath, jsonFileName);
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            string globalData = JsonConvert.SerializeObject(savingData, Formatting.Indented);
            
            streamWriter.Write(globalData);
        }
    }

    // Delete all slots
    public void DeleteAllSlots()
    {
        savingData.Clear();
        


        // Resave list without current slot
        string jsonFileName = "AllSaves.json";
        string path = Path.Combine(Application.persistentDataPath, jsonFileName);
        using (StreamWriter streamWriter = new StreamWriter(path, false))
        {
            string globalData = JsonConvert.SerializeObject(savingData, Formatting.Indented);
            streamWriter.Write(string.Empty);
        }
    }
}