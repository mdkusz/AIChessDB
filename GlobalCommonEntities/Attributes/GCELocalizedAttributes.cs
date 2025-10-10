using System;
using System.ComponentModel;
using System.Resources;
using System.Threading;

namespace GlobalCommonEntities.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class GCELocalizedDescriptionAttribute : DescriptionAttribute
    {
        private bool _bTranslate = true;
        private ResourceManager _resources = null;

        public GCELocalizedDescriptionAttribute(string id, Type rtype)
            : base(id)
        {
            _resources = new ResourceManager(rtype);
        }
        public override string Description
        {
            get
            {
                if (_bTranslate)
                {
                    try
                    {
                        DescriptionValue = _resources.GetString(base.Description, Thread.CurrentThread.CurrentCulture);
                        _bTranslate = false;
                    }
                    catch { }
                }
                return DescriptionValue;
            }
        }
    }
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class GCELocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private bool _bTranslate = true;
        private ResourceManager _resources = null;
        public GCELocalizedDisplayNameAttribute(string id, Type rtype)
            : base(id)
        {
            _resources = new ResourceManager(rtype);
        }
        public override string DisplayName
        {
            get
            {
                if (_bTranslate)
                {
                    try
                    {
                        DisplayNameValue = _resources.GetString(base.DisplayName, Thread.CurrentThread.CurrentCulture);
                        _bTranslate = false;
                    }
                    catch { }
                }
                return DisplayNameValue;
            }
        }
    }
}
