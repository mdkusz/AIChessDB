using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DesktopControls.Tools
{
    /// <summary>
    /// Auxiliary methods for Windows Forms controls
    /// </summary>
    /// <remarks>
    /// This class is used to adjust control size and/or fit them in containers, like toolbars, listboxes, and split containers.
    /// It is just an UI tool to help in the design of Windows Forms applications.
    /// </remarks>
    public static class ControlHelpers
    {
        /// <summary>
        /// Set the width of a ComboBox in a ToolStrip to fit all items
        /// </summary>
        /// <param name="comboBox">
        /// ToolStripComboBox control
        /// </param>
        public static void SetToolBarComboBoxWidth(ToolStripComboBox comboBox)
        {
            if (comboBox.Items.Count == 0)
            {
                return;
            }
            int maxwidth = 0;
            using (Graphics gr = Graphics.FromHwnd(comboBox.Control.Handle))
            {
                foreach (object item in comboBox.Items)
                {
                    maxwidth = Math.Max(maxwidth, (int)Math.Ceiling(gr.MeasureString(item.ToString(), comboBox.Font).Width + SystemInformation.VerticalScrollBarWidth));
                }
            }
            comboBox.Width = maxwidth;
        }
        /// <summary>
        /// Set the width of a ListBox to fit all items
        /// </summary>
        /// <param name="lb">
        /// ListBox control
        /// </param>
        public static void SetListBoxWidth(ListBox lb)
        {
            if (lb.Items.Count == 0)
            {
                return;
            }
            int maxwidth = 0;
            using (Graphics gr = Graphics.FromHwnd(lb.Handle))
            {
                foreach (object item in lb.Items)
                {
                    maxwidth = Math.Max(maxwidth, (int)Math.Ceiling(gr.MeasureString(item.ToString(), lb.Font).Width + SystemInformation.VerticalScrollBarWidth));
                }
            }
            lb.Width = maxwidth;
        }
        /// <summary>
        /// Get the size of a control to fit all items in a list
        /// </summary>
        /// <param name="container">
        /// Container control
        /// </param>
        /// <param name="items">
        /// List of items
        /// </param>
        /// <returns></returns>
        public static Size GetListSize(Control container, List<object> items)
        {
            int maxwidth = 0;
            int height = 0;
            using (Graphics gr = Graphics.FromHwnd(container.Handle))
            {
                foreach (object item in items)
                {
                    SizeF sz = gr.MeasureString(item.ToString(), container.Font);
                    height = (int)Math.Ceiling(height + sz.Height);
                    maxwidth = Math.Max(maxwidth, (int)Math.Ceiling(sz.Width + SystemInformation.VerticalScrollBarWidth));
                }
            }
            return new Size(maxwidth, height);
        }
        /// <summary>
        /// Set the splitter distance of a SplitContainer.Panel2 to fit all items
        /// </summary>
        /// <param name="sp">
        /// SplitContainer control
        /// </param>
        /// <param name="items">
        /// List of objets to fit
        /// </param>
        public static void SetRightPanelSplitterDistance(SplitContainer sp, List<object> items)
        {
            int maxwidth = 0;
            using (Graphics gr = Graphics.FromHwnd(sp.Panel2.Handle))
            {
                foreach (object item in items)
                {
                    maxwidth = Math.Max(maxwidth, (int)Math.Ceiling(gr.MeasureString(item.ToString(), sp.Panel2.Font).Width + SystemInformation.VerticalScrollBarWidth));
                }
            }
            sp.SplitterDistance = sp.Width - (maxwidth + sp.SplitterWidth);
        }
        /// <summary>
        /// Set the splitter distance of a SplitContainer.Panel2 to fit all items in a ListView
        /// </summary>
        /// <param name="sp">
        /// SplitContainer control
        /// </param>
        /// <param name="items">
        /// ListView with items to fit
        /// </param>
        public static void SetRightPanelSplitterDistance(SplitContainer sp, ListView items)
        {
            int maxwidth = 0;
            using (Graphics gr = Graphics.FromHwnd(items.Handle))
            {
                foreach (ListViewItem item in items.Items)
                {
                    maxwidth = Math.Max(maxwidth, (int)Math.Ceiling(gr.MeasureString(item.Text, item.Font).Width + SystemInformation.VerticalScrollBarWidth));
                }
                foreach (ListViewGroup group in items.Groups)
                {
                    maxwidth = Math.Max(maxwidth, (int)Math.Ceiling(gr.MeasureString(group.Header, SystemFonts.CaptionFont).Width + SystemInformation.VerticalScrollBarWidth));
                }
            }
            sp.SplitterDistance = sp.Width - (maxwidth + sp.SplitterWidth);
        }
        /// <summary>
        /// Set a Splitter Container width to fit two lists of objects
        /// </summary>
        /// <param name="sp">
        /// SplitContainer control
        /// </param>
        /// <param name="litems">
        /// Left List of objets
        /// </param>
        /// <param name="ritems">
        /// Right List of objets
        /// </param>
        public static void FitSplitterLists(SplitContainer sp, List<object> litems, List<object> ritems)
        {
            int lmaxwidth = 0;
            using (Graphics gr = Graphics.FromHwnd(sp.Panel1.Handle))
            {
                foreach (object item in litems)
                {
                    lmaxwidth = Math.Max(lmaxwidth, (int)Math.Ceiling(gr.MeasureString(item.ToString(), sp.Panel1.Font).Width + SystemInformation.VerticalScrollBarWidth));
                }
            }
            int rmaxwidth = 0;
            using (Graphics gr = Graphics.FromHwnd(sp.Panel1.Handle))
            {
                foreach (object item in ritems)
                {
                    rmaxwidth = Math.Max(rmaxwidth, (int)Math.Ceiling(gr.MeasureString(item.ToString(), sp.Panel2.Font).Width + SystemInformation.VerticalScrollBarWidth));
                }
            }
            sp.Width = sp.SplitterWidth + lmaxwidth + rmaxwidth;
            sp.SplitterDistance = sp.Width - (lmaxwidth + sp.SplitterWidth);
        }
        /// <summary>
        /// Create columns and rows in a ListView from a list of objects
        /// </summary>
        /// <param name="listView">
        /// ListView control
        /// </param>
        /// <param name="objects">
        /// Object list
        /// </param>
        public static void PopulateListView(ListView listView, IList objects)
        {
            listView.View = View.Details;
            listView.Columns.Clear();
            listView.Items.Clear();

            if (objects == null || objects.Count == 0)
            {
                return;
            }

            // Get browsable properties
            var properties = objects[0].GetType().GetProperties()
                .Where(prop => prop.IsDefined(typeof(BrowsableAttribute), false) &&
                               ((BrowsableAttribute)prop.GetCustomAttribute(typeof(BrowsableAttribute))).Browsable)
                .ToList();

            // Add columns to ListView
            foreach (var prop in properties)
            {
                var displayName = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? prop.Name;
                listView.Columns.Add(displayName);
            }

            // Add rows with values to ListView
            foreach (var obj in objects)
            {
                ListViewItem item = new ListViewItem(properties.Select(prop => prop.GetValue(obj)?.ToString() ?? string.Empty).ToArray());
                item.Tag = obj;
                listView.Items.Add(item);
            }

            // Adjust column sizes
            foreach (ColumnHeader column in listView.Columns)
            {
                column.Width = -2; // Auto-resize to fit content
            }
        }
        /// <summary>
        /// Add object to a ListView
        /// </summary>
        /// <param name="listView">
        /// ListView control
        /// </param>
        /// <param name="obj">
        /// Object to add
        /// </param>
        public static void AddToListView(ListView listView, object obj)
        {
            if (obj == null)
            {
                return;
            }

            // Get browsable properties
            var properties = obj.GetType().GetProperties()
                .Where(prop => prop.IsDefined(typeof(BrowsableAttribute), false) &&
                               ((BrowsableAttribute)prop.GetCustomAttribute(typeof(BrowsableAttribute))).Browsable)
                .ToList();

            // Add object to ListView
            ListViewItem item = new ListViewItem(properties.Select(prop => prop.GetValue(obj)?.ToString() ?? string.Empty).ToArray());
            item.Tag = obj;
            listView.Items.Add(item);
        }
    }
}
