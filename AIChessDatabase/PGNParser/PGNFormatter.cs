using AIChessDatabase.Data;
using AIChessDatabase.Interfaces;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using GlobalCommonEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.UIResources;
using static Resources.Properties.Resources;

namespace AIChessDatabase.PGNParser
{
    /// <summary>
    /// Export matches in PGN format.
    /// </summary>
    public class PGNFormatter : IFileExportFormatter, IUIIdentifier
    {
        public PGNFormatter()
        {
            FriendlyName = NAME_PGNFormatter;
        }
        /// <summary>
        /// IUIIdentifier: Element name
        /// </summary>
        public string FriendlyName { get; set; }
        /// <summary>
        /// IUIIdentifier: Element description
        /// </summary>
        public string FriendlyDescription { get; set; }
        /// <summary>
        /// IUIIdentifier: Element category
        /// </summary>
        public string UICategory
        {
            get
            {
                return CATEGORY_DATAFORMATTERS;
            }
            set { }
        }
        /// <summary>
        /// IUIIdentifier: Object implementation
        /// </summary>
        public object Implementation()
        {
            return this;
        }
        /// <summary>
        /// IUIIdentifier: Parameterized object implementation
        /// </summary>
        /// <param name="p">
        /// parameter array
        /// </param>
        /// <returns>
        /// Initialized object
        /// </returns>
        public object ParamImplementation(params object[] p)
        {
            return this;
        }
        /// <summary>
        /// Database connection index, used to identify the connection in multi-connection scenarios.
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// Database repository to use for data export
        /// </summary>
        public IObjectRepository Repository { get; set; }
        /// <summary>
        /// Flag to export match and move comments in the PGN file.
        /// </summary>
        public bool ExportComments { get; set; }
        /// <summary>
        /// IDataExportFormatter: This formatter allows exporting only column headers
        /// </summary>
        public bool ExportHeaders { get { return false; } }
        /// <summary>
        /// IDataExportFormatter: Data target types
        /// </summary>
        public ExportTarget Targets
        {
            get
            {
                return ExportTarget.File;
            }
        }
        /// <summary>
        /// IFileExportFormatter: File selection filter
        /// </summary>
        public string FileFilter
        {
            get
            {
                return FIL_PGNFiles;
            }
        }
        /// <summary>
        /// IFileExportFormatter: Selected file name
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// IDataExportFormatter: Executable name to open the file
        /// </summary>
        public string ExeName
        {
            get
            {
                return "Notepad";
            }
        }
        /// <summary>
        /// IDataExportFormatter: Progress monitor object
        /// </summary>
        public IProgressMonitor ProgressMonitor { get; set; }
        /// <summary>
        /// IDataExportFormatter: Export formatted data to a file
        /// </summary>
        /// <param name="target">
        /// Data target
        /// </param>
        /// <param name="data">
        /// Data to export
        /// </param>
        /// <param name="formatters">
        /// Format configuration
        /// </param>
        public async Task<string> ExportData(ExportTarget target, DataTable data, List<QueryColumn> formatters)
        {
            try
            {
                ProgressMonitor?.Reset(this);
                ProgressMonitor?.SetTotalSteps(data.Rows.Count);
                await Task.Run(async () =>
                {
                    using (StreamWriter writer = new StreamWriter(FileName))
                    {
                        for (int ix = 0; ix < data.Rows.Count; ix++)
                        {
                            ulong m = Convert.ToUInt64(data.Rows[ix]["cod_match"]);
                            Match match = Repository.CreateObject(typeof(Match)) as Match;
                            await match.FastLoad(m, ConnectionIndex);
                            writer.WriteLine(match.GetPGN(ExportComments));
                            ProgressMonitor?.Step();
                        }
                        writer.Close();
                    }
                });
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                ProgressMonitor?.Stop(this);
            }
        }
        public override string ToString()
        {
            return FriendlyName;
        }
    }
}
