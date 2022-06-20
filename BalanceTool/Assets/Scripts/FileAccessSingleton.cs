using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileAccessSingleton
{
    public static void WriteFile(string _jsonString, string _filePath)
	{
		if (!System.IO.File.Exists(_filePath))
		{
			GameManager.instance.ChangeInfoText("File " + _filePath + " doesn't exists !");
		}

		StreamWriter writer = new StreamWriter(_filePath, false);
		writer.WriteLine(_jsonString);
		writer.Close();

		//Debug.Log(_jsonString);
	}

	public static string LoadFile(string _filePath)
	{
		if (System.IO.File.Exists(_filePath))
		{
			StreamReader reader = new StreamReader(_filePath, true);
			string toReturn = reader.ReadToEnd();
			reader.Close();

			//Debug.Log(toReturn);

			return toReturn;
		}
		else
		{
			GameManager.instance.ChangeInfoText("File " + _filePath + " doesn't exists !");

			return "";
		}
		
	}
}
