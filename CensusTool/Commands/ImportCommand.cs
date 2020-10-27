using Census.Core;
using Census.Import;
using Census.MySql;
using Fusi.Tools;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CensusTool.Commands
{
    public sealed class ImportCommand : ICommand
    {
        private readonly IConfiguration _config;
        private readonly string _inputDir;
        private readonly string _fileMask;
        private readonly string _dbName;

        public ImportCommand(AppOptions options, string inputDir,
            string fileMask, string dbName)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _config = options.Configuration;
            _inputDir = inputDir ??
                throw new ArgumentNullException(nameof(inputDir));
            _fileMask = fileMask ??
                throw new ArgumentNullException(nameof(fileMask));
            _dbName = dbName ??
                throw new ArgumentNullException(nameof(dbName));
        }

        public static void Configure(CommandLineApplication command,
            AppOptions options)
        {
            command.Description = "Import CSV data into a MySql DB.";
            command.HelpOption("-?|-h|--help");

            CommandArgument inputDirArgument = command.Argument("[input-dir]",
                "The input files directory");
            CommandArgument fileMaskArgument = command.Argument("[file-mask]",
                "The input files mask");
            CommandArgument dbNameArgument = command.Argument("[db-name]",
                "The target database name");

            command.OnExecute(() =>
            {
                options.Command = new ImportCommand(
                    options,
                    inputDirArgument.Value,
                    fileMaskArgument.Value,
                    dbNameArgument.Value);
                return 0;
            });
        }

        public Task Run()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("IMPORT DATA\n");
            Console.ResetColor();
            Console.WriteLine(
                $"Input dir: {_inputDir}\n" +
                $"Input mask: {_fileMask}\n" +
                $"DB name: {_dbName}\n");

            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddSerilog(Log.Logger);

            string csTemplate = _config.GetConnectionString("Census");
            string cs = string.Format(csTemplate, _dbName);
            CsvImporter importer = new CsvImporter(cs)
            {
                Logger = loggerFactory.CreateLogger("import")
            };

            IDbManager manager = new MySqlDbManager(csTemplate);
            if (!manager.Exists(_dbName))
            {
                manager.CreateDatabase(
                    _dbName,
                    CsvImporter.GetTargetSchema(),
                    null);
            }

            foreach (string filePath in Directory.GetFiles(
                _inputDir, _fileMask)
                .OrderBy(s => s))
            {
                Console.WriteLine(Path.GetFileName(filePath));

                importer.Import(filePath, CancellationToken.None,
                    new Progress<ProgressReport>(
                        r => Console.Write(" " + r.Message)));
                Console.WriteLine(" done");
            }

            return Task.CompletedTask;
        }
    }
}
