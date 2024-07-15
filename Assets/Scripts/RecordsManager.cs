using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RecordsManager : MonoBehaviour
{
    public GameObject listItemPrefab;
    public Transform content;
    public static string recordsFilePath = "records.txt";

    public static void SaveRecords(List<int> records)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(recordsFilePath))
            {
                foreach (int record in records)
                    writer.WriteLine(record);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing file: {ex.Message}");
        }
    }

    public static List<int> LoadRecords()
    {
        List<int> records = new List<int>();

        try
        {
            using (StreamReader reader = new StreamReader(recordsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (int.TryParse(line, out int record))
                        records.Add(record);
                    else
                        Console.WriteLine($"Invalid value in line: {line}");
                }
            }
        }
        catch (FileNotFoundException) { }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }

        return records;
    }

    void Start()
    {
        FillList(LoadRecords());
    }

    void FillList(List<int> records)
    {
        int number = 1;
        foreach (int record in records)
        {
            GameObject newItem = Instantiate(listItemPrefab, content);
            TextMeshProUGUI[] itemTextFields =
                newItem.GetComponentsInChildren<TextMeshProUGUI>();
            itemTextFields[0].text = number.ToString();
            itemTextFields[1].text = record.ToString();
            number++;
        }
    }
    public void ClearList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnClickButtonClear()
    {
        SaveRecords(null);
        ClearList();
    }

    public void OnClickButtonBack()
    {
        SceneManager.LoadScene("Menu");
    }
}
