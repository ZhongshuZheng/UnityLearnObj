using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
// using UnityEditor;
// using UnityEditor.Build.Content;
// using UnityEditorInternal;
using UnityEngine;

public class SaveManager: Singleton<SaveManager>
{

    private GameSaveData _gameSaveData;

    string save_path;

    // Start is called before the first frame update
    void Start()
    {
        save_path = Application.persistentDataPath + "/gamesave/";
        _gameSaveData = new GameSaveData();
    }

    public void register(ISaveable saveable) {
        _gameSaveData.save_obj_list.Add(saveable);
    }

    public void LoadAll() {
        try {
            LoadFromFile(save_path + "data.sav"); 
        } catch {
            print("load save data failed");
        }

        if (_gameSaveData.save_data_dict.Count == 0) {
            return;
        }
        foreach (ISaveable obj in _gameSaveData.save_obj_list) {
            obj.Load(_gameSaveData.save_data_dict[obj.GetType().Name]);
        }
        _gameSaveData.save_data_dict.Clear();
        print("load all");
    }

    public void SaveAll() {
        foreach (ISaveable obj in _gameSaveData.save_obj_list) {
            _gameSaveData.save_data_dict.Add(obj.GetType().Name, obj.Save());
        }
        
        foreach (string obj in _gameSaveData.save_data_dict.Keys) {
            print(obj + ":" + _gameSaveData.save_data_dict[obj]);
        }
        SaveToFile(save_path + "data.sav", JsonConvert.SerializeObject(_gameSaveData.save_data_dict));

        _gameSaveData.save_data_dict.Clear();
    }

    public void SaveToFile(string filename, string data) {
        print(filename);
        if (File.Exists(filename)) {
            File.Delete(filename);
        } else {
            Directory.CreateDirectory(save_path);
        }

        using (var stream = File.Open(filename, FileMode.Create)) {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false)) {
                writer.Write(AESUtils.Encrypt(data));
            }
        }
    }

    public void LoadFromFile(string filename) {
        if (!File.Exists(filename)) {
            return;
        }

        string save_data;
        using (var stream = File.Open(filename, FileMode.Open)) {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, false)) {
                save_data = reader.ReadString();
            }
        }
        save_data = AESUtils.Decrypt(save_data);
        _gameSaveData.save_data_dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(save_data);
    }

}

