using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData {
	public string name;
	public string ip;
}


public class SaveManager : MonoBehaviour {

	public static SaveManager S;
	[HideInInspector] public SaveData saveData;

	void Awake() {
		if (S != null)
			return;
		S = this;
		Load();
	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(GetPath());
		bf.Serialize(file, saveData);
		file.Close();
	}

	void Load() {
		if (File.Exists(GetPath())) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(GetPath(), FileMode.Open);
			try {
				saveData = (SaveData)bf.Deserialize(file);
			}
			catch {
				Debug.LogError("Load data read error!");
			}
			file.Close();
		}
	}

	string GetPath() {
		return Application.persistentDataPath + "/saveData.gd";
	}
}
