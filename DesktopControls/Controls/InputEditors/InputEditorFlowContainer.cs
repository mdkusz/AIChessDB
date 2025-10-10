using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// InputEditorBase container as a FlowLayoutPanel
    /// </summary>
    public class InputEditorFlowContainer : FlowLayoutPanel
    {
        public InputEditorFlowContainer()
        {
            AutoScroll = true;
            FlowDirection = FlowDirection.TopDown;
            WrapContents = false;
            Dock = DockStyle.Fill;
            Padding = new Padding(3, 4, 3, 4);
            Margin = new Padding(3, 4, 3, 4);
            BackColor = SystemColors.Desktop;
        }
        /// <summary>
        /// Inform parent container that it should resize
        /// </summary>
        public event EventHandler ResizeParent;
        /// <summary>
        /// Input editor control factory
        /// </summary>
        [Browsable(false)]
        public IInputEditorFactory EditorFactory { get; set; }
        /// <summary>
        /// Refresh controls in the container
        /// </summary>
        /// <param name="sender">
        /// Instance of the object sending the event
        /// </param>
        /// <param name="e">
        /// Event arguments with information about the operation to perform:
        /// </param>
        public void RefreshEditor(object sender, RefreshEditorEventArgs e)
        {
            try
            {
                List<IInputEditorBase> refreshList = new List<IInputEditorBase>();
                if (e.Index < 0)
                {
                    if ((e.Operation != EditorContainerOperation.Refresh) &&
                        (e.Operation != EditorContainerOperation.Update))
                    {
                        throw new Exception(ERR_InvalidMultipleOperation);
                    }
                    foreach (Control c in Controls)
                    {
                        IInputEditorBase dsp = c as InputEditorBase;
                        if (dsp != null)
                        {
                            refreshList.Add(dsp);
                        }
                    }
                }
                else if (e.Operation == EditorContainerOperation.Add)
                {
                    IInputEditorBase dsp = EditorFactory.CreateEditor(e.Property, sender, this);
                    Controls.Add(dsp as Control);
                    Controls.SetChildIndex(dsp as Control, e.Index);
                    ResizeParent?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    IInputEditorBase dsp = Controls[e.Index] as InputEditorBase;
                    if (dsp != null)
                    {
                        refreshList.Add(dsp);
                    }
                }
                for (int ix = 0; ix < refreshList.Count; ix++)
                {
                    IInputEditorBase dsp = refreshList[ix];
                    switch (e.Operation)
                    {
                        case EditorContainerOperation.Remove:
                            Controls.RemoveAt(e.Index);
                            ResizeParent?.Invoke(this, EventArgs.Empty);
                            break;
                        case EditorContainerOperation.Refresh:
                            (dsp as InputEditorBase).RefreshEditorValue();
                            break;
                        case EditorContainerOperation.Update:
                            (dsp as InputEditorBase).UpdatePropertyValue();
                            break;
                        case EditorContainerOperation.Rebuild:
                            dsp = EditorFactory.CreateEditor(e.Property, sender, this);
                            Controls.RemoveAt(e.Index);
                            Controls.Add(dsp as Control);
                            Controls.SetChildIndex(dsp as Control, e.Index);
                            ResizeParent?.Invoke(this, EventArgs.Empty);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            foreach (Control c in Controls)
            {
                c.Width = ClientSize.Width - Padding.Horizontal;
            }
        }
    }
}
