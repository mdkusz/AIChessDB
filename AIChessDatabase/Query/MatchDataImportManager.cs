using AIChessDatabase.Data;
using BaseClassesAndInterfaces.ImportExport;
using BaseClassesAndInterfaces.Interfaces;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Query
{
    /// <summary>
    /// Match data import manager to other databases
    /// </summary>
    [Serializable]
    [DataContract]
    public class MatchDataImportManager : GenericDataImportManager
    {
        private IGenericObjectRepository<ObjectBase> _origin = null;
        private IGenericObjectRepository<ObjectBase> _target = null;
        private ISQLUIQuery _loopquery = null;
        public MatchDataImportManager()
        {
        }
        /// <summary>
        /// IUIIdentifier: Element name
        /// </summary>
        [XmlIgnore]
        public override string FriendlyName
        {
            get
            {
                return NAME_MatchDataImportManager;
            }
            set { }
        }
        /// <summary>
        /// IUIIdentifier: Element description
        /// </summary>
        [XmlIgnore]
        public override string FriendlyDescription
        {
            get
            {
                return DESCRIPTION_MatchDataImportManager;
            }
            set { }
        }
        /// <summary>
        /// IDataImportManager: Local repository to create objects
        /// </summary>
        [XmlIgnore]
        public override IGenericObjectRepository Repository
        {
            get
            {
                return _origin;
            }
            set
            {
                if (!value.TryAs<ObjectBase>(out _origin))
                {
                    throw new ArgumentException(ERR_BADREPOSITORYTYPE);
                }
                _loopquery = null;
            }
        }
        /// <summary>
        /// IDataImportManager: Target repository to create objects
        /// </summary>
        [XmlIgnore]
        public override IGenericObjectRepository ExportRepository
        {
            get
            {
                return _target;
            }
            set
            {
                if (!value.TryAs<ObjectBase>(out _target))
                {
                    throw new ArgumentException(ERR_BADREPOSITORYTYPE);
                }
            }
        }
        /// <summary>
        /// IDataImportManager: Data row validation
        /// </summary>
        /// <param name="index">
        /// Data row index
        /// </param>
        /// <param name="cindex">
        /// Database connection index
        /// </param>
        /// <returns>
        /// Error message or null if data is valid
        /// </returns>
        public override string ValidateRow(int index, int cindex = 0)
        {
            return null;
        }
        /// <summary>
        /// IDataImportManager: Data row validation
        /// </summary>
        /// <param name="index">
        /// Data row index
        /// </param>
        /// <param name="cindex">
        /// Database connection index
        /// </param>
        /// <returns>
        /// Error message or null if data is valid
        /// </returns>
        public override async Task<string> ValidateRowAsync(int index, int cindex = 0)
        {
            await Task.Yield();
            return null;
        }
        /// <summary>
        /// IDataImportManager: Data row import
        /// </summary>
        /// <param name="index">
        /// Data row index
        /// </param>
        /// <param name="cindex">
        /// Database connection index
        /// </param>
        /// <returns>
        /// Error message or null if the operation succeeded
        /// </returns>
        public override string ImportRow(int index, int cindex = 0)
        {
            return null;
        }
        /// <summary>
        /// IDataImportManager: Importación de una fila de datos / 
        /// IDataImportManager: Data row import
        /// </summary>
        /// <param name="index">
        /// Índice de la fila de datos / 
        /// Data row index
        /// </param>
        /// <param name="cindex">
        /// Índice de la conexión conla base de datos / 
        /// Database connection index
        /// </param>
        /// <returns>
        /// Mensaje de error o null si la importación es correcta / 
        /// Error message or null if the operation succeeded
        /// </returns>
        public override async Task<string> ImportRowAsync(int index, int cindex = 0)
        {
            int contarget = -1;
            try
            {
                _results = false;
                contarget = _target.GetFreeConnection();
                ulong m = Convert.ToUInt64(Data.Rows[index]["cod_match"]);
                Match match = _origin.CreateObject(typeof(Match)) as Match;
                await match.FastLoad(m, cindex);
                Match nmatch = _target.CreateObject(match) as Match;
                _target.Connector.OpenConnection(cindex);
                _target.Connector.BeginTransaction(contarget);
                if (!Duplicates)
                {
                    if (!await nmatch.Duplicated(contarget, _loopquery))
                    {
                        _loopquery = await nmatch.LoopInsert(contarget, _loopquery);
                        _target.Connector.Commit(contarget);
                        ExportedCount++;
                    }
                }
                else
                {
                    _loopquery = await nmatch.LoopInsert(contarget, _loopquery);
                    _target.Connector.Commit(contarget);
                    ExportedCount++;
                }
                return null;
            }
            catch (Exception ex)
            {
                _target.Connector.Rollback(contarget);
                return ex.Message;
            }
            finally
            {
                if (contarget >= 0)
                {
                    _target.ReleaseConnection(contarget);
                }
            }
        }
    }
}
