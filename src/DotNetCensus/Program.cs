﻿using CommandLine;
using DotNetCensus.Core.Models;

namespace DotNetCensus;

public class Program
{
    private static string? _directory;
    private static Target? _repo;
    private static bool _includeTotals;
    private static bool _includeInventory;
    private static string? _file;

    public static void Main(string[] args)
    {
        //process arguments
        Parser.Default.ParseArguments<Options>(args)
               .WithParsed(RunOptions)
               .WithNotParsed(HandleParseError);

        //If there is a folder to scan, run the process against it
        if (string.IsNullOrEmpty(_directory) == false || _repo != null)
        {
            if (_includeInventory == true)
            {
                Core.Main.GetInventoryResults(_directory, _repo, _file);
            }
            else
            {
                Core.Main.GetFrameworkSummary(_directory, _repo, _includeTotals, _file);
            }
        }
    }

    static void RunOptions(Options opts)
    {
        //handle options
        _directory = opts.Directory;

        //setup the GitHub repo details
        if (opts.Owner != null)
        {
            if (opts.Repo != null)
            {
                _repo = new Target(opts.Owner, opts.Repo)
                {
                    User = opts.Owner,
                    Password = opts.Password
                };
            }
            else
            {
                _repo = new Target(opts.Owner)
                {
                    User = opts.Owner,
                    Password = opts.Password
                };
            }
        }
        if (_directory == null && _repo == null)
        {
            //If both directory and repo are null, use the current directory
            _directory = Directory.GetCurrentDirectory();
        }
        _includeTotals = opts.IncludeTotals;
        _includeInventory = opts.IncludeInventory;
        _file = opts.File;
    }

    static void HandleParseError(IEnumerable<Error> errs)
    {
        //handle errors
        var excList = new List<Exception>();
        foreach (var err in errs)
        {
            excList.Add(new ArgumentException(err.ToString()));
        }
        if (excList.Any())
        {
            throw new AggregateException(excList);
        }
    }
}