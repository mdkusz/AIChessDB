using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// User interface for managing additional filter columns in a query grid control.
    /// </summary>
    public partial class ColumnManagerUI : UserControl, IQueryGridColumnManager
    {
        protected Dictionary<string, List<QueryColumn>> _columnsByCategory = new Dictionary<string, List<QueryColumn>>();
        protected Dictionary<string, IFilterEditor> _customEditors = null;
        protected Dictionary<string, IValueListProvider> _customValueProviders = null;
        protected Point _ptRClick;
        public ColumnManagerUI()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Standardized column manager to interoperate with the client grid control.
        /// </summary>
        [Browsable(false)]
        public IQueryGridColumnManager ColumnManager { get; set; }
        /// <summary>
        /// IQueryGridColumnManager: Available columns to operate on
        /// </summary>
        public List<QueryColumn> AvailableColumns
        {
            get
            {
                List<QueryColumn> columns = new List<QueryColumn>();
                foreach (List<QueryColumn> lc in _columnsByCategory.Values)
                {
                    columns.AddRange(lc);
                }
                return columns;
            }
        }
        /// <summary>
        /// IQueryGridColumnManager: Object that manages the results filter and order
        /// </summary>
        public IQueryResultsManager ResultsManager { get; set; }
        /// <summary>
        /// IQueryGridColumnManager: Categories list to group columns
        /// </summary>
        /// <remarks>
        /// Can be null if no categories are defined
        /// </remarks>
        public List<string> Categories
        {
            get
            {
                return new List<string>(_columnsByCategory.Keys);
            }
        }
        /// <summary>
        /// IQueryGridColumnManager: Custom column filter editors dictionary
        /// </summary>
        public Dictionary<string, IFilterEditor> CustomFilterEditors { get { return _customEditors; } }
        /// <summary>
        /// IQueryGridColumnManager: Custom column value providers dictionary
        /// </summary>
        public Dictionary<string, IValueListProvider> CustomValueProviders { get { return _customValueProviders; } }
        /// <summary>
        /// IQueryGridColumnManager: Add a custom filter editor for a column
        /// </summary>
        /// <param name="columnUID">
        /// Column unique identifier
        /// </param>
        /// <param name="editor">
        /// Custom filter editor
        /// </param>
        public void AddColumnFilterCustomEditor(string columnUID, IFilterEditor editor)
        {
            if (_customEditors == null)
            {
                _customEditors = new Dictionary<string, IFilterEditor>();
            }
            _customEditors[columnUID] = editor;
        }
        /// <summary>
        /// IQueryGridColumnManager: Add a custom value provider for a column
        /// </summary>
        /// <param name="columnUID">
        /// Column unique identifier
        /// </param>
        /// <param name="vprovider">
        /// Custom value provider
        /// </param>
        public void AddColumnValueProvider(string columnUID, IValueListProvider vprovider)
        {
            if (_customValueProviders == null)
            {
                _customValueProviders = new Dictionary<string, IValueListProvider>();
            }
            _customValueProviders[columnUID] = vprovider;
        }
        /// <summary>
        /// IQueryGridColumnManager: Extracts columns from a category
        /// </summary>
        /// <param name="category">
        /// Category name
        /// </param>
        /// <returns>
        /// Category columns list
        /// </returns>
        public List<QueryColumn> GetColumnsByCategory(string category)
        {
            if (string.IsNullOrEmpty(category) || !_columnsByCategory.ContainsKey(category))
            {
                return null;
            }
            return _columnsByCategory[category];
        }
        /// <summary>
        /// IQueryGridColumnManager: Add a list of columns to a category
        /// </summary>
        /// <param name="category">
        /// Category name
        /// </param>
        /// <param name="columns">
        /// Columns list to add
        /// </param>
        public void AddColumnsToCategory(string category, List<QueryColumn> columns)
        {
            if (string.IsNullOrEmpty(category) || columns == null || columns.Count == 0)
            {
                return;
            }
            if (!_columnsByCategory.ContainsKey(category))
            {
                _columnsByCategory[category] = new List<QueryColumn>();
            }
            _columnsByCategory[category].AddRange(columns);
        }
        /// <summary>
        /// IQueryGridColumnManager: Build the user interface to operate on columns
        /// </summary>
        public void BuildUI()
        {
            foreach (string category in _columnsByCategory.Keys)
            {
                List<QueryColumn> columns = _columnsByCategory[category];
                ListViewGroup group = new ListViewGroup(category, HorizontalAlignment.Left);
                lvColumns.Groups.Add(group);
                foreach (QueryColumn col in columns)
                {
                    ListViewItem item = new ListViewItem(col.Caption, group)
                    {
                        Tag = col
                    };
                    lvColumns.Items.Add(item);
                }
            }
        }
        /// <summary>
        /// IQueryGridColumnManager: Add a context menu option to operate on a column
        /// </summary>
        /// <param name="text">
        /// Context menu option text
        /// </param>
        /// <param name="description">
        /// Context menu option description
        /// </param>
        /// <param name="handler">
        /// Event handler that will be executed when the context menu option is selected
        /// </param>
        public void AddEventHandler(string text, string description, ColumnOperationEventHandler handler)
        {
            ColumnToolStripMenuItem cmop = new ColumnToolStripMenuItem(handler)
            {
                AccessibleRole = AccessibleRole.MenuItem,
                AccessibleName = text,
                AccessibleDescription = description,
                Name = "op" + ctxMenu.Items.Count.ToString(),
                Text = text,
                ToolTipText = description
            };
            cmop.Click += new EventHandler(ctxMenu_opNClick);
            ctxMenu.Items.Add(cmop);
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            int maxwidth = 0;
            using (Graphics gr = Graphics.FromHwnd(lvColumns.Handle))
            {
                foreach (ListViewItem item in lvColumns.Items)
                {
                    maxwidth = Math.Max(maxwidth, (int)Math.Ceiling(gr.MeasureString(item.Text, item.Font).Width + SystemInformation.VerticalScrollBarWidth));
                }
                foreach (ListViewGroup group in lvColumns.Groups)
                {
                    maxwidth = Math.Max(maxwidth, (int)Math.Ceiling(gr.MeasureString(group.Header, SystemFonts.CaptionFont).Width + SystemInformation.VerticalScrollBarWidth));
                }
            }
            return new Size(maxwidth, proposedSize.Height);
        }
        private void ctxMenu_opNClick(object sender, EventArgs e)
        {
            try
            {
                if ((_ptRClick.X >= 0) && (_ptRClick.Y >= 0))
                {
                    ListViewHitTestInfo info = lvColumns.HitTest(_ptRClick.X, _ptRClick.Y);
                    ColumnToolStripMenuItem mitem = sender as ColumnToolStripMenuItem;
                    if ((info.Item != null) && (mitem != null))
                    {
                        mitem.FireColumnOperation(this, info.Item.Tag as QueryColumn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ctxMenu_Opening(object sender, CancelEventArgs e)
        {
            if ((_ptRClick.X >= 0) && (_ptRClick.Y >= 0))
            {
                ListViewHitTestInfo info = lvColumns.HitTest(_ptRClick.X, _ptRClick.Y);
                if (info.Item == null)
                {
                    e.Cancel = true;
                }
            }
        }

        private void lvColumns_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                _ptRClick = e.Location;
                ctxMenu.Show(lvColumns, e.Location);
            }
        }
    }
    /// <summary>
    /// ToolStripMenuItem specialized to handle column operations in the query grid control.
    /// </summary>
    public class ColumnToolStripMenuItem : ToolStripMenuItem
    {
        public ColumnToolStripMenuItem(ColumnOperationEventHandler handler) : base()
        {
            ColumnOperation = handler;
        }
        /// <summary>
        /// Fire the column operation event with the sender and column information.
        /// </summary>
        /// <param name="sender">
        /// Object that is the source of the event, typically the control that contains this menu item.
        /// Use null to use the current menu item instance as the sender.
        /// </param>
        /// <param name="column">
        /// QueryColumn instance that is being operated on.
        /// </param>
        public void FireColumnOperation(object sender, QueryColumn column)
        {
            ColumnOperation?.Invoke(sender ?? this, new ColumnOperationEventArgs(column));
        }
        private ColumnOperationEventHandler ColumnOperation = null;
    }
}
