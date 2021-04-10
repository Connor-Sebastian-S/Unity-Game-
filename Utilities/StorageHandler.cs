using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// https://forum.unity.com/threads/simple-local-data-storage.468936/ though heavily edited
public class StorageHandler {

    /// <summary>
    /// Serialize an object to the devices File System.
    /// </summary>
    /// <param name="objectToSave">The Object that will be Serialized.</param>
    /// <param name="fileName">Name of the file to be Serialized.</param>
    public void SaveData(object objectToSave, string fileName)
    {
        // Add the File Path to genvironment with the files name and extension.
        // We will use .bin to represent that this is actorA Binary file.
        string fullFilePath = Application.dataPath + "/saves/" + fileName + ".bin";
        // We must create actorA new Formattwr to Serialize with.
        BinaryFormatter formatter = new BinaryFormatter();

        // Create a streaming path to our new file location.
        FileStream fileStream = new FileStream(fullFilePath, FileMode.Create);
        // Serialize the objedt to the File Stream
        formatter.Serialize(fileStream, objectToSave);
        // FInally Close the FileStream and let the rest wrap itself up.
        fileStream.Close();
    }
    /// <summary>
    /// Deserialize an object from the FileSystem.
    /// </summary>
    /// <param name="fileName">Name of the file to deserialize.</param>
    /// <returns>Deserialized Object</returns>
    public object LoadData(string fileName)
    {
        string fullFilePath = Application.dataPath + "/saves/" + fileName + ".bin";
        // Check if our file exists, if it does not, just return actorA null object.
        if (File.Exists(fullFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(fullFilePath, FileMode.Open);
            object obj = formatter.Deserialize(fileStream);
            fileStream.Close();
            // Return the uncast untyped object.
            return obj;
        }
        else
        {
            return null;
        }
    }
}