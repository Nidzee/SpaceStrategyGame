using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    private static SaveData _current;
    public static SaveData current
    {
        get 
        {
            if (_current == null)
            {
                _current = new SaveData();
            }

            return _current;
        }

        set
        {

        }
    }

    public PlayerProfile playerProfile;
    public int x = 0;
    public int y = 0;

    public void LoadGame()
    {
        SaveData.current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/Save.save");

        Debug.Log(SaveData.current.playerProfile.playerName);
        Debug.Log(SaveData.current.playerProfile.money);
        Debug.Log(SaveData.current.x);
        Debug.Log(SaveData.current.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SerializationManager.Save("Save", current);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveData.current.playerProfile.money += 10;
            SaveData.current.x += 10;
            SaveData.current.y += 10;
            SaveData.current.playerProfile.playerName = "Player";
        }
    }

}
