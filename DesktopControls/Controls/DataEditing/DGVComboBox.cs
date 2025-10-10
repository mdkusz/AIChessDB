using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls.DataEditing
{
    public class DGVComboBox : ComboBox, IDataGridViewEditingControl
    {
        protected bool _initializing;
        public DGVComboBox() : base()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }
        public void SetValue(List<IUIIdentifier> values, object value)
        {
            _initializing = true;
            Items.Clear();
            Items.AddRange(values.ToArray());
            if (value != null)
            {
                for (int ix = 0; ix < Items.Count; ix++)
                {
                    ObjectWrapper obj = Items[ix] as ObjectWrapper;
                    if (obj == null)
                    {
                        break;
                    }
                    if (obj.Implementation().Equals(value))
                    {
                        SelectedIndex = ix;
                        break;
                    }
                }
            }
            _initializing = false;
        }
        public object EditingControlFormattedValue
        {
            get
            {
                if (SelectedItem == null)
                {
                    return "";
                }
                return SelectedItem.ToString();
            }
            set
            {
                if (value is string)
                {
                    foreach (object item in Items)
                    {
                        if (item.ToString() == (string)value)
                        {
                            SelectedItem = item;
                            break;
                        }
                    }
                }
            }
        }

        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            Font = dataGridViewCellStyle.Font;
            ForeColor = dataGridViewCellStyle.ForeColor;
            BackColor = dataGridViewCellStyle.BackColor;
        }

        public int EditingControlRowIndex { get; set; }
        public int EditingControlColumnIndex { get; set; }

        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        public DataGridView EditingControlDataGridView { get; set; }

        public bool EditingControlValueChanged { get; set; }

        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (!_initializing && (EditingControlDataGridView != null))
            {
                EditingControlValueChanged = true;
                EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnSelectedIndexChanged(e);
            }
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            Size sz = base.GetPreferredSize(proposedSize);
            using (Graphics gr = Graphics.FromHwnd(Handle))
            {
                int width = 0;
                foreach (object obj in Items)
                {
                    width = Math.Max(width, 20 + (int)gr.MeasureString(obj.ToString(), Font).Width);
                }
                sz.Width = width;
            }
            return sz;
        }
    }
}
