﻿// -------------------------------------------------------
// © Kaplas. Licensed under MIT. See LICENSE for details.
// -------------------------------------------------------
namespace ParTool
{
    using System;
    using System.IO;
    using ParLibrary.Converter;
    using Yarhl.FileSystem;

    /// <summary>
    /// Create contents functionality.
    /// </summary>
    internal static partial class Program
    {
        private static void Create(Options.Create opts)
        {
            WriteHeader();

            if (!Directory.Exists(opts.InputDirectory))
            {
                Console.WriteLine($"ERROR: \"{opts.InputDirectory}\" not found!!!!");
                return;
            }

            if (File.Exists(opts.ParArchivePath))
            {
                Console.WriteLine("WARNING: Output file already exists. It will be overwritten.");
                Console.Write("Continue? (y/N) ");
                string answer = Console.ReadLine();
                if (!string.IsNullOrEmpty(answer) && answer.ToUpperInvariant() != "Y")
                {
                    Console.WriteLine("CANCELLED BY USER.");
                    return;
                }

                File.Delete(opts.ParArchivePath);
            }

            var parameters = new ParArchiveWriterParameters
            {
                CompressorVersion = opts.Compression,
                OutputPath = Path.GetFullPath(opts.ParArchivePath),
                IncludeDots = !opts.AlternativeMode,
            };

            Console.Write("Reading input directory... ");
            string nodeName = new DirectoryInfo(opts.InputDirectory).Name;
            Node node = NodeFactory.FromDirectory(opts.InputDirectory, "*", nodeName, true);

#pragma warning disable CA1308 // Normalize strings to uppercase
            node.SortChildren((x, y) => string.CompareOrdinal(x.Name.ToLowerInvariant(), y.Name.ToLowerInvariant()));
#pragma warning restore CA1308 // Normalize strings to uppercase
            Console.WriteLine("DONE!");

            ParArchiveWriter.NestedParCreating += sender => Console.WriteLine($"Creating nested PAR {sender.Name}... ");
            ParArchiveWriter.NestedParCreated += sender => Console.WriteLine($"{sender.Name} created!");
            ParArchiveWriter.FileCompressing += sender => Console.WriteLine($"Compressing {sender.Name}... ");

            DateTime startTime = DateTime.Now;
            Console.WriteLine("Creating PAR (this may take a while)... ");
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(opts.ParArchivePath)));
            node.TransformWith<ParArchiveWriter, ParArchiveWriterParameters>(parameters);
            node.Dispose();

            DateTime endTime = DateTime.Now;
            Console.WriteLine("DONE!");

            Console.WriteLine($"Time elapsed: {endTime - startTime:g}");
        }
    }
}
