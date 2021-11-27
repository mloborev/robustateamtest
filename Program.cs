using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TestTaskRobusta.Context;
using TestTaskRobusta.Models;

namespace TestTaskRobusta
{
    class Program
    {
        static async Task ParseFile()
        {
            using (StreamReader file = new StreamReader("affiliates_report__daily.csv"))
            {
                int counter = 0;
                string line;
                List<string> csvfile = new List<string>(30);

                while ((line = file.ReadLine()) != null)
                {
                    csvfile.Add(line);
                    counter++;
                }

                List<string> startLine = csvfile[0].Split(';').ToList();
                bool encounteredFirstTotalDeps = false;
                int clicksNum = 0;
                int regsNum = 0;
                int depsNum = 0;

                for (int i = 0; i < startLine.Count; i++)
                {
                    if(startLine[i].Contains("Clicks"))
                    {
                        clicksNum = i;
                    }
                    if (startLine[i].Contains("Regs"))
                    {
                        regsNum = i;
                    }
                    if (startLine[i].Contains("Total Deps") && encounteredFirstTotalDeps == true)
                    {
                        depsNum = i;
                    }
                    else
                    {
                        encounteredFirstTotalDeps = true;
                    }

                }

                List<string> totalLine = csvfile[--counter].Split(';').ToList();
                using (ApplicationContext db = new ApplicationContext())
                {
                    TotalForMonth test = new TotalForMonth
                    {
                        Clicks = Convert.ToInt32(totalLine[clicksNum]),
                        Regs = Convert.ToInt32(totalLine[regsNum]),
                        Deps = Convert.ToInt32(totalLine[depsNum]),
                        DateOfParse = DateTime.Now
                    };
                    db.TotalForMonths.Add(test);
                    await db.SaveChangesAsync();
                }
            }
            Console.WriteLine("Данные сохранены. Нажмите любую клавишу для продолжения.");
            Console.ReadKey();
        }

        static async Task SeeDataInTable()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var items = await db.TotalForMonths.ToListAsync();
                Console.WriteLine("Список объектов:");
                foreach (var item in items)
                {
                    Console.WriteLine($"Total clicks: {item.Clicks}, total deps: {item.Deps}, total regs: {item.Regs}, date of parse: {item.DateOfParse}");
                }
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения.");
            Console.ReadKey();
        }

        static async Task CleanTable()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var rows = await db.TotalForMonths.ToListAsync();
                foreach (var row in rows)
                {
                    db.TotalForMonths.Remove(row);
                }
                await db.SaveChangesAsync();
            }
            Console.WriteLine("Таблица очищена. Нажмите любую клавишу для продолжения.");
            Console.ReadKey();
        }

        static async Task Main(string[] args)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Убедитесь, что файл \"affiliates_report__daily.csv\" находится в той же папке, что и программа.\n" +
                    "Введите:\n" +
                    "\"parse\" - получить данные из файла\n" +
                    "\"see\" - увидеть данные в таблице\n" +
                    "\"clean\" - очистить базу данных.\n");
                string answer = Console.ReadLine();

                switch(answer)
                {
                    case "see":
                        await SeeDataInTable(); break;
                    case "clean":
                        await CleanTable(); break;
                    case "parse":
                        if (!File.Exists("affiliates_report__daily.csv"))
                        {
                            Console.WriteLine("Файл отсутствует. Нажмите любую клавишу для продолжения.");
                            Console.ReadKey();
                            break;
                        }
                        await ParseFile(); break;
                    default:
                        Environment.Exit(0); break;
                }
            }
        }
    }
}
