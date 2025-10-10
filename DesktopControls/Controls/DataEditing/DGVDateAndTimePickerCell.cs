using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls.DataEditing
{
    public class DGVDateAndTimePickerCell : DataGridViewCell
    {
        public DGVDateAndTimePickerCell() : base() { }
        [Browsable(false)]
        public override Type EditType
        {
            get
            {
                return typeof(DGVDateAndTimePicker);
            }
        }
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue,
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            DGVDateAndTimePickerColumn col = OwningColumn as DGVDateAndTimePickerColumn;
            if ((col != null) && (OwningRow != null))
            {
                DGVDateAndTimePicker ctl = DataGridView.EditingControl as DGVDateAndTimePicker;
                ctl.EditingControlDataGridView = DataGridView;
                ctl.EditingControlRowIndex = RowIndex;
                ctl.EditingControlColumnIndex = ColumnIndex;
                ctl.NullableDateAndTime = Value as DateTime?;
            }
        }
        public override Type ValueType
        {
            get
            {
                return typeof(DateTime?);
            }
        }
        public override Type FormattedValueType
        {
            get
            {
                return typeof(string);
            }
        }
        public override DataGridViewCellStyle GetInheritedStyle(DataGridViewCellStyle inheritedCellStyle, int rowIndex, bool includeColors)
        {
            if (rowIndex < 0)
            {
                return inheritedCellStyle;
            }
            DataGridViewCellStyle style = base.GetInheritedStyle(inheritedCellStyle, rowIndex, includeColors);
            if (!(style is DGVDateAndTimePickerCellStyle))
            {
                style = OwningColumn.DefaultCellStyle;
            }
            return style;
        }
        public override object DefaultNewRowValue
        {
            get
            {
                return DateTime.Now;
            }
        }
        public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
        {
            DGVDateAndTimePickerColumn col = OwningColumn as DGVDateAndTimePickerColumn;
            if ((col != null) && (OwningRow != null) && (formattedValue is string))
            {
                DateTime dt;
                if (DateTime.TryParse(formattedValue.ToString(), out dt))
                {
                    return dt;
                }
            }
            return null;
        }
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
            object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            if (cellStyle != null)
            {
                if ((paintParts & DataGridViewPaintParts.Background) ==
                    DataGridViewPaintParts.Background)
                {
                    using (Brush br = new SolidBrush(cellStyle.BackColor))
                    {
                        graphics.FillRectangle(br, cellBounds);
                    }
                }
                if ((paintParts & DataGridViewPaintParts.Border) ==
                    DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }
                if (formattedValue != null)
                {
                    SizeF sz = graphics.MeasureString(formattedValue.ToString(), cellStyle.Font ?? DataGridView.Font);
                    using (Brush br = new SolidBrush(cellStyle.ForeColor))
                    {
                        graphics.DrawString(formattedValue.ToString(), cellStyle.Font ?? DataGridView.Font, br, cellBounds.Left + cellStyle.Padding.Left, cellBounds.Top + cellStyle.Padding.Top + (cellBounds.Height - sz.Height) / 2);
                    }
                }
            }
        }
        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            DGVDateAndTimePicker ctl = DataGridView.EditingControl as DGVDateAndTimePicker;
            if ((ctl != null) && (ctl.EditingControlRowIndex == RowIndex) && (ctl.EditingControlColumnIndex == ColumnIndex))
            {
                return ctl.PreferredSize;
            }
            if ((RowIndex >= 0) && (FormattedValue != null) && !string.IsNullOrEmpty(FormattedValue.ToString()))
            {
                SizeF sz = graphics.MeasureString(FormattedValue.ToString(), cellStyle.Font ?? DataGridView.Font);
                return new Size((int)sz.Width, (int)sz.Height);
            }
            return base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
        }
        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            if (value != null)
            {
                return value.ToString();
            }
            return value;
        }
        protected override bool SetValue(int rowIndex, object value)
        {
            if (rowIndex < 0)
            {
                return false;
            }
            return base.SetValue(rowIndex, value);
        }
    }
}
