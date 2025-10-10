using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.UserInterface;
using GlobalCommonEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Control to select squares in a chess board to build a query filter.
    /// </summary>
    public partial class SquareFilter : UserControl, IFilterEditor
    {
        private UIFilterExpression _efilter;
        private string _selection = new string(' ', 64);
        private bool _color = true;
        private bool _side = true;
        public SquareFilter()
        {
            InitializeComponent();
            lFilter.Text = "";
            bOK.Text = BTN_OK;
            bCancel.Text = BTN_CANCEL;
            lSquare.Text = LAB_Square;
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
                    _selection = _efilter.CustomFormattedValue;
                    DrawBoard();
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
        /// Refresh the filter operators list and size it
        /// </summary>
        private void RefreshFilterOperators()
        {
            if (Element != null)
            {
                if (FilterOperators != null)
                {
                    // Calculate the opeator dropdown list size
                    using (Graphics gr = Graphics.FromHwnd(cbOperators.Handle))
                    {
                        int maxwidth = 0;
                        foreach (IUIIdentifier op in FilterOperators)
                        {
                            LogicOperator lop = op.Implementation() as LogicOperator;
                            if (lop != null)
                            {
                                switch (lop.Operator)
                                {
                                    case LogicOperator.LogicOperators.NonStandard:
                                    case LogicOperator.LogicOperators.Null:
                                    case LogicOperator.LogicOperators.BetweenSymmetric:
                                    case LogicOperator.LogicOperators.BetweenAsymmetric:
                                    case LogicOperator.LogicOperators.Exists:
                                    case LogicOperator.LogicOperators.Not:
                                    case LogicOperator.LogicOperators.BitAnd:
                                    case LogicOperator.LogicOperators.BitXor:
                                        continue;
                                }
                            }
                            cbOperators.Items.Add(op);
                            maxwidth = Math.Max((int)gr.MeasureString(op.ToString(), cbOperators.Font).Width, maxwidth);
                        }
                        cbOperators.Width = maxwidth + SystemInformation.VerticalScrollBarWidth;
                    }
                }
                if (FilterCondition != null)
                {
                    LogicOperator lop = ElementProvider.SQLElement(typeof(LogicOperator)) as LogicOperator;
                    lop = lop.ExtractOperator(FilterCondition, ElementProvider);
                    // Current filter expression values
                    if (lop != null)
                    {
                        SQLElement op = lop as SQLElement;
                        for (int ix = 0; ix < cbOperators.Items.Count; ix++)
                        {
                            if (op.Equals(cbOperators.Items[ix] as SQLElement))
                            {
                                cbOperators.SelectedIndex = ix;
                                break;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Draw board image.
        /// </summary>
        private void DrawBoard()
        {
            Bitmap board = new Bitmap(164, 164);
            Graphics gr = Graphics.FromImage(board);
            try
            {
                gr.FillRectangle(Brushes.Black, 0, 0, board.Width, board.Height);
                List<Rectangle> selrects = new List<Rectangle>();
                for (int rowt = 0; rowt < 8; rowt++)
                {
                    int row = _side ? rowt : (7 - rowt);
                    int rowm = _side ? (7 - rowt) : rowt;
                    bool black = (rowt & 1) != 0;
                    for (int colt = 0; colt < 8; colt++)
                    {
                        int col = _side ? colt : (7 - colt);
                        if (black)
                        {
                            gr.FillRectangle(Brushes.Gray, 2 + col * 20, 2 + row * 20, 20, 20);
                        }
                        else
                        {
                            gr.FillRectangle(Brushes.LightGray, 2 + col * 20, 2 + row * 20, 20, 20);
                        }
                        if (_selection[colt + rowt * 8] != ' ')
                        {
                            Rectangle rsel = new Rectangle(2 + col * 20, 2 + rowm * 20, 20, 20);
                            selrects.Add(rsel);
                        }
                        black = !black;
                    }
                }
                using (Pen pm = new Pen(Color.Lime, 2f))
                {
                    foreach (Rectangle r in selrects)
                    {
                        gr.DrawRectangle(pm, r);
                    }
                }
                pbBoard.Image = board.Clone() as Bitmap;
                Refresh();
            }
            finally
            {
                gr.Dispose();
                board.Dispose();
            }
        }

        private void SquareSelectionBoard_Load(object sender, EventArgs e)
        {
            RefreshFilterOperators();
            DrawBoard();
        }

        private void pbBoard_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                IUIIdentifier op = cbOperators.SelectedItem as IUIIdentifier;
                if (op != null)
                {
                    IFilterExpressionBuilder fop = op.Implementation() as IFilterExpressionBuilder;
                    int n = fop.ArgumentsCount;
                    if (n == 0)
                    {
                        return;
                    }
                    int w = (pbBoard.Width - 164) / 2;
                    int h = (pbBoard.Height - 164) / 2;
                    int col = (e.X - w) / 20;
                    col = _side ? col : (7 - col);
                    int row = (e.Y - h) / 20;
                    row = _side ? (7 - row) : row;
                    if ((col < 0) || (col > 7) || (row < 0) || (row > 7))
                    {
                        return;
                    }
                    bool ctrl = (ModifierKeys & Keys.Control) == Keys.Control;
                    int selected = _selection.Count(c => c == '1');
                    int pos = col + row * 8;
                    switch (n)
                    {
                        case 1:
                            _selection = new string(' ', 63);
                            _selection = _selection.Insert(pos, "1");
                            break;
                        case 2:
                            if (!ctrl)
                            {
                                _selection = new string(' ', 63);
                                _selection = _selection.Insert(pos, "1");
                            }
                            else if (_selection[pos] == ' ')
                            {
                                if (selected < 2)
                                {
                                    _selection = _selection.Remove(pos, 1).Insert(pos, "1");
                                }
                            }
                            else
                            {
                                _selection = _selection.Remove(pos, 1).Insert(pos, " ");
                            }
                            break;
                        case -1:
                            if (!ctrl)
                            {
                                _selection = new string(' ', 63);
                                _selection = _selection.Insert(pos, "1");
                            }
                            else if (_selection[pos] == ' ')
                            {
                                _selection = _selection.Remove(pos, 1).Insert(pos, "1");
                            }
                            else
                            {
                                _selection = _selection.Remove(pos, 1).Insert(pos, " ");
                            }
                            break;
                    }
                    List<string> lpossel = new List<string>();
                    for (int i = 0; i < _selection.Length; i++)
                    {
                        if (_selection[i] == '1')
                        {
                            int m = i + 1;
                            int r = m % 8;
                            int f = m / 8;
                            lpossel.Add((char)('a' + r) + (f + 1).ToString());
                        }
                    }
                    lFilter.Text = op.ToString() + " " + string.Join(", ", lpossel);
                    bOK.Enabled = ((n < 3) && (lpossel.Count == n)) || ((n == -1) && (lpossel.Count > 0));
                    DrawBoard();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Build the filter expression using the current user selected values.
        /// </summary>
        private void BuildFilter()
        {
            LogicOperator lop = cbOperators.SelectedItem as LogicOperator;
            List<object> lpossel = new List<object>();
            for (int i = 0; i < _selection.Length; i++)
            {
                if (_selection[i] == '1')
                {
                    lpossel.Add(i);
                }
            }
            _efilter = lop.BuildExpression(Element as ISQLTypedElement, lpossel, ElementProvider);
            _efilter.CustomFormattedValue = _selection;
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

        private void cbOperators_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _selection = new string(' ', 64);
                DrawBoard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
