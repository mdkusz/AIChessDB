using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.UserInterface;
using GlobalCommonEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;
using static BaseClassesAndInterfaces.Properties.Resources;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Specialized user control for editing position filters.
    /// </summary>
    public partial class PositionFilter : UserControl, IFilterEditor
    {
        private const string cfilterseprator = "<#psep$>";
        private string _initialBoard = "";
        private string _initialCustom = "";
        private Rectangle _initialSelection = new Rectangle(0, 0, 8, 8);
        private UIFilterExpression _efilter = null;
        private bool _regex;
        private static List<StoredBoard> _boards = null;
        public PositionFilter()
        {
            InitializeComponent();
            label1.Text = LAB_DESCRIPTION;
            label2.Text = LAB_CUSTOM;
            lStored.Text = LAB_STOREDBOARDS;
            if (_boards == null)
            {
                _boards = new List<StoredBoard>();
            }
            else
            {
                foreach (StoredBoard sb in _boards)
                {
                    cbBoards.Items.Add(sb);
                }
            }
        }
        /// <summary>
        /// IFilterEditor: Wrong values color
        /// </summary>
        public Color ValueErrorColor { get; set; }
        /// <summary>
        /// IFilterEditor: Correct values color
        /// </summary>
        public Color ValueOKColor { get; set; }
        /// <summary>
        /// IFilterEditor: Object to apply a filter
        /// </summary>
        public SQLElement Element { get; set; }
        /// <summary>
        /// IFilterEditor: Field name in the user interface
        /// </summary>
        public string FieldUIName { get; set; }
        /// <summary>
        /// IFilterEditor: Filter operator list
        /// </summary>
        public List<IUIIdentifier> FilterOperators { get; set; }
        /// <summary>
        /// IFilterEditor: Filter expression
        /// </summary>
        public UIFilterExpression FilterCondition
        {
            get
            {
                return _efilter;
            }
            set
            {
                _efilter = value;
                if (_efilter != null)
                {
                    string[] parts = _efilter.CustomFormattedValue.Split(new string[] { cfilterseprator }, StringSplitOptions.None);
                    string[] ritems = parts[0].Split(',');
                    _initialSelection = new Rectangle(int.Parse(ritems[0]), int.Parse(ritems[1]), int.Parse(ritems[2]), int.Parse(ritems[3]));
                    _initialBoard = parts[1];
                    _initialCustom = parts[2];
                    cbBoard.Selection = _initialSelection;
                    txtCustom.Text = _initialCustom;
                    if (string.IsNullOrEmpty(txtCustom.Text))
                    {
                        cbBoard.Expression = _initialBoard;
                        cbBoard.Selection = _initialSelection;
                        _regex = !string.IsNullOrEmpty(parts[3]);
                        if (!_regex)
                        {
                            cbSimpleBoard.Board = _initialBoard;
                        }
                    }
                    txtDescription.Text = parts[4];
                }
            }
        }
        /// <summary>
        /// IFilterEditor: The editor is visible to the user
        /// </summary>
        public bool FilterEditorVisible { get; set; }
        /// <summary>
        /// IFilterEditor: Filter operators provider
        /// </summary>
        public ISQLElementProvider ElementProvider { get; set; }
        /// <summary>
        /// IFilterEditor: Allowed values list provider
        /// </summary>
        public IValueListProvider ValueListProvider { get; set; }
        /// <summary>
        /// IFilterEditor: Evento para indicar que el usuario ha aceptado el filtro / 
        /// IFilterEditor: Event to infom that the user has accepted the filter
        /// </summary>
        public event EventHandler FilterAccepted;
        /// <summary>
        /// IFilterEditor: Evento para indicar que el usuario ha cancelado el filtro / 
        /// IFilterEditor: Event to inform that the user has cancelled the filter
        /// </summary>
        public event EventHandler FilterCancelled;
        /// <summary>
        /// Build the filter expression using the current user selected values.
        /// </summary>
        private void BuildFilter()
        {
            LogicOperator lop = ElementProvider.SQLElement(typeof(LogicOperator), new[] { cbSimple.Checked ? TK_3D : TK_regexp_like }) as LogicOperator;
            lop.IgnoreCase = false;
            string lboard = cbSimple.Checked ? cbSimpleBoard.Board :
                (string.IsNullOrEmpty(txtCustom.Text) ? cbBoard.Expression : txtCustom.Text);
            UIFilterExpression fexpr = lop.BuildExpression(Element as ISQLTypedElement, new List<object> { lboard }, ElementProvider);
            List<string> cexprparts = new List<string>();
            if (cbSimple.Checked)
            {
                cexprparts.Add("0,0,8,8");
                cexprparts.Add(cbSimpleBoard.Board);
                cexprparts.Add("");
                cexprparts.Add("");
            }
            else
            {
                cexprparts.Add($"{cbBoard.Selection.X},{cbBoard.Selection.Y},{cbBoard.Selection.Width},{cbBoard.Selection.Height}");
                cexprparts.Add(cbBoard.Expression);
                cexprparts.Add(txtCustom.Text);
                if (string.IsNullOrEmpty(txtCustom.Text))
                {
                    cexprparts.Add(cbBoard.Regex ? "1" : "");
                }
                else
                {
                    cexprparts.Add("1");
                }
            }
            cexprparts.Add(txtDescription.Text);
            fexpr.CustomFormattedValue = string.Join(cfilterseprator, cexprparts);
            fexpr.FriendlyName = txtDescription.Text;
            _efilter = fexpr;
        }
        private void cbSimple_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSimple.Checked)
            {
                if (!cbBoard.Regex)
                {
                    cbSimpleBoard.Board = cbBoard.Expression;
                }
                else
                {
                    cbSimpleBoard.Clear();
                }
                txtCustom.Clear();
                txtCustom.Enabled = false;
            }
            else
            {
                cbBoard.Expression = cbSimpleBoard.Board;
                cbBoard.Selection = new Rectangle(0, 0, 8, 8);
                txtCustom.Enabled = true;
            }
            cbBoard.Visible = !cbSimple.Checked;
            cbSimpleBoard.Visible = cbSimple.Checked;
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            try
            {
                cbBoard.Clear();
                cbSimpleBoard.Clear();
                txtCustom.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtCustom.Text = _initialCustom;
                cbBoard.Selection = _initialSelection;
                if (string.IsNullOrEmpty(_initialCustom))
                {
                    cbBoard.Expression = string.IsNullOrEmpty(_initialBoard) ? INITIAL_BOARD : _initialBoard;
                    if (!cbBoard.Regex)
                    {
                        cbSimpleBoard.Board = string.IsNullOrEmpty(_initialBoard) ? INITIAL_BOARD : _initialBoard;
                    }
                }
                else
                {
                    cbSimple.Checked = false;
                    cbBoard.Expression = "";
                    cbSimpleBoard.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            bSave.Enabled = !string.IsNullOrEmpty(txtDescription.Text);
        }

        private void cbBoards_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cbBoards.SelectedItem != null) && (cbBoards.SelectedItem is StoredBoard))
            {
                bUse.Enabled = !((cbBoards.SelectedItem as StoredBoard).Regex && cbSimple.Checked);
            }
            else
            {
                bUse.Enabled = false;
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                StoredBoard sb = new StoredBoard()
                {
                    Name = txtDescription.Text
                };
                if (cbSimple.Checked)
                {
                    sb.Selection = new Rectangle(0, 0, 8, 8);
                    sb.Board = cbSimpleBoard.Board;
                    sb.Custom = "";
                    sb.Regex = false;
                }
                else
                {
                    sb.Selection = cbBoard.Selection;
                    sb.Board = cbBoard.Expression;
                    sb.Custom = txtCustom.Text;
                    sb.Regex = cbBoard.Regex;
                }
                if (_boards.Contains(sb))
                {
                    _boards[_boards.IndexOf(sb)] = sb;
                }
                else
                {
                    _boards.Add(sb);
                    cbBoards.Items.Add(sb);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bUse_Click(object sender, EventArgs e)
        {
            try
            {
                StoredBoard sb = cbBoards.SelectedItem as StoredBoard;
                if (sb != null)
                {
                    cbBoard.Selection = sb.Selection;
                    cbBoard.Expression = sb.Board;
                    txtCustom.Text = sb.Custom;
                    if (string.IsNullOrEmpty(txtDescription.Text))
                    {
                        txtDescription.Text = sb.Name;
                    }
                    if (!sb.Regex)
                    {
                        cbSimpleBoard.Board = sb.Board;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCustom_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCustom.Text))
            {
                cbBoard.Expression = "";
                cbSimple.Checked = false;
                cbSimple.Enabled = false;
                cbSimpleBoard.Clear();
            }
            else
            {
                cbSimple.Enabled = true;
            }
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            try
            {
                BuildFilter();
                Form frmc = Parent as Form;
                if (frmc != null)
                {
                    frmc.Controls.Remove(this);
                    if (frmc.Modal)
                    {
                        frmc.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        FilterAccepted?.Invoke(this, EventArgs.Empty);
                        frmc.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Form frmc = Parent as Form;
            if (frmc != null)
            {
                frmc.Controls.Remove(this);
                if (frmc.Modal)
                {
                    frmc.DialogResult = DialogResult.Cancel;
                }
                else
                {
                    FilterCancelled?.Invoke(this, EventArgs.Empty);
                    frmc.Close();
                }
            }
        }

        private void PositionFilter_Load(object sender, EventArgs e)
        {
            try
            {
                cbBoard.Clear();
                cbSimpleBoard.Clear();
                txtCustom.Clear();
            }
            catch
            {
            }
        }
    }
    /// <summary>
    /// POCO class for storing board information.
    /// </summary>
    internal class StoredBoard : IEquatable<StoredBoard>, IComparable<StoredBoard>
    {
        public StoredBoard()
        {
        }
        /// <summary>
        /// Board configuration in FEN or similar format.
        /// </summary>
        public string Board { get; set; }
        /// <summary>
        /// Custom regular expression representing the board.
        /// </summary>
        public string Custom { get; set; }
        /// <summary>
        /// Flag to indicate if the custom expression is a regular expression.
        /// </summary>
        public bool Regex { get; set; }
        /// <summary>
        /// Board selection rectangle, in squares.
        /// </summary>
        public Rectangle Selection { get; set; }
        /// <summary>
        /// Name of this stored board.
        /// </summary>
        public string Name { get; set; }
        public int CompareTo(StoredBoard other)
        {
            return Name.CompareTo(other.Name);
        }
        public bool Equals(StoredBoard other)
        {
            return Name.Equals(other);
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
