using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    public void Save(string saveFile)
    {
        string path = GetPathFromSaveFile(saveFile);
        print("Save to " + path);

        using (FileStream stream = File.Open(path,FileMode.Create))
        {
            Transform playerpos = GameWorld.player.transform;
            byte[] buffer = SerializeVector(playerpos.position);
            stream.Write(buffer,0,buffer.Length);
        }
        
    }
    public void Load(string saveFile)
    {
        string path = GetPathFromSaveFile(saveFile);
         using (FileStream stream = File.Open(path,FileMode.Open))
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer,0,buffer.Length);
            Transform playerpos = GameWorld.player.transform;
            playerpos.position = DeserializeVector(buffer);
        }
        print("Load From " + GetPathFromSaveFile(saveFile));

    }

    public string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath,saveFile + ".sav");
    }

    private Transform GetPlayerPosition()
    {
        return GameWorld.player.transform;
    }

    private byte[] SerializeVector(Vector3 vector)
    {
        byte[] vectroBytes = new byte[3*4];
        BitConverter.GetBytes(vector.x).CopyTo(vectroBytes,0);
        BitConverter.GetBytes(vector.y).CopyTo(vectroBytes,4);
        BitConverter.GetBytes(vector.z).CopyTo(vectroBytes,8);
        return vectroBytes;
    }
    private Vector3 DeserializeVector(byte[] buffer)
    {
        Vector3 result = new Vector3();
        result.x = BitConverter.ToSingle(buffer,0);
        result.y = BitConverter.ToSingle(buffer,4);
        result.z = BitConverter.ToSingle(buffer,8);
        return result;
    }
}
