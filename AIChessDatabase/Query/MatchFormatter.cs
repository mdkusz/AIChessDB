using BaseClassesAndInterfaces;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using GlobalCommonEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.UIResources;
using static Resources.Properties.Resources;

namespace AIChessDatabase.Query
{
    /// <summary>
    /// Frmatter to export matches to a database
    /// </summary>
    [EnumObjectFilter(nameof(IDataExportFormatter))]
    public class MatchFormatter : IDatabaseExportFormatter, IUIIdentifier
    {
        public MatchFormatter()
        {
            FriendlyName = NAME_MatchFormatter;
            ImportManager = new MatchDataImportManager();
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
        /// IDataExportFormatter: This formatter allows exporting only column headers
        /// </summary>
        public virtual bool ExportHeaders { get { return false; } }
        /// <summary>
        /// IDataExportFormatter: Data target types
        /// </summary>
        public ExportTarget Targets
        {
            get
            {
                return ExportTarget.Database;
            }
        }
        /// <summary>
        /// IDataExportFormatter: Progress monitor object
        /// </summary>
        public IProgressMonitor ProgressMonitor { get; set; }
        /// <summary>
        /// IDatabaseExportFormatter: Database connection index
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// IDatabaseExportFormatter: Data import manager
        /// </summary>
        public IDataImportManager ImportManager { get; set; }
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
                ImportManager.Data = data;
                await Task.Run(async () =>
                {
                    for (int ix = 0; ix < data.Rows.Count; ix++)
                    {
                        string error = await ImportManager.ValidateRowAsync(ix, ConnectionIndex);
                        if (string.IsNullOrEmpty(error))
                        {
                            error = await ImportManager.ImportRowAsync(ix, ConnectionIndex);
                        }
                        if (!string.IsNullOrEmpty(error))
                        {
                            throw new Exception(error);
                        }
                        ProgressMonitor?.Step();
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
