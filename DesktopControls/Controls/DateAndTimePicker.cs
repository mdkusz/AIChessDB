using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls
{
    /// <summary>
    /// Editor de fecha y hora / 
    /// Date and time editor
    /// </summary>
    public partial class DateAndTimePicker : UserControl
    {
        public DateAndTimePicker()
        {
            InitializeComponent();
            dtpDate.Value = DateTime.Now;
            dtpTime.Value = DateTime.Now;
            AutoSize = true;
        }
        /// <summary>
        /// Colores de los controles de fecha y hora / 
        /// Date and time controls colors
        /// </summary>
        public Color DateBackColor
        {
            get
            {
                return dtpDate.BackColor;
            }
            set
            {
                dtpDate.BackColor = value;
            }
        }
        public Color TimeBackColor
        {
            get
            {
                return dtpTime.BackColor;
            }
            set
            {
                dtpTime.BackColor = value;
            }
        }
        public Color DateForeColor
        {
            get
            {
                return dtpDate.ForeColor;
            }
            set
            {
                dtpDate.ForeColor = value;
            }
        }
        public Color TimeForeColor
        {
            get
            {
                return dtpTime.ForeColor;
            }
            set
            {
                dtpTime.ForeColor = value;
            }
        }
        /// <summary>
        /// Evento de cambio del valor / 
        /// ValueSet changed event
        /// </summary>
        public event EventHandler DateTimeChanged;
        /// <summary>
        /// Control que muestra la fecha / 
        /// Date control
        /// </summary>
        public DateTimePicker DatePicker
        {
            get
            {
                return dtpDate;
            }
        }
        /// <summary>
        /// Activar o desactivar la fecha / 
        /// Enable or disable date
        /// </summary>
        public bool DateChecked
        {
            get
            {
                return dtpDate.Checked;
            }
            set
            {
                dtpDate.Checked = value;
            }
        }
        /// <summary>
        /// Activar o desactivar la hora / 
        /// Enable or disable time
        /// </summary>
        public bool TimeChecked
        {
            get
            {
                return dtpTime.Checked;
            }
            set
            {
                dtpTime.Checked = value;
            }
        }
        /// <summary>
        /// Valor con fecha y hora / 
        /// ValueSet with date and time
        /// </summary>
        public DateTime DateAndTime
        {
            get
            {
                if (dtpDate.Checked && dtpTime.Checked)
                {
                    return new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day,
                        dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second);
                }
                if (dtpTime.Checked)
                {
                    return Time;
                }
                return Date;
            }
            set
            {
                dtpDate.Value = value;
                dtpTime.Value = value;
                Width = dtpDate.Width + dtpTime.Width;
            }
        }
        /// <summary>
        /// Valor con fecha y hora que admite valores nulos / 
        /// ValueSet with date and time with null values
        /// </summary>
        public DateTime? NullableDateAndTime
        {
            get
            {
                if (dtpDate.Checked && dtpTime.Checked)
                {
                    return new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day,
                        dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second);
                }
                if (dtpTime.Checked)
                {
                    return Time;
                }
                if (dtpDate.Checked)
                {
                    return Date;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    dtpDate.Value = value.Value;
                    dtpTime.Value = value.Value;
                }
                else
                {
                    dtpDate.Checked = false;
                    dtpTime.Checked = false;
                }
                Width = dtpDate.Width + dtpTime.Width;
            }
        }
        /// <summary>
        /// Valor con fecha solo / 
        /// Only date value
        /// </summary>
        public DateTime Date
        {
            get
            {
                return dtpDate.Value.Date;
            }
            set
            {
                dtpDate.Value = value.Date;
                dtpTime.Checked = false;
                Width = dtpDate.Width;
            }
        }
        /// <summary>
        /// Valor con fecha solo que admite valores nulos / 
        /// Only date value with nulls
        /// </summary>
        public DateTime? NullableDate
        {
            get
            {
                if (dtpDate.Checked)
                {
                    return dtpDate.Value.Date;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    dtpDate.Checked = true;
                    dtpDate.Value = value.Value.Date;
                }
                else
                {
                    dtpDate.Checked = false;
                }
                dtpTime.Checked = false;
                Width = dtpDate.Width;
            }
        }
        /// <summary>
        /// Valor con hora sola / 
        /// Time value only
        /// </summary>
        public DateTime Time
        {
            get
            {
                return new DateTime(1, 1, 1, dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second);
            }
            set
            {
                dtpDate.Checked = false;
                dtpTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    value.Hour, value.Minute, value.Second);
                Width = dtpTime.Width;
            }
        }
        /// <summary>
        /// Valor con hora sola que admite valores nulos / 
        /// Time value only with nulls
        /// </summary>
        public DateTime? NullableTime
        {
            get
            {
                if (dtpTime.Checked)
                {
                    return new DateTime(1, 1, 1, dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second);
                }
                return null;
            }
            set
            {
                dtpDate.Checked = false;
                if (value != null)
                {
                    dtpTime.Checked = true;
                    dtpTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                        value.Value.Hour, value.Value.Minute, value.Value.Second);
                }
                else
                {
                    dtpTime.Checked = false;
                }
                Width = dtpTime.Width;
            }
        }
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDate.Checked)
            {
                OnValueChanged(e);
                DateTimeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void dtpTime_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTime.Checked)
            {
                OnValueChanged(e);
                DateTimeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private void dtpDate_FormatChanged(object sender, EventArgs e)
        {
            using (Graphics gr = Graphics.FromHwnd(dtpDate.Handle))
            {
                Size szd = new Size((int)gr.MeasureString(dtpDate.Text, dtpDate.Font).Width + 40, dtpDate.Size.Height);
                if ((dtpDate.Size.Width != szd.Width) ||
                    (dtpDate.Size.Height != szd.Height))
                {
                    dtpDate.Size = szd;
                }
            }
        }

        private void dtpDate_FontChanged(object sender, EventArgs e)
        {
            dtpDate_FormatChanged(sender, e);
        }

        private void dtpTime_FontChanged(object sender, EventArgs e)
        {
            using (Graphics gr = Graphics.FromHwnd(dtpTime.Handle))
            {
                Size szt = new Size((int)gr.MeasureString("00:00:00", dtpTime.Font).Width + 40, dtpTime.Size.Height);
                if ((dtpTime.Size.Width != szt.Width) ||
                    (dtpTime.Size.Height != szt.Height))
                {
                    dtpTime.Size = szt;
                }
            }
        }
        private void dtpDate_SizeChanged(object sender, EventArgs e)
        {
            Size = new Size(dtpDate.Width + dtpTime.Width, Math.Max(dtpDate.Height, dtpTime.Height));
        }

        private void dtpTime_SizeChanged(object sender, EventArgs e)
        {
            Size = new Size(dtpDate.Width + dtpTime.Width, Math.Max(dtpDate.Height, dtpTime.Height));
        }
        protected virtual void OnValueChanged(EventArgs e)
        {
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            Size sz = base.GetPreferredSize(proposedSize);
            dtpDate_FontChanged(dtpDate, EventArgs.Empty);
            dtpTime_FontChanged(dtpTime, EventArgs.Empty);
            return new Size(dtpDate.Width + dtpTime.Width, Math.Max(dtpDate.Height, dtpTime.Height));
        }
    }
}
