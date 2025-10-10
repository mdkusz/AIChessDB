using System;

namespace DesktopControls.Controls.PropertyTable.Attributes
{
    public class ValueEditorCaptionAttribute : Attribute
    {
        /// <summary>
        /// Atributo para establecer la leyenda del control de edición, por ejemplo en un check box
        /// Attribute to set the edition control caption, for instance in a check box control
        /// </summary>
        /// <param name="caption"></param>
        public ValueEditorCaptionAttribute(string caption)
        {
            Caption = caption;
        }
        public virtual string Caption { get; set; }
    }
}
