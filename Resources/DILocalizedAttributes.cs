using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using System.Resources;
using Resources.Properties;
using System.CodeDom;
using System.Reflection;

namespace Resources
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class DILocalizedDescriptionAttribute : DescriptionAttribute
    {
        private bool _bTranslate = true;
        private ResourceManager _resources = null;

        public DILocalizedDescriptionAttribute(string id, Type rtype = null)
            : base(id)
        {
            if (rtype == null)
            {
                _resources = UIResources.ResourceManager;
            }
            else
            {
                _resources = new ResourceManager(rtype);
            }
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
    public class DILocalizedCategoryAttribute : CategoryAttribute
    {
        private ResourceManager _resources = null;
        public DILocalizedCategoryAttribute(string id, Type rtype = null)
            : base(id)
        {
            if (rtype == null)
            {
                _resources = UIResources.ResourceManager;
            }
            else
            {
                _resources = new ResourceManager(rtype);
            }
        }
        protected override string GetLocalizedString(string value)
        {
            try
            {
                return _resources.GetString(value, Thread.CurrentThread.CurrentCulture);
            }
            catch
            {
                return base.GetLocalizedString(value);
            }
        }
    }
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class DILocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private bool _bTranslate = true;
        private ResourceManager _resources = null;
        public DILocalizedDisplayNameAttribute(string id, Type rtype = null)
            : base(id)
        {
            if (rtype == null)
            {
                _resources = UIResources.ResourceManager;
            }
            else
            {
                _resources = new ResourceManager(rtype);
            }
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
