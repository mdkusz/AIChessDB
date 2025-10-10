using System;
using System.Windows.Forms;

namespace DesktopControls.Controls.DataEditing
{
    public class DGVDateAndTimePicker : DateAndTimePicker, IDataGridViewEditingControl
    {
        public DGVDateAndTimePicker()
        {
        }
        public object EditingControlFormattedValue
        {
            get
            {
                return DateAndTime.ToString();
            }
            set
            {
                if (value is string)
                {
                    DateTime dt;
                    if (DateTime.TryParse((string)value, out dt))
                    {
                        DateAndTime = dt;
                    }
                    else
                    {
                        NullableDateAndTime = null;
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
            DateForeColor = dataGridViewCellStyle.ForeColor;
            TimeForeColor = dataGridViewCellStyle.ForeColor;
            DateBackColor = dataGridViewCellStyle.BackColor;
            TimeBackColor = dataGridViewCellStyle.BackColor;
            DGVDateAndTimePickerCellStyle dtstyle = dataGridViewCellStyle as DGVDateAndTimePickerCellStyle;
            if (dtstyle != null)
            {
                DatePicker.Format = dtstyle.DateFormat;
                if (dtstyle.DateFormat == DateTimePickerFormat.Custom)
                {
                    DatePicker.CustomFormat = dtstyle.CustomFormat;
                }
            }
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

        protected override void OnValueChanged(EventArgs eventargs)
        {
            if (EditingControlDataGridView != null)
            {
                EditingControlValueChanged = true;
                EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnValueChanged(eventargs);
            }
        }
    }
}
