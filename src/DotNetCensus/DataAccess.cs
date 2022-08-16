﻿using ConsoleTables;
using DotNetCensus.Core;
using DotNetCensus.Core.APIs;
using DotNetCensus.Core.Models;
using System.Diagnostics.Metrics;

namespace DotNetCensus;

public static class DataAccess
{
    private static List<Project> GetProjects(string? directory, Repo? repo)
    {
        List<Project> projects = new();
        List<Project> sortedProjects = new();
        if (string.IsNullOrEmpty(directory) == false)
        {
            //Run the calculations to get and aggregate the results
            projects = ProjectScanning.SearchDirectory(directory);
        }
        else if (repo != null)
        {
            string? owner = repo.Owner;
            string? repository = repo.Repository;
            string? clientId = repo.User;
            string? clientSecret = repo.Password;
            projects = Task.Run(async () => await GitHubAPI.GetRepoFiles(clientId, clientSecret, owner, repository, "main")).Result;
        }
        //Need to sort so that Linux + Windows results are the same
        if (projects != null)
        {
            sortedProjects = projects.OrderBy(o => o.Path).ToList();
        }
        return sortedProjects;
    }

    public static string? GetInventoryResults(string? directory, Repo? repo, string? file)
    {
        List<Project> projects = GetProjects(directory, repo);

        //If it's inventory output, remove the full path from each project
        if (directory != null)
        {
            foreach (Project item in projects)
            {
                item.Path = item.Path.Replace(directory, "");
            }
        }

        if (string.IsNullOrEmpty(file) == true)
        {
            ConsoleTable table = new("Path", "FileName", "FrameworkCode", "FrameworkName", "Family", "Language", "Status");
            foreach (Project item in projects)
            {
                table.AddRow(item.Path, item.FileName, item.FrameworkCode, item.FrameworkName, item.Family, item.Language, item.Status);
            }
            string result = table.ToMinimalString();
            Console.WriteLine(result);
            return result;
        }
        else
        {
            //Create a CSV file
            StreamWriter sw = File.CreateText(file);
            sw.WriteLine("Path,FileName,FrameworkCode,FrameworkName,Family,Language,Status");
            foreach (Project item in projects)
            {
                sw.WriteLine(item.Path + "," +
                    item.FileName + "," +
                    item.FrameworkCode + "," +
                    item.FrameworkName + "," +
                    item.Family + "," +
                    item.Language + "," +
                    item.Status);
            }
            string? result = sw?.ToString();
            sw?.Close();

            //FileInfo fileInfo = new(_file);
            Console.WriteLine($"Exported results to '{file}'");
            return result;
        }
    }

    public static string? GetFrameworkSummary(string? directory, Repo? repo, bool includeTotals, string? file)
    {
        List<Project> projects = GetProjects(directory, repo);
        List<FrameworkSummary> frameworks = Census.AggregateFrameworks(projects, includeTotals);

        if (string.IsNullOrEmpty(file) == true)
        {
            //Create and output the table
            ConsoleTable table = new("Framework", "FrameworkFamily", "Count", "Status");
            foreach (FrameworkSummary item in frameworks)
            {
                table.AddRow(item.Framework, item.FrameworkFamily, item.Count, item.Status);
            }
            string result = table.ToMinimalString();
            Console.WriteLine(result);
            return result;
        }
        else
        {
            //Create a CSV file
            StreamWriter sw = File.CreateText(file);
            sw.WriteLine("Framework,FrameworkFamily,Count,Status");
            foreach (FrameworkSummary item in frameworks)
            {
                sw.WriteLine(item.Framework + "," +
                    item.FrameworkFamily + "," +
                    item.Count + "," +
                    item.Status);
            }
            string? result = sw?.ToString();
            sw?.Close();

            //FileInfo fileInfo = new(_file);
            Console.WriteLine($"Exported results to '{file}'");
            return result;
        }
    }
}
