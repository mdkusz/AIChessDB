using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable
{
    public partial class PropertyContainerPanel : Panel
    {
        protected Panel headerPanel = new Panel();
        protected FlowLayoutPanel propertyCollectionPanel = new FlowLayoutPanel();
        public PropertyContainerPanel()
        {
            SuspendLayout();
            Margin = new Padding(0);
            Padding = new Padding(0);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Name = "headerPanel";
            headerPanel.Height = 20;
            headerPanel.TabIndex = 0;
            propertyCollectionPanel.AutoScroll = false;
            propertyCollectionPanel.Dock = DockStyle.Fill;
            propertyCollectionPanel.Name = "propertyCollectionPanel";
            propertyCollectionPanel.TabIndex = 1;
            propertyCollectionPanel.FlowDirection = FlowDirection.TopDown;
            propertyCollectionPanel.WrapContents = false;
            propertyCollectionPanel.Padding = new Padding(0);
            propertyCollectionPanel.Margin = new Padding(0);
            Controls.Add(propertyCollectionPanel);
            Controls.Add(headerPanel);
            ResumeLayout(false);
        }
    }
}
