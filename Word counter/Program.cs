using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;


while (true)
{
    Console.WriteLine("Укажите путь к файлу:");
    try
    {
        string pathToRead = Console.ReadLine();

        //Условие выхода из консольного приложения.
        if (pathToRead == "quit")
        {
           break;
        }

        string allText = File.ReadAllText(pathToRead);

        List<string> words = new List<string>();

        words = Split(allText);
        words.Sort();

        Dictionary<string, int> wordsWithCount = new Dictionary<string, int>();
        
        wordsWithCount = SortedWords(words);

        while (true)
        {
            try
            {
                Console.WriteLine("Укажите путь, куда сохранить отсортированный список:");
                string pathToWrite = Console.ReadLine();

                foreach (var word in wordsWithCount)
                {
                    File.AppendAllText(pathToWrite, $"{word.Key}\t\t{word.Value}\n");
                }

                Console.WriteLine("Успешно!\n");
                break;
            }
            catch (Exception ex) when (ex is IOException || ex is ArgumentException || ex is DirectoryNotFoundException)
            {

                Console.WriteLine("Указан неверный путь. \n");
            }
        }
    }

    catch (FileNotFoundException)
    {
        Console.WriteLine("Указанного файла не существует. \n");
    }

    catch (Exception ex) when (ex  is IOException || ex is ArgumentException || ex is DirectoryNotFoundException || ex is UnauthorizedAccessException)
    {
        Console.WriteLine("Указан неверный путь. \n");
    }
}

///Этот метод принимает список всех слов, подсчитывает кол-во уникальных, записывает их в словарь. Потом этот словарь сортируется по убыванию уникальных слов и 
///возвращает итоговый список с отсортированными словами.
Dictionary<string, int> SortedWords(List<string> allWords)
{
    Dictionary<string, int> result = new Dictionary<string, int>();

    for(int i=0; i<allWords.Count; i++)
    {
        string word = allWords[i];
        int count = 1;

        if(i==allWords.Count-1)
        {
            result.Add(word, count);
            break;
        }
        for(int j=i+1; j<allWords.Count; j++)
        {

            if(word==allWords[j])
            {
                count++;
                i = j;
                continue;
            }
            break;
        }
        result.Add(word, count);
    }
    result = result.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
    return result;
}

///Данный метод принемает исходный текст, делит его на слова и переводит все слова в нижний регистр, чтобы упростить подсчет уникальных слов и возврощает массив всех слов в тексте.
List<string> Split (string text)
{
    List<string> words = new List<string>();
    string temp = "";

    for(int i=0;i<text.Length;i++)
    {
        int cor = (int)text[i];

        //Этим условием идет разделение текста на отдельные слова.
        if (cor == 10 || cor == 13 || (cor >= 32 && cor <= 44) || cor == 46 || cor == 47 || (cor >= 58 && cor <= 64) || (cor >= 91 && cor <= 95) || (cor >= 123 && cor <= 126) )
        {

            if (temp != "")
            {
                words.Add(temp.ToLower());
                temp = "";
            }
            continue;
        }
        temp = temp + text[i];
    }
    return words;
}