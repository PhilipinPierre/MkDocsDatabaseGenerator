using MkDocsDatabaseGenerator.Model;
using PlantUml.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using MkDocsDatabaseGenerator.ViewModel.Interface;
using MkDocsDatabaseGenerator.Extension;
using System.ComponentModel;
using Wpf.Util;
using Wpf.Util.Extension;

namespace MkDocsDatabaseGenerator.ViewModel
{
#pragma warning disable CS8612 // La nullabilité des types référence dans le type ne correspond pas au membre implémenté implicitement.

    public partial class MainWindowViewModel : NotifyPropertyChanged, IMainWindowsViewModel
#pragma warning restore CS8612 // La nullabilité des types référence dans le type ne correspond pas au membre implémenté implicitement.
    {
        #region Command

        #region Property : CanGenerateDocumentation

        private bool _canGenerateDocumentation;

        public bool CanGenerateDocumentation
        {
            get { return this._canGenerateDocumentation; }
            set
            {
                this._canGenerateDocumentation = value;
                OnPropertyChanged(nameof(this.CanGenerateDocumentation));
                GenerateDocumentationCommand.NotifyCanExecuteChanged();
            }
        }

        #endregion Property : CanGenerateDocumentation

        #region Property : CanMergeGeneratedDocumentation

        private bool _canMergeGeneratedDocumentation;

        public bool CanMergeGeneratedDocumentation
        {
            get { return this._canMergeGeneratedDocumentation; }
            set
            {
                this._canMergeGeneratedDocumentation = value;
                OnPropertyChanged(nameof(this.CanMergeGeneratedDocumentation));
                this.MergeGeneratedDocumentationCommand.NotifyCanExecuteChanged();
            }
        }

        #endregion Property : CanMergeGeneratedDocumentation

        #region Property : CanRegenerateYaml

        private bool _canRegenerateYaml;

        public bool CanRegenerateYaml
        {
            get { return this._canRegenerateYaml; }
            set
            {
                this._canRegenerateYaml = value;
                OnPropertyChanged(nameof(this.CanRegenerateYaml));
                this.RegenerateYamlCommand.NotifyCanExecuteChanged();
            }
        }

        #endregion Property : CanRegenerateYaml

        #region Property : CanStartStopServer

        private bool _canStartStopServer;

        public bool CanStartStopServer
        {
            get { return this._canStartStopServer; }
            set
            {
                this._canStartStopServer = value;
                OnPropertyChanged(nameof(this.CanStartStopServer));
                this.StartStopServerCommand.NotifyCanExecuteChanged();
            }
        }

        #endregion Property : CanStartStopServer

        #endregion Command

        #region Properties

        private bool isGenerating = false;

        #region Property : Database

        private string database = Properties.Settings.Default.Database;// string.Empty;

        public string Database
        {
            get { return database; }
            set
            {
                database = value;
                if (!String.IsNullOrEmpty(database))
                {
                    Properties.Settings.Default.Database = database;
                    Properties.Settings.Default.Save();
                }
                OnPropertyChanged(nameof(Database));
            }
        }

        #endregion Property : Database

        #region Property : FolderPath

        private string folderPath = Properties.Settings.Default.FolderPath;// string.Empty;

        public string FolderPath
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                if (!String.IsNullOrEmpty(folderPath))
                {
                    Properties.Settings.Default.FolderPath = folderPath;
                    Properties.Settings.Default.Save();
                }
                OnPropertyChanged(nameof(FolderPath));
            }
        }

        #endregion Property : FolderPath

        #region Property : Server

        private string server = Properties.Settings.Default.Server;// string.Empty;

        public string Server
        {
            get { return server; }
            set
            {
                server = value;
                if (!String.IsNullOrEmpty(server))
                {
                    Properties.Settings.Default.Server = server;
                    Properties.Settings.Default.Save();
                }
                OnPropertyChanged(nameof(Server));
            }
        }

        #endregion Property : Server

        #region Property : PlantUmlPath

        private string plantUmlPath = Properties.Settings.Default.PlantUmlPath;

        public string PlantUmlPath
        {
            get { return this.plantUmlPath; }
            set
            {
                this.plantUmlPath = value;
                if (!String.IsNullOrEmpty(plantUmlPath))
                {
                    Properties.Settings.Default.PlantUmlPath = plantUmlPath;
                    Properties.Settings.Default.Save();
                }
                OnPropertyChanged(nameof(this.PlantUmlPath));
            }
        }

        #endregion Property : PlantUmlPath

        #region Property : GraphvizDotPath

        private string graphvizDotPath = Properties.Settings.Default.GraphvizDotPath;

        public string GraphvizDotPath
        {
            get { return this.graphvizDotPath; }
            set
            {
                this.graphvizDotPath = value;
                if (!String.IsNullOrEmpty(graphvizDotPath))
                {
                    Properties.Settings.Default.GraphvizDotPath = graphvizDotPath;
                    Properties.Settings.Default.Save();
                }
                OnPropertyChanged(nameof(this.GraphvizDotPath));
            }
        }

        #region Property : OperationQuantity

        private int operationQuantity;

        public int OperationQuantity
        {
            get { return this.operationQuantity; }
            set
            {
                this.operationQuantity = value;
                OnPropertyChanged(nameof(this.OperationQuantity));
            }
        }

        #endregion Property : OperationQuantity

        #region Property : NbOperation

        private int nbOperation;

        public int NbOperation
        {
            get { return this.nbOperation; }
            set
            {
                this.nbOperation = value;
                OnPropertyChanged(nameof(this.NbOperation));
            }
        }

        #endregion Property : NbOperation

        #region Property : Operation

        private string operation = "Doing nothing now";

        public string Operation
        {
            get { return this.operation; }
            set
            {
                this.operation = value;
                OnPropertyChanged(nameof(this.Operation));
            }
        }

        #endregion Property : Operation

        #region Property : SubOperation

        private string subOperation = String.Empty;

        public string SubOperation
        {
            get { return this.subOperation; }
            set
            {
                this.subOperation = value;
                OnPropertyChanged(nameof(this.SubOperation));
            }
        }

        #endregion Property : SubOperation

        #region Property : NewVersion

        private bool newVersion = true;

        public bool NewVersion
        {
            get { return this.newVersion; }
            set
            {
                this.newVersion = value;
                OnPropertyChanged(nameof(this.NewVersion));
            }
        }

        #endregion Property : NewVersion

        #region Property : StartStopServerButton

        public bool IsServerStarting { get; set; } = false;

        public Process? MkDocsServerProcess { get; set; }

        #region Property : IsServerStart

        private bool isServerStart;

        public bool IsServerStart
        {
            get { return this.isServerStart; }
            set
            {
                this.isServerStart = value;
                OnPropertyChanged(nameof(this.IsServerStart));
            }
        }

        #endregion Property : IsServerStart

        #endregion Property : StartStopServerButton

        #endregion Property : GraphvizDotPath

        #region Property : JavaPath

        private string javaPath = Properties.Settings.Default.JavaPath;

        public string JavaPath
        {
            get { return this.javaPath; }
            set
            {
                this.javaPath = value;
                if (!String.IsNullOrEmpty(this.JavaPath))
                {
                    Properties.Settings.Default.JavaPath = this.JavaPath;
                    Properties.Settings.Default.Save();
                }
                OnPropertyChanged(nameof(this.JavaPath));
            }
        }

        #endregion Property : JavaPath

        #region Property : MakeGraph

        private bool makeGraph = true;

        public bool MakeGraph
        {
            get { return this.makeGraph; }
            set
            {
                this.makeGraph = value;
                OnPropertyChanged(nameof(this.MakeGraph));
            }
        }

        #endregion Property : MakeGraph

        #endregion Properties

        public MainWindowViewModel()
        {
            this.CanGenerateDocumentation = true;
            this.CanMergeGeneratedDocumentation = false;
            this.CanRegenerateYaml = false;
            this.CanStartStopServer = Directory.Exists(this.folderPath) && new DirectoryInfo(folderPath).GetFiles("*.yml", SearchOption.TopDirectoryOnly).Any();
        }

        private void CreateIndexMdFile(string folderPath, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = new DirectoryInfo(folderPath)?.Parent?.Name ?? "Unknown";
            }
            List<String> lines = new List<string>();
            lines.Add("# " + name);
            lines.Add("");
            lines.Add("This document describes the needs and development of the project \"" + name + "\".");
            lines.Add("");
            lines.Add("## Database locations");
            lines.Add("");
            var last2 = server.TakeLast(2);
            if (last2.All(c => c == '1'))
            {
                lines.Add("Production : ");
                lines.Add("");
                lines.Add("Test : `" + server + "." + database + "`");
                lines.Add("");
                lines.Add("Development: `" + server + "." + database + "`");
                lines.Add("");
            }
            else
            {
                lines.Add("Production : `" + server + "." + database + "`");
                lines.Add("");
                lines.Add("Test : ");
                lines.Add("");
                lines.Add("Development: ");
                lines.Add("");
            }
            lines.Add("## Historique");
            lines.Add("");
            lines.Add("| Version | Date | Comment |");
            lines.Add("| --- | --- | --- | ");
            lines.Add("| 0 | " + DateTime.Now.ToShortDateString() + " | First Version. | ");

            lines.WriteFile(Path.Combine(folderPath, "index.md"));
        }

        private void CreateIntroMdFile(string folderPath)
        {
            List<String> lines = new List<string>();
            lines.Add("&copy;Copyright Faymonville");
            lines.Add("");
            lines.Add("# Introduction");

            lines.WriteFile(Path.Combine(folderPath, "intro.md"));
        }

        private async Task CreateNewYaml(TextInfo textInfo, string ymlFullPath, CancellationToken cancellationToken)
        {
            if (Path.GetDirectoryName(ymlFullPath) is not string ymlDirectoryPath)
                throw new Exception($"Directory do not exist : {ymlFullPath}");
            DirectoryInfo info = new DirectoryInfo(Path.Combine(ymlDirectoryPath, "docs"));
            if (info != null)
            {
                List<String> lines = new List<string>();
                lines.Add("site_name: " + textInfo.ToTitleCase(Database.Replace('_', ' ')));
                lines.Add("");
                lines.Add("site_url: \"\"");
                lines.Add("");
                lines.Add("use_directory_urls: false");
                lines.Add("");
                lines.Add("nav:");
                buildNavigationFromDirectory(lines, textInfo, info, 1, Path.Combine(ymlDirectoryPath, "docs"));
                lines.Add("");
                lines.Add("");
                lines.Add("");
                lines.Add("");
                lines.Add("theme: ");
                lines.Add("  name: material");
                lines.Add("  custom_dir: custom_theme/");
                lines.Add("  features:");
                lines.Add("    - navigation.tabs");
                lines.Add("    - navigation.tracking");
                lines.Add("    - navigation.sections");
                lines.Add("    - navigation.footer");
                lines.Add("    - navigation.indexes");
                lines.Add("    - toc.integrate");
                lines.Add("    - search.suggest");
                lines.Add("    - search.highlight");
                lines.Add("    - header.autohide");
                lines.Add("  icon:");
                lines.Add("    admonition:");
                lines.Add("      note: octicons/tag-16");
                lines.Add("      abstract: octicons/checklist-16");
                lines.Add("      info: octicons/info-16");
                lines.Add("      tip: octicons/squirrel-16");
                lines.Add("      success: octicons/check-16");
                lines.Add("      question: octicons/question-16");
                lines.Add("      warning: octicons/alert-16");
                lines.Add("      failure: octicons/x-circle-16");
                lines.Add("      danger: octicons/zap-16");
                lines.Add("      bug: octicons/bug-16");
                lines.Add("      example: octicons/beaker-16");
                lines.Add("      quote: octicons/quote-16");
                lines.Add("    repo: material/microsoft-azure-devops");
                lines.Add("  highlightjs: true");
                lines.Add("  hljs_style: sql");
                lines.Add("  hljs_languages:");
                lines.Add("    - sql");
                lines.Add("    - c#");
                lines.Add("  palette:");
                lines.Add("    # Palette toggle for automatic mode");
                lines.Add("    - media: \"(prefers - color - scheme)\"");
                lines.Add("      toggle:");
                lines.Add("        icon: material/brightness-auto");
                lines.Add("        name: Switch to light mode");
                lines.Add("");
                lines.Add("    # Palette toggle for light mode");
                lines.Add("    - media: \"(prefers - color - scheme: light)\"");
                lines.Add("      scheme: default ");
                lines.Add("      toggle:");
                lines.Add("        icon: material/brightness-7");
                lines.Add("        name: Switch to dark mode");
                lines.Add("");
                lines.Add("    # Palette toggle for dark mode");
                lines.Add("    - media: \"(prefers - color - scheme: dark)\"");
                lines.Add("      scheme: slate");
                lines.Add("      toggle:");
                lines.Add("        icon: material/brightness-4");
                lines.Add("        name: Switch to system preference");
                lines.Add("");
                lines.Add("markdown_extensions:");
                lines.Add("  - attr_list");
                lines.Add("  - md_in_html");
                lines.Add("  - tables");
                lines.Add("  - attr_list");
                lines.Add("  - admonition");
                lines.Add("  - pymdownx.caret");
                lines.Add("  - pymdownx.emoji:");
                lines.Add("      emoji_index: !!python/name:material.extensions.emoji.twemoji");
                lines.Add("      emoji_generator: !!python/name:material.extensions.emoji.to_svg");
                lines.Add("  - pymdownx.critic");
                lines.Add("  - pymdownx.caret");
                lines.Add("  - pymdownx.keys");
                lines.Add("  - pymdownx.mark");
                lines.Add("  - pymdownx.tilde");
                lines.Add("  - pymdownx.details");
                lines.Add("  - pymdownx.superfences");
                lines.Add("  - def_list");
                lines.Add("  - toc");
                lines.Add("  - pymdownx.tasklist:");
                lines.Add("      custom_checkbox: true");
                lines.Add("plugins:");
                lines.Add("  - search:");
                lines.Add("      separator: '[\\s\\-,:!=\\[\\]()\"/]+|(?!\\b)(?=[A-Z][a-z])|\\.(?!\\d)|&[lg]t;\\'");
                lines.Add("  - mkdocstrings");
                lines.Add("  - offline");
                lines.Add("  - print-site");

                await lines.WriteFileAsync(filePath: ymlFullPath, cancellationToken: cancellationToken);
            }
        }

        [RelayCommand]
        private void FolderPathSearch(object o)
        {
            FolderPicker dialog = new FolderPicker();
            dialog.InputPath = Properties.Settings.Default.FolderPath;
            if (dialog.ShowDialog() is bool isShowed
                && isShowed
                && dialog.ResultPath is string folderPath
                && !folderPath.IsNullOrEmptyOrWhiteSpace()
                && Directory.Exists(folderPath))
            {
                FolderPath = folderPath;
            }
        }

        private void GenerateForAllServer(object o)
        {
        }

        #region GenerateDocumentation


        [RelayCommand(CanExecute = nameof(this.CanGenerateDocumentation), IncludeCancelCommand = true)]
        public async Task GenerateDocumentation(CancellationToken cancellationToken)
        {
            if (!isGenerating)
            {
                isGenerating = true;

                this.CanMergeGeneratedDocumentation = false;
                this.CanRegenerateYaml = false;
                this.CanStartStopServer = Directory.Exists(this.folderPath) && new DirectoryInfo(folderPath).GetFiles("*.yml", SearchOption.TopDirectoryOnly).Any();
                try
                {
                    using (RenderingViewModel renderingViewModel = new RenderingViewModel(folderPath: this.folderPath, caller: this))
                    {
                        if (String.IsNullOrEmpty(Database) || string.IsNullOrEmpty(Server) || string.IsNullOrEmpty(FolderPath))
                        {
                            MessageBox.Show("All fields have to be filled");
                        }
                        else
                        {

                            Operation = "Create directories.";
                            renderingViewModel.CreateDirectory("custom_theme");
                            renderingViewModel.CreateDirectory("custom_theme", "img");
                            renderingViewModel.CreateDirectory("docs");
                            renderingViewModel.CreateDirectory("docs", "Database");

                            if (!File.Exists(Path.Combine(folderPath, "docs", "intro.md")))
                            {
                                CreateIntroMdFile(Path.Combine(folderPath, "docs"));
                            }
                            if (!File.Exists(Path.Combine(folderPath, "docs", "index.md")))
                            {
                                CreateIndexMdFile(Path.Combine(folderPath, "docs"), new DirectoryInfo(folderPath).Name);
                            }

                            Operation = "Load database informations.";
                            DatabaseGenerator generator = new DatabaseGenerator(Server, Database);
                            var tables = generator.Tables;

                            Operation = "Copy current version in older folder.";

                            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                            if (NewVersion)
                            {
                                renderingViewModel.CreateDirectory("docs", "Database");
                                DirectoryInfo databaseDirectory = new DirectoryInfo(Path.Combine(folderPath, "docs", "Database"));
                                DirectoryInfo[] directories = databaseDirectory.GetDirectories();
                                int max = 0;
                                if (databaseDirectory.GetDirectories().Any(d => d.Name == Constants.OlderFolderName)
                                    && databaseDirectory.GetDirectories("Older", SearchOption.AllDirectories).OrderBy(d => d.FullName.Length).FirstOrDefault() is DirectoryInfo olderDirectoryInfo)
                                {
                                    databaseDirectory = olderDirectoryInfo;
                                    Regex regex = new Regex(@"^V\d$");
                                    if (databaseDirectory.GetDirectories("V*", SearchOption.TopDirectoryOnly).Where(f => regex.IsMatch(f.Name)).OrderByDescending(f => f.Name.Substring(1)).FirstOrDefault()?.Name is string vMax)
                                        Int32.TryParse(vMax.Substring(1).Split('-', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), out max);
                                }
                                renderingViewModel.CreateDirectory("docs", "Database", Constants.OlderFolderName, "V" + (max + 1));
                                foreach (var directory in directories.Where(d => d.Name != Constants.OlderFolderName))
                                {
                                    renderingViewModel.CreateDirectory("docs", "Database", Constants.OlderFolderName, "V" + (max + 1), directory.Name);
                                    directory.FullName.CopyFilesRecursively(Path.Combine(folderPath, "docs", "Database", Constants.OlderFolderName, "V" + (max + 1), directory.Name));
                                }
                                foreach (FileInfo newPath in new DirectoryInfo(Path.Combine(folderPath, "docs", "Database")).GetFiles("*.*", SearchOption.TopDirectoryOnly))
                                {
                                    File.Copy(newPath.FullName, newPath.FullName.Replace(Path.Combine(folderPath, "docs", "Database"), Path.Combine(folderPath, "docs", "Database", Constants.OlderFolderName, "V" + (max + 1))), true);
                                }
                                await MakeNewVersionNavigation(textInfo, folderPath, max + 1, cancellationToken: cancellationToken);
                            }
                            else if (!File.Exists(Path.Combine(folderPath, "mkdocs.yml")))
                            {
                                await CreateNewYaml(textInfo, Path.Combine(folderPath, "mkdocs.yml"), cancellationToken: cancellationToken);
                            }
                            Operation = "Generate Custom MarkDown files.";
                            renderingViewModel.CreateDirectory("docs", "Database", Constants.CustomFolderName);
                            GenerateCustomMarkdowns(textInfo, tables, Path.Combine(folderPath, "docs", "Database", Constants.CustomFolderName));

                            OperationQuantity = tables.Count() * 4 + 5;
                            NbOperation = 0;

                            string global = String.Empty;

                            if (MakeGraph)
                            {
                                Operation = "Generate table schemas.";
                                global = generateTableDiagrams(renderingViewModel, tables, Path.Combine(folderPath, "docs", "Database"));
                            }

                            SubOperation = "Generate Markdown Files";
                            generateTableMarkdowns(renderingViewModel, tables: tables, folderPath: Path.Combine(folderPath, "docs", "Database"), makeGraph: MakeGraph, cancellationToken: cancellationToken);

                            if (MakeGraph)
                            {
                                Operation = "Render table schemas.";
                                await renderingViewModel.RenderDiagram(
                                    tables: tables,
                                    folderPath: Path.Combine(folderPath, "docs", "Database"),
                                    global: global,
                                    cancellationToken: cancellationToken);
                            }

                        }
                        //SubOperation = "Update Navigation");
                    }
                }
                catch (OperationCanceledException e)
                {
                    Logging.Instance.Warning(e);
                    this.Operation = "Generate Database documentation";
                    this.SubOperation = "Cancelled";
                }
                catch (Exception e)
                {
                    Logging.Instance.Error(e);
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    Operation = String.Empty;
                    SubOperation = String.Empty;
                    OperationQuantity = 0;
                    NbOperation = 0;
                    isGenerating = false;
                    this.CanMergeGeneratedDocumentation = true;
                }
            }
        }

        private void buildNavigationFromDirectory(List<String> lines, TextInfo textInfo, DirectoryInfo info, int level, string folderOrigin)
        {
            if ((info.GetFiles("*.md", SearchOption.AllDirectories)?.Length ?? 0) > 0 && !info.Name.Contains(Constants.CustomFolderName))
            {
                if (info.Name == Constants.OlderFolderName)
                {
                    foreach (DirectoryInfo directory in info.GetDirectories("*", SearchOption.TopDirectoryOnly))
                    {
                        StringBuilder builder = new StringBuilder();
                        for (int i = 0; i < level; i++)
                        {
                            builder.Append("  ");
                        }
                        builder.Append("- " + textInfo.ToTitleCase(Path.GetFileNameWithoutExtension(directory.Name)) + " : ");
                        lines.Add(builder.ToString());
                        buildNavigationFromDirectory(lines, textInfo, directory, level + 1, folderOrigin);
                    }
                }
                else if ((info.GetFiles("*.md", SearchOption.TopDirectoryOnly)?.Length ?? 0) == 0 && (info.GetDirectories("*", SearchOption.TopDirectoryOnly)?.Length ?? 0) > 0)
                {
                    foreach (DirectoryInfo directory in info.GetDirectories("*", SearchOption.TopDirectoryOnly))
                    {
                        buildNavigationFromDirectory(lines, textInfo, directory, level, folderOrigin);
                    }
                }
                else
                {
                    foreach (FileInfo file in info.GetFiles("*.md", SearchOption.TopDirectoryOnly).Where(f => !f.Name.StartsWith(Constants.CustomStartFileName) && !f.Name.StartsWith(Constants.AutoGeneratedStartFileName)))
                    {
                        StringBuilder builder = new StringBuilder();
                        for (int i = 0; i < level; i++)
                        {
                            builder.Append("  ");
                        }
                        builder.Append("- " + textInfo.ToTitleCase(Path.GetFileNameWithoutExtension(file.Name)) + " : " + file.FullName.Replace(folderOrigin + "\\", ""));
                        lines.Add(builder.ToString());
                    }
                    foreach (DirectoryInfo directory in info.GetDirectories("*", SearchOption.TopDirectoryOnly).Where(d => !d.Name.Contains(Constants.CustomFolderName)))
                    {
                        StringBuilder builder = new StringBuilder();
                        for (int i = 0; i < level; i++)
                        {
                            builder.Append("  ");
                        }
                        builder.Append("- " + textInfo.ToTitleCase(Path.GetFileNameWithoutExtension(directory.Name)) + " : ");
                        lines.Add(builder.ToString());
                        buildNavigationFromDirectory(lines, textInfo, directory, level + 1, folderOrigin);
                    }
                }
            }
        }

        private async Task CreateNewVersionInIndexFile(string folderPath, string version, string comment, CancellationToken cancellationToken)
        {
            if (Directory.Exists(folderPath))
            {
                DirectoryInfo directory = new DirectoryInfo(folderPath);
                if (directory.GetFiles("index.md", SearchOption.TopDirectoryOnly).FirstOrDefault() is FileInfo index)
                {
                    String[] lines = File.ReadAllLines(index.FullName);
                    string escape = Regex.Escape("|");
                    Regex regex = new Regex(escape + "*Version*" + escape + "*Date*" + escape + "*Comment*" + escape);
                    int iLine = 0;
                    while (iLine < lines.Length && !regex.IsMatch(lines[iLine]))
                    {
                        iLine++;
                    }
                    while (iLine < lines.Length && lines[iLine].StartsWith("|"))
                    {
                        iLine++;
                    }

                    List<String> indexNew = new List<string>();
                    indexNew.AddRange(lines.Split(iLine, out string[] last));
                    indexNew.Add("| " + version + " | " + DateTime.Now.ToShortDateString() + " | " + comment + " |");
                    indexNew.AddRange(last);

                    await indexNew.WriteFileAsync(index.FullName, cancellationToken: cancellationToken);
                }
            }
        }

        private void GenerateCustomMarkdowns(TextInfo textInfo, ICollection<Table> tables, string folderPath)
        {
            foreach (Table table in tables)
            {
                if (!File.Exists(Path.Combine(folderPath, table.MdFile)))
                {
                    List<String> lines = new List<string>();
                    lines.Add("#" + textInfo.ToTitleCase(table.Name));
                    lines.Add("");
                    lines.Add("");
                    lines.Add("");
                    File.WriteAllLines(Path.Combine(folderPath, Constants.CustomStartFileName + table.MdFile), lines);
                }
            }
        }

        private string generateTableDiagrams(RenderingViewModel renderingViewModel, ICollection<Table> tables, string folderPath)
        {
            StringBuilder global = new StringBuilder();
            global.AppendLine(Constants.PlantUmlStart);
            global.AppendLine(Constants.PlantUmlTheme);
            global.AppendLine(Constants.PlantUmlBaseStyle);
            global.AppendLine(Constants.PlantUmlOrthoParameter);

            List<string> references = new List<string>();
            foreach (Table table in tables)
            {
                //Render table
                SubOperation = String.Format("Generating {0} table schema", table.Name);
                renderingViewModel.CreateDirectory("docs", "Database", "Table", table.TableName);
                var diagram = simpleDiagram(table);
                table.TableUml = diagram;
                global.AppendLine(diagram);
            }
            foreach (Table tableGenerated in tables)
            {
                //Render table with reference
                SubOperation = String.Format("Generating {0} Reference table schema", tableGenerated.Name);
                references.Clear();
                foreach (Reference reference in tableGenerated.References.Where(r => r.Referenced_TableName != r.TableName))
                {
                    references.Add(String.Format("\"{0}\" {1} \"{2}\"", reference.TableName.Replace(' ', '_'), "|" + (reference.ColumnNullable ? "o" : "|") + ".." + (reference.Referenced_ColumnNullable ? "o" : "|") + "{", reference.Referenced_TableName.Replace(' ', '_')));
                }
                references = references.Distinct().ToList();
                StringBuilder builder = new StringBuilder();
                references.ForEach(r => builder.AppendLine(r));
                string referencesString = builder.ToString();
                tableGenerated.ReferencesUml = referencesString;

                //Rendre table reference by
                SubOperation = String.Format("Generating {0} Reference by table schema", tableGenerated.Name);
                references.Clear();
                foreach (ReferenceBy reference in tableGenerated.ReferenceBies.Where(r => r.Referenced_By_TableName != r.TableName))
                {
                    references.Add(String.Format("\"{0}\" {1} \"{2}\"", reference.TableName.Replace(' ', '_'), "}o.." + (reference.Referenced_By_ColumnNameNullable ? "o" : "|") + "|", reference.Referenced_By_TableName.Replace(' ', '_')));
                }
                references = references.Distinct().ToList();

                StringBuilder builderReferenceBy = new StringBuilder();
                references.ForEach(r => builderReferenceBy.AppendLine(r));
                tableGenerated.ReferenceBiesUml = builderReferenceBy.ToString();
            }

            SubOperation = "Generating global table schema";

            //Global with references
            references = new List<string>();
            foreach (Table table in tables)
            {
                foreach (Reference reference in table.References)
                {
                    references.Add(String.Format("\"{0}\" {1} \"{2}\"", reference.TableName.Replace(' ', '_'), "|" + (reference.ColumnNullable ? "o" : "|") + ".." + (reference.Referenced_ColumnNullable ? "o" : "|") + "{", reference.Referenced_TableName.Replace(' ', '_')));
                }
            }
            references = references.Distinct().ToList();
            references.ForEach(r => global.AppendLine(r));
            global.AppendLine(Constants.PlantUmlEnd);

            NbOperation++;
            return global.ToString();
        }

        private void generateTableMarkdowns(RenderingViewModel renderingViewModel, ICollection<Table> tables, string folderPath, bool makeGraph = false, CancellationToken? cancellationToken = null)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            renderingViewModel.CreateDirectory("docs", "Database", Constants.TableAutoGeneratedFolderName);
            if (makeGraph)
            {
                StringBuilder globalBuilder = new StringBuilder();
                globalBuilder.AppendLine("# Database");
                globalBuilder.AppendLine("");
                globalBuilder.AppendLine("# Global Database Schema");
                globalBuilder.AppendLine("");
                globalBuilder.AppendLine("![Global]([full_database.svg](full_database.svg))");
                globalBuilder.WriteFile(Path.Combine(folderPath, "general.md"));
            }

            foreach (Table table in tables)
            {
                // Table
                renderingViewModel.CreateDirectory("docs", "Database", Constants.TableAutoGeneratedFolderName, table.TableName);
                cancellationToken?.ThrowIfCancellationRequested();

                StringBuilder builder = new StringBuilder();
                builder.AppendLine("\n## Information sur la table\n");
                builder.AppendLine("| Table Name          | Column Name      | Column Type      | Column Length | Column Precision | Remarks |");
                builder.AppendLine("| ------------------- | ---------------- | ---------------- | ------------- | ---------------- | ------- |");
                foreach (Column column in table.Columns.OrderBy(c => c.ColumnId))
                {
                    builder.Append("| " + table.TableName + " | ");
                    builder.Append(column.ColumnName + " | ");
                    builder.Append(column.ColumnType + " | ");
                    builder.Append(column.ColumnLength + " | ");
                    builder.AppendLine(column.ColumnPrecision + " |");
                }
                // Show Table Graph
                if (makeGraph)
                {
                    builder.AppendLine("\n## Schema\n");
                    builder.AppendLine("![" + table.Name + "]([" + table.ImageFile + "](" + table.ImageFile + "))");
                }

                // Reference
                builder.AppendLine("\n## References\n\n### Table\n");

                builder.AppendLine("| Fk Name | Schema Name | Column | Referenced Table | Referenced Column |");
                builder.AppendLine("| ------- | ----------- | ------ | ---------------- | ----------------- |");
                foreach (Reference reference in table.References)
                {
                    builder.Append("| " + reference.FK_Name + " | ");
                    builder.Append(reference.Schema_Name + " | ");
                    builder.Append(reference.ColumnName + " | ");
                    builder.Append("[" + reference.Referenced_TableName + @"](../" + reference.Referenced_TableName + @"/" + reference.Referenced_Table.MdFile + ") | ");
                    builder.AppendLine(reference.Referenced_ColumnName + " | ");
                }

                // Show Reference Graph
                if (makeGraph)
                {
                    builder.AppendLine("\n### Schema\n");
                    builder.AppendLine("![" + table.Name + "]([" + table.ImageReferenceFile + "](" + table.ImageReferenceFile + "))");
                }

                // Reference By
                builder.AppendLine("\n## References By\n\n### Table\n");

                builder.AppendLine("| Schema | Table | Column | Referenced By | Referenced Name | Referenced by Column |");
                builder.AppendLine("| ------ | ----- | ------ | ------------- | --------------- | -------------------- |");
                foreach (ReferenceBy referenceBy in table.ReferenceBies)
                {
                    builder.Append("| " + referenceBy.Schema_Name + " | ");
                    builder.Append(referenceBy.TableName + " | ");
                    builder.Append(referenceBy.ColumnName + " | ");
                    builder.Append("[" + referenceBy.Referenced_By_Table.Name + @"](../" + referenceBy.Referenced_By_TableName + @"/" + referenceBy.Referenced_By_Table.MdFile + ") | ");
                    builder.Append(referenceBy.Referenced_By_FK_Name + " | ");
                    builder.AppendLine(referenceBy.Referenced_By_ColumnName + " | ");
                }

                // Show Reference by Graph
                if (makeGraph)
                {
                    builder.AppendLine("\n### Schema\n");
                    builder.AppendLine("![" + table.Name + "]([" + table.ImageReferenceByFile + "](" + table.ImageReferenceByFile + "))");
                }

                builder.WriteFile(Path.Combine(folderPath, Constants.TableAutoGeneratedFolderName, table.TableName, Constants.AutoGeneratedStartFileName + table.MdFile));

                NbOperation++;
            }
        }

        private async Task MakeNewVersionNavigation(TextInfo textInfo, string folderPath, int newVersion, CancellationToken cancellationToken)
        {
            if (!File.Exists(Path.Combine(folderPath, "mkdocs.yml")))
                await CreateNewYaml(textInfo, Path.Combine(folderPath, "mkdocs.yml"), cancellationToken: cancellationToken);

            await CreateNewVersionInIndexFile(
                folderPath: Path.Combine(folderPath, "docs"),
                version: "V" + newVersion,
                comment: "Version auto-generated.",
                cancellationToken: cancellationToken);

            string[] lines = File.ReadAllLines(Path.Combine(folderPath, "mkdocs.yml"));

            List<String> updateNavigation = new List<String>();
            int iLine;
            for (iLine = 0; iLine < lines.Length && lines[iLine] != "nav:"; iLine++)
            {
                updateNavigation.Add(lines[iLine]);
            }

            if (iLine < lines.Length)
            {
                updateNavigation.Add(lines[iLine]);
                int spacingDatabase = lines[iLine].CountFirstSpaces();
                while (iLine < lines.Length && !lines[iLine].Contains("- Table :"))
                {
                    updateNavigation.Add(lines[iLine]);
                    iLine++;
                }
                if (iLine < lines.Length)
                {
                    int spacingDatabaseTable = lines[iLine].CountFirstSpaces();
                    if (spacingDatabase < spacingDatabaseTable)
                    {
                        int iEndNavigation = iLine;
                        while (iEndNavigation < lines.Length && lines[iEndNavigation].StartsWith(' '))
                        {
                            iEndNavigation++;
                        }

                        String[] databaseNavigation = lines.Split(iLine + 1, iEndNavigation, out string[] beforeNavigation, out string[] afterNavigation);

                        int iOlder = iLine;
                        while (iOlder > 0 && (lines[iOlder].CountFirstSpaces() != spacingDatabaseTable || !lines[iOlder].Contains("- Older :")))
                        {
                            iOlder--;
                        }
                        if (iOlder == 0)
                        {
                            StringBuilder builder = new StringBuilder();
                            for (int iTab = 0; iTab < spacingDatabaseTable; iTab++)
                                builder.Append(' ');
                            builder.Append("- Older :");
                            updateNavigation.Add(builder.ToString());
                            builder.Clear();
                            for (int iTab = 0; iTab < spacingDatabaseTable + 1; iTab++)
                                builder.Append(' ');
                            builder.Append("  ");
                            builder.Append("- V1 :");
                            updateNavigation.Add(builder.ToString());
                            foreach (string nav in databaseNavigation)
                            {
                                updateNavigation.Add("    " + nav.Replace(@"Database\Table_autoGenerated", @"Database\Table_autoGenerated\Older\V1"));
                            }
                        }
                        else
                        {
                            int olderSpacing = lines[iOlder].CountFirstSpaces();
                            while (iOlder < lines.Length && lines[iOlder].CountFirstSpaces() != olderSpacing)
                            {
                                iOlder++;
                            }
                            iLine = iOlder;

                            StringBuilder builder = new StringBuilder();
                            for (int iTab = 0; iTab < spacingDatabaseTable; iTab++)
                                builder.Append(' ');
                            builder.Append("  ");
                            builder.AppendLine("- V" + newVersion + " :");
                            updateNavigation.Add(builder.ToString());
                            foreach (string nav in databaseNavigation)
                            {
                                updateNavigation.Add("    " + nav.Replace(@"Database\Table_autoGenerated", @"Database\Table_autoGenerated\Older\V" + newVersion));
                            }
                        }

                        while (iLine < lines.Length)
                        {
                            updateNavigation.Add(lines[iLine]);
                            iLine++;
                        }

                        File.WriteAllLines(Path.Combine(folderPath, "mkdocs.yml"), updateNavigation);
                    }
                }
            }
        }

        private string simpleDiagram(Table table)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(String.Format("entity \"{0}\" as {1}", table.TableName.Replace("_", " "), table.TableName.Replace(' ', '_')) + "{");
            foreach (Column column in table.Columns.Where(c => c.IsPrimaryKey))
            {
                builder.AppendLine(String.Format("  {0} : {1}({2},{3}) {4} {5}", column.ColumnName, column.ColumnType, column.ColumnLength, column.ColumnPrecision, column.IsIdentity ? "<<generated>>" : "", column.IsNullable ? "null" : ""));
            }
            builder.AppendLine("  --");
            foreach (Column column in table.Columns.Where(c => !c.IsPrimaryKey))
            {
                builder.AppendLine(String.Format("  {0} : {1}({2},{3}) {4} {5}", column.ColumnName, column.ColumnType, column.ColumnLength, column.ColumnPrecision, column.IsIdentity ? "<<generated>>" : "", column.IsNullable ? "null" : ""));
            }
            builder.AppendLine("}");
            return builder.ToString();
        }

        #endregion GenerateDocumentation

        #region MergeGeneratedDocumentation

        [RelayCommand(CanExecute = nameof(this.CanMergeGeneratedDocumentation), IncludeCancelCommand = true)]
        public async Task MergeGeneratedDocumentation(CancellationToken cancellationToken)
        {
            try
            {
                this.CanMergeGeneratedDocumentation = false;
                this.CanRegenerateYaml = false;
                this.CanStartStopServer = false;
                if (Directory.Exists(Path.Combine(folderPath, "docs", "Database", Constants.TableAutoGeneratedFolderName)) && Directory.Exists(Path.Combine(folderPath, "docs", "Database", Constants.CustomFolderName)))
                {
                    DirectoryInfo autoGeneratedDir = new DirectoryInfo(Path.Combine(folderPath, "docs", "Database", Constants.TableAutoGeneratedFolderName));
                    DirectoryInfo customDir = new DirectoryInfo(Path.Combine(folderPath, "docs", "Database", Constants.CustomFolderName));
                    FileInfo[] customFiles = customDir.GetFiles("*.md", SearchOption.TopDirectoryOnly);
                    foreach (DirectoryInfo directory in autoGeneratedDir.GetDirectories("*", SearchOption.TopDirectoryOnly).Where(d => d.Name != Constants.OlderFolderName))
                    {
                        FileInfo[] files = directory.GetFiles("*.md", SearchOption.TopDirectoryOnly);
                        if (customFiles.Any(f => f.Name.StartsWith(Constants.CustomStartFileName)) && files.Any(f => f.Name.StartsWith(Constants.AutoGeneratedStartFileName)))
                        {
                            using (FileStream stream = File.Create(files.Single(f => f.Name.StartsWith(Constants.AutoGeneratedStartFileName)).FullName.Replace(Constants.AutoGeneratedStartFileName, "")))
                            {
                                using (StreamWriter writer = new StreamWriter(stream))
                                {
                                    StringBuilder customBuilder = new StringBuilder();
                                    foreach (string line in await File.ReadAllLinesAsync(Path.Combine(customDir.FullName, Constants.CustomStartFileName + files.Single(f => f.Name.StartsWith(Constants.AutoGeneratedStartFileName)).Name.Replace(Constants.AutoGeneratedStartFileName, "")), cancellationToken: cancellationToken))
                                    {
                                        customBuilder.AppendLine(line);
                                    }
                                    await writer.WriteLineAsync(value: customBuilder, cancellationToken: cancellationToken);
                                    StringBuilder generatedBuilder = new StringBuilder();
                                    foreach (string line in await File.ReadAllLinesAsync(files.Single(f => f.Name.StartsWith(Constants.AutoGeneratedStartFileName)).FullName))
                                    {
                                        generatedBuilder.AppendLine(line);
                                    }
                                    await writer.WriteLineAsync(generatedBuilder, cancellationToken: cancellationToken);
                                    await writer.FlushAsync();
                                }
                            }
                        }
                    }
                    this.CanRegenerateYaml = true;
                }
            }
            catch (OperationCanceledException e)
            {
                Logging.Instance.Warning(e);
                this.Operation = "Merge generate and custom";
                this.SubOperation = "Cancelled";
            }
            catch (Exception e)
            {
                Logging.Instance.Error(e);
            }
            finally
            {
                this.CanMergeGeneratedDocumentation = true;
            }
        }

        #endregion MergeGeneratedDocumentation

        #region RegenerateYaml

        [RelayCommand(CanExecute = nameof(this.CanRegenerateYaml), IncludeCancelCommand = true)]
        public async Task RegenerateYaml(CancellationToken cancellationToken)
        {
            try
            {
                this.CanRegenerateYaml = false;
                this.CanStartStopServer = Directory.Exists(this.folderPath) && new DirectoryInfo(folderPath).GetFiles("*.yml", SearchOption.TopDirectoryOnly).Any();

                if (File.Exists(Path.Combine(folderPath, "mkdocs.yml")))
                    File.Delete(Path.Combine(folderPath, "mkdocs.yml"));
                await CreateNewYaml(CultureInfo.CurrentCulture.TextInfo, Path.Combine(folderPath, "mkdocs.yml"), cancellationToken: cancellationToken);
                this.CanStartStopServer = true;
            }
            catch (OperationCanceledException e)
            {
                Logging.Instance.Warning(e);
                this.Operation = "Regenerate YML";
                this.SubOperation = "Cancelled";
            }
            catch (Exception e)
            {
                Logging.Instance.Error(e);
            }
            finally
            {
                this.CanRegenerateYaml = true;
            }
        }

        #endregion RegenerateYaml

        #region StartStopServer

        [RelayCommand(CanExecute = nameof(this.CanStartStopServer), IncludeCancelCommand = true)]
        public async Task StartStopServer(CancellationToken cancellationToken)
        {
            if (!IsServerStarting)
            {
                IsServerStarting = true;
                try
                {
                    this.CanStartStopServer = false;
                    if (!IsServerStart && MkDocsServerProcess != null && Process.GetProcessById(MkDocsServerProcess.Id) is Process runningProcess)
                    {
                        MessageBox.Show("A Server is currently running. Please Stop it before creating a new server.");
                        IsServerStart = false;
                    }
                    else
                    {
                        if (!IsServerStart)
                        {
                            string strCmdText = "mkdocs serve";

                            MkDocsServerProcess = new System.Diagnostics.Process();
                            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                            startInfo.FileName = "cmd.exe";
                            startInfo.Arguments = "/C " + strCmdText;
                            MkDocsServerProcess.StartInfo = startInfo;
                            if (MkDocsServerProcess.Start())
                            {
                            }
                            await MkDocsServerProcess.WaitForExitAsync(cancellationToken: cancellationToken);
                            if (MkDocsServerProcess.ExitCode == 1)
                            {
                                IsServerStart = true;
                            }
                            else
                            {
                                IsServerStart = false;
                            }
                        }
                        else
                        {
                            MkDocsServerProcess?.Kill(true);
                            IsServerStart = false;
                            MkDocsServerProcess = null;
                        }
                    }
                }
                catch (OperationCanceledException e)
                {
                    Logging.Instance.Warning(e);
                    this.Operation = "Regenerate YML";
                    this.SubOperation = "Cancelled";
                    if (IsServerStart)
                        MkDocsServerProcess = null;
                    IsServerStart = false;
                }
                catch (Exception e)
                {
                    Logging.Instance.Error(e);
                    MkDocsServerProcess = null;
                    IsServerStart = false;
                }
                finally
                {
                    this.CanStartStopServer = true;
                }
                IsServerStarting = false;
            }
        }

        #endregion StartStopServer

        [RelayCommand]
        private void GraphvizDotPathSearch(object o)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.GraphvizDotPath);
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.FileName = "dot.exe";
            dialog.DefaultExt = "exe";
            dialog.Filter = "Executable File (*.exe)|*.exe|All files (*.*)|*.*";
            if (dialog.ShowDialog() ?? false)
            {
                GraphvizDotPath = dialog.FileName;
            }
        }

        [RelayCommand]
        private void JavaPathSearch(object o)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Properties.Settings.Default.JavaPath;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.FileName = "java.exe";
            dialog.DefaultExt = "exe";
            dialog.Filter = "Executable File (*.exe)|*.exe|All files (*.*)|*.*";
            if (dialog.ShowDialog() ?? false)
            {
                JavaPath = dialog.FileName;
            }
        }

        [RelayCommand]
        private void PlantUmlPathSearch(object o)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.PlantUmlPath);
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.DefaultExt = "jar";
            dialog.Filter = "Java archive (*.jar)|*.jar|All files (*.*)|*.*";
            if (dialog.ShowDialog() ?? false)
            {
                PlantUmlPath = dialog.FileName;
            }
        }
    }
}