using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class SaveLoadManager : MonoBehaviour
{
    [System.Serializable]
    public class SaveData
    {
        public Vector3 playerPosition;
        public float timer;
        public int currentAmmoClip;
        public int ammoInReserve;
    }

    private const string SaveFileName = "Assets/Saves/SaveData.xml";

    private GunController gunController; // Reference to the GunController script

    private void Start()
    {
        gunController = FindObjectOfType<GunController>(); // Find the GunController script in the scene
    }

    void Update()
    {
        // Save game when F1 is pressed
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Save();
        }

        // Load game when F2 is pressed
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Load();
        }
    }

    private void Save()
    {
        SaveData data = new SaveData();
        data.playerPosition = transform.position;
        data.timer = Timer.Instance.GetCurrentTime(); // Update this line if you have a Timer class
        data.currentAmmoClip = gunController._currentAmmoClip;
        data.ammoInReserve = gunController._ammoInReserve;

        XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
        string directoryPath = Path.GetDirectoryName(SaveFileName);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using (FileStream stream = new FileStream(SaveFileName, FileMode.Create))
        {
            serializer.Serialize(stream, data);
        }

        Debug.Log("Game saved.");
    }

    private void Load()
    {
        if (File.Exists(SaveFileName))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            using (FileStream stream = new FileStream(SaveFileName, FileMode.Open))
            {
                SaveData data = (SaveData)serializer.Deserialize(stream);
                transform.position = data.playerPosition;
                // Update Timer.Instance.SetCurrentTime(data.timer); if you have a Timer class
                gunController._currentAmmoClip = data.currentAmmoClip;
                gunController._ammoInReserve = data.ammoInReserve;
                gunController.UpdateGunUI(); // Update the UI after loading ammo data
            }

            Debug.Log("Game loaded.");
        }
        else
        {
            Debug.LogWarning("No saved data found.");
        }
    }
}
