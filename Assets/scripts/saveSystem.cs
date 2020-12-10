using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class saveSystem
{
    public string path;


    public void setPath(string path_par) {
        path = path_par;
    }

    public void savePlayer(player player_scr) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        playerData data = new playerData(player_scr);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public bool checkSaveFile() {
        if (File.Exists(path)) {
            return true;
        } else {
            return false;
        }
    }

    public playerData loadPlayer() {
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            playerData data = formatter.Deserialize(stream) as playerData;
            stream.Close();
            return data;
        } else {
            Debug.LogError("save file not found in " + path);
            return null;
        }
    }

    public void deleteSaveFile() {
        if (File.Exists(path)) {
            File.Delete(path);
        }
    }
}
