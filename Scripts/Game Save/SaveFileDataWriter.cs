using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveFileDataWriter 
{
    public string saveDataDirectoryPath = "";
    public string saveFileName="";

    public bool CheckToSeeIfFileExists()
    {
        if(File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));

    }


    //Used To Create A SAVE FILE
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        //Save Path
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Creating Save File, At: " + savePath);

            //Serialize Game data object int JSON
            string dataToStore = JsonUtility.ToJson(characterData, true);

            //Write That File To Our System
            using(FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError("GAME NOT SAVED, Error Whilst TRYING TO SAVE CHARACTER DATA" + savePath +" \n" + ex);
        }

    }

    //Used To LOAD A SAVE FILE

    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        //Load Path
        
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if(File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream steram = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(steram))
                    {
                        dataToLoad = reader.ReadToEnd();

                    }
                }
                //DeSerialize the SAVE DATA From JASON to UNITY C# 
                characterData= JsonUtility.FromJson<CharacterSaveData>(dataToLoad);

            }
            catch(Exception ex)
            {
                Debug.LogError("Failed To LOAD SAVE" + loadPath +" \n" + ex);
            }
            
        }
        return characterData;

    }
}
