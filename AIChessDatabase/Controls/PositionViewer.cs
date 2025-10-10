using AIChessDatabase.Data;
using AIChessDatabase.Dialogs;
using AIChessDatabase.Interfaces;
using AIChessDatabase.Query;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.Tools;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Control to select matches by a chess position.
    /// </summary>
    public partial class PositionViewer : UserControl
    {
        private string _position = "";
        private ChessDBQuery _query = null;
        private MasterDetailQuery _masterDetailQuery = null;
        private DataTable _results = null;
        private bool _color = true;
        private bool _side = true;

        public PositionViewer()
        {
            InitializeComponent();
            lMoves.Text = LAB_MOVES;
            lResult.Text = LAB_RESULT;
            dgMatches.FOManager = new BasicFilterAndOrderManager(FOManagerUIType.List);
        }
        /// <summary>
        /// Database connection index to use for queries.
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// Object and database repository to use for queries and object creation.
        /// </summary>
        [Browsable(false)]
        public IObjectRepository Repository { get; set; }
        /// <summary>
        /// Current board position as string.
        /// </summary>
        /// <remarks>
        /// This is a 64 character string representing the board position.
        /// 0: are for empty squares:
        /// b or B are for black and white bishops respectively,
        /// r or R are for black and white rooks respectively,
        /// n or N are for black and white knights respectively,
        /// q or Q are for black and white queens respectively,
        /// k or K are for black and white kings respectively,
        /// p or P are for black and white pawns respectively.
        /// </remarks>
        public string BoardPosition
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                bFind.Enabled = !string.IsNullOrEmpty(_position);
            }
        }
        /// <summary>
        /// Player color to search for matches: White = true, Black = false.
        /// </summary>
        public bool Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }
        /// <summary>
        /// Side to view the board: true = white side, false = black side.
        /// </summary>
        public bool Side
        {
            get
            {
                return _side;
            }
            set
            {
                _side = value;
                foreach (Control c in pBoards.Controls)
                {
                    TinyBoard tb = c as TinyBoard;
                    if ((tb != null) && (tb.SideView != _side))
                    {
                        tb.SideView = _side;
                    }
                }
            }
        }
        /// <summary>
        /// Get the selected match from the data grid.
        /// </summary>
        /// <returns>
        /// A match object with the selected match data, or null if no match is selected.
        /// </returns>
        public async Task<Match> SelectedMatch()
        {
            if (dgMatches.Grid.SelectedRows.Count > 0)
            {
                ulong m = Convert.ToUInt64(dgMatches.Grid.SelectedRows[0].Cells[0].Value);
                Match match = Repository.CreateObject(typeof(Match)) as Match;
                await match.FastLoad(m, ConnectionIndex);
                return match;
            }
            return null;
        }
        private void dgMatches_QueryChanged(object sender, EventArgs e)
        {
            try
            {
                _results = dgMatches.Grid.DataSource as DataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void dgMatches_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                bShow.Enabled = false;
                pBoards.Controls.Clear();
                if (dgMatches.Grid.SelectedRows.Count != 0)
                {
                    bShow.Enabled = true;
                    int npos = (_results.Columns.Count - 7) / 5;
                    ulong imatch = Convert.ToUInt64(dgMatches.Grid.SelectedRows[0].Cells[0].Value);
                    int nmov = Convert.ToInt32(dgMatches.Grid.SelectedRows[0].Cells[1].Value);
                    _masterDetailQuery.DetailQueries[0].Parameters[0].DefaultValue = imatch;
                    _masterDetailQuery.DetailQueries[0].Parameters[1].DefaultValue = nmov;
                    DataTable dt = await Repository.Connector.ExecuteTableAsync(_masterDetailQuery.DetailQueries[0], null, null, ConnectionIndex);
                    foreach (DataRow row in dt.Rows)
                    {
                        TinyBoard tb = new TinyBoard()
                        {
                            FromTo = new Point(Convert.ToInt32(row["move_from"]),
                                Convert.ToInt32(row["move_to"])),
                            BoardPosition = row["to_board"].ToString(),
                            SideView = Side,
                            Player = row["move_player"].ToString() == "0",
                            ANText = nmov.ToString() + "." + row["move_an_text"].ToString()
                        };
                        pBoards.Controls.Add(tb);
                        if (!tb.Player)
                        {
                            nmov++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bFind_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    bShow.Enabled = false;
                    bFind.Enabled = false;
                    UseWaitCursor = true;
                    _query = new ChessDBQuery();
                    _query.ConnectionIndex = ConnectionIndex;
                    _query.Repository = Repository;
                    string result = cbResult.SelectedItem as string;
                    if (result == "all")
                    {
                        result = "";
                    }
                    _masterDetailQuery = _query.BuildPositionQuery(result, int.Parse(txtMoves.Text), BoardPosition);
                    dgMatches.Grid.Query = _masterDetailQuery.MasterQuery;
                    dgMatches.Grid.RefreshData();
                    _results = dgMatches.Grid.DataSource as DataTable;
                    pBoards.Controls.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    bShow.Enabled = dgMatches.Grid.SelectedRows.Count != 0;
                    int nm = 0;
                    bFind.Enabled = int.TryParse(txtMoves.Text, out nm);
                    UseWaitCursor = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtMoves_TextChanged(object sender, EventArgs e)
        {
            int nm = 0;
            bFind.Enabled = int.TryParse(txtMoves.Text, out nm);
        }

        private async void bShow_Click(object sender, EventArgs e)
        {
            try
            {
                Match match = await SelectedMatch();
                if (match != null)
                {
                    _masterDetailQuery.DetailQueries[0].Parameters[0].DefaultValue = match.IdMatch;
                    _masterDetailQuery.DetailQueries[0].Parameters[1].DefaultValue = Convert.ToInt32(dgMatches.Grid.SelectedRows[0].Cells[1].Value);
                    UseWaitCursor = true;
                    DataTable dtm = await match.Repository.GlobalQuery(_masterDetailQuery.DetailQueries[0], ConnectionIndex);
                    UseWaitCursor = false;
                    DlgMatch dlg = new DlgMatch()
                    {
                        ConnectionIndex = ConnectionIndex
                    };
                    while (dtm.Columns.Count > 1)
                    {
                        dtm.Columns.RemoveAt(0);
                    }
                    dlg.SetMatch(match, dtm);
                    dlg.Show(this);
                }
            }
            catch (Exception ex)
            {
                UseWaitCursor = false;
                MessageBox.Show(ex.Message);
            }
        }
    }
}
