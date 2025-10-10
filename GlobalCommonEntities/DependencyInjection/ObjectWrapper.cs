using GlobalCommonEntities.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GlobalCommonEntities.DependencyInjection
{
    /// <summary>
    /// Generic object to provide user interface to any object
    /// </summary>
    /// <remarks>
    /// This is a platform intended to provide a common interface to any object from an undefinded number of providers.
    /// ObjectWrapper is a generic object that wraps any object and provides a common user interface to it.
    /// FriendlyName, FriendlyDescription, UID and ContentType are the main properties to provide a common interface.
    /// ToString method is overriden to return the FriendlyName property. So the object can be used in any user control like combo or list boxes.
    /// The IComparable and IEquatable interfaces are implemented to sort and locate elements in lists and collections.
    /// </remarks>
    /// <seealso cref="ObjectWrapper<T>"/>
    public class ObjectWrapper : IUIIdentifier, IActiveObject, IComparable<ObjectWrapper>, IEquatable<ObjectWrapper>
    {
        protected object _object = null;
        public ObjectWrapper(object obj)
        {
            _object = obj;
        }
        public ObjectWrapper(object obj, string name, string id)
        {
            _object = obj;
            FriendlyName = name;
            UID = id;
        }
        /// <summary>
        /// Convert a list of ObjectWrapper generic objects to a list of typed objects
        /// </summary>
        /// <param name="objectList">
        /// ObjectWrapper list to convert
        /// </param>
        /// <returns>
        /// A typed list with the ObjectWrapper list inner objects
        /// </returns>
        /// <remarks>
        /// This method provides an easy way to convert a list of ObjectWrapper container objects to a list of its contained typed objects.
        /// </remarks>
        public static IList ConvertList(List<ObjectWrapper> objectList)
        {
            if (objectList.Count > 0)
            {
                Type type = objectList[0].ContentType;
                var listType = typeof(List<>).MakeGenericType(type);
                var typedList = (IList)Activator.CreateInstance(listType);

                foreach (ObjectWrapper obj in objectList)
                {
                    typedList.Add(obj.Implementation());
                }

                return typedList;
            }
            return null;
        }
        /// <summary>
        /// Unique identifier
        /// </summary>
        /// <remarks>
        /// Most objects in this platform have an unique identifier. This property helps to access it.
        /// </remarks>
        public string UID { get; set; }
        /// <summary>
        /// IUIIdentifier: Name to identify the object
        /// </summary>
        /// <remarks>
        /// The main use of this property is to show a user friendly name in user interface controls.
        /// </remarks>
        public string FriendlyName { get; set; }
        /// <summary>
        /// IUIIdentifier: Object use description
        /// </summary>
        /// <remarks>
        /// The main use of this property is to provide a description for tool tips and description panels.
        /// </remarks>
        public string FriendlyDescription { get; set; }
        /// <summary>
        /// IUIIdentifier: Object category
        /// </summary>
        /// <remarks>
        /// Use this property to classify the object.
        /// </remarks>
        public string UICategory { get; set; }
        /// <summary>
        /// IActiveObject: Object enabled status
        /// </summary>
        /// <remarks>
        /// This object can be used in controls as content or as an item in a list. This property allows to enable or disable the object.
        /// </remarks>
        public bool Active { get; set; } = true;
        /// <summary>
        /// IActiveObject: Object selection status
        /// </summary>
        /// <remarks>
        /// This object can be used in controls as content or as an item in a list. This property allows to select or unselect the object.
        /// </remarks>
        public bool Selected { get; set; }
        /// <summary>
        /// IActiveObject: Object check status
        /// </summary>
        /// <remarks>
        /// This object can be used in controls as content or as an item in a list. This property allows to check or uncheck the object.
        /// </remarks>
        public bool? Checked { get; set; }
        /// <summary>
        /// GenericType of the inner object
        /// </summary>
        public Type ContentType
        {
            get
            {
                return _object?.GetType();
            }
        }
        /// <summary>
        /// Get the inner object
        /// </summary>
        /// <remarks>
        /// This method does not return a new instance of the object, but the same object that was passed to the constructor. It can be null.
        /// </remarks>
        /// <returns>
        /// Object instance
        /// </returns>
        public object Implementation()
        {
            return _object;
        }
        /// <summary>
        /// Get a new instance of the inner object using the provided parameters
        /// </summary>
        /// <param name="p">
        /// Parameters to initialize the object
        /// </param>
        /// <returns>
        /// New instance of the object
        /// </returns>
        public object ParamImplementation(params object[] p)
        {
            if (p == null || p.Length == 0)
            {
                return _object;
            }
            List<Type> types = new List<Type>();
            foreach (object o in p)
            {
                types.Add(o.GetType());
            }
            return _object.GetType().GetConstructor(types.ToArray()).Invoke(p);
        }
        public int CompareTo(ObjectWrapper other)
        {
            if ((other == null) ||
                (other._object == null) ||
                (other._object.GetType() != _object.GetType()))
            {
                return 1;
            }
            if (_object is IComparable)
            {
                dynamic thisObject = _object;
                dynamic otherObject = other;

                return thisObject.CompareTo(otherObject);
            }
            return FriendlyName.CompareTo(other.FriendlyName);
        }
        public bool Equals(ObjectWrapper other)
        {
            if ((other == null) ||
                (other._object == null) ||
                (other._object.GetType() != _object.GetType()))
            {
                return false;
            }
            dynamic thisObject = _object;
            dynamic otherObject = other._object;

            return thisObject.Equals(otherObject);
        }
        public override bool Equals(object other)
        {
            if (other is ObjectWrapper)
            {
                return Equals(other as ObjectWrapper);
            }
            if ((other == null) ||
                (other.GetType() != _object.GetType()))
            {
                return false;
            }
            dynamic thisObject = _object;
            dynamic otherObject = other;

            return thisObject.Equals(otherObject);
        }
        public override int GetHashCode()
        {
            return _object.GetHashCode();
        }
        public override string ToString()
        {
            return FriendlyName;
        }
    }
    /// <summary>
    /// Typed ObjectWrapper object to provide user interface to an object of GenericType T
    /// </summary>
    /// <example>
    /// Use this object to wrap generic objects with whidely known types.
    /// </example>
    /// <remarks>
    /// This is a platform intended to provide a common interface to any object from an undefinded number of providers.
    /// ObjectWrapper<T> is a generic typed object that wraps any object and provides a common user interface to it.
    /// FriendlyName, FriendlyDescription, UID and ContentType are the main properties to provide a common interface.
    /// ToString method is overriden to return the FriendlyName property. So the object can be used in any user control like combo or list boxes.
    /// The IComparable and IEquatable interfaces are implemented to sort and locate elements in lists and collections.
    /// </remarks>
    /// <seealso cref="ObjectWrapper"/>
    public class ObjectWrapper<T> : IUIIdentifier, IActiveObject, IComparable<ObjectWrapper<T>>, IEquatable<ObjectWrapper<T>>, IEquatable<T>
    {
        protected T _object;
        public ObjectWrapper(T obj)
        {
            _object = obj;
        }
        public ObjectWrapper(T obj, string name, string id)
        {
            _object = obj;
            FriendlyName = name;
            UID = id;
        }
        /// <summary>
        /// Convert a list of ObjectWrapper typed objects to a list of typed objects
        /// </summary>
        /// <param name="objectList">
        /// ObjectWrapper list to convert
        /// </param>
        /// <returns>
        /// A typed list with the ObjectWrapper list inner objects
        /// </returns>
        /// <remarks>
        /// This method provides an easy way to convert a list of typed ObjectWrapper container objects to a list of its contained typed objects.
        /// </remarks>
        public static List<T> ConvertList(List<ObjectWrapper<T>> objectList)
        {
            List<T> list = new List<T>();

            foreach (ObjectWrapper<T> obj in objectList)
            {
                list.Add(obj.TypedImplementation);
            }

            return list;
        }
        /// <summary>
        /// Convert a list of typed objects to a list of typed ObjectWrapper objects
        /// </summary>
        /// <param name="objectList">
        /// List to convert
        /// </param>
        /// <returns>
        /// A typed ObjectWrapper list wrapping the list objects
        /// </returns>
        /// <remarks>
        /// This method provides an easy way to convert a list of objects to a list of typed ObjectWrapper container objects .
        /// </remarks>
        public static List<ObjectWrapper<T>> ConvertList(List<T> objectList)
        {
            List<ObjectWrapper<T>> list = new List<ObjectWrapper<T>>();

            foreach (T obj in objectList)
            {
                if (obj is IStandardObject)
                {
                    IStandardObject stdobj = obj as IStandardObject;
                    list.Add(new ObjectWrapper<T>(obj, stdobj.StdName, stdobj.StdUID));
                }
                else
                {
                    list.Add(new ObjectWrapper<T>(obj));
                }
            }

            return list;
        }
        /// <summary>
        /// Unique identifier
        /// </summary>
        /// <remarks>
        /// Most objects in this platform have an unique identifier. This property helps to access it.
        /// </remarks>
        public string UID { get; set; }
        /// <summary>
        /// IUIIdentifier: Name to identify the object
        /// </summary>
        /// <remarks>
        /// The main use of this property is to show a user friendly name in user interface controls.
        /// </remarks>
        public string FriendlyName { get; set; }
        /// <summary>
        /// IUIIdentifier: Object use description
        /// </summary>
        /// <remarks>
        /// The main use of this property is to provide a description for tool tips and description panels.
        /// </remarks>
        public string FriendlyDescription { get; set; }
        /// <summary>
        /// IUIIdentifier: Object category
        /// </summary>
        /// <remarks>
        /// Use this property to classify the object.
        /// </remarks>
        public string UICategory { get; set; }
        /// <summary>
        /// IActiveObject: Object enabled status
        /// </summary>
        /// <remarks>
        /// This object can be used in controls as content or as an item in a list. This property allows to enable or disable the object.
        /// </remarks>
        public bool Active { get; set; } = true;
        /// <summary>
        /// IActiveObject: Object selection status
        /// </summary>
        /// <remarks>
        /// This object can be used in controls as content or as an item in a list. This property allows to select or unselect the object.
        /// </remarks>
        public bool Selected { get; set; }
        /// <summary>
        /// IActiveObject: Object check status
        /// </summary>
        /// <remarks>
        /// This object can be used in controls as content or as an item in a list. This property allows to check or uncheck the object.
        /// </remarks>
        public bool? Checked { get; set; }
        /// <summary>
        /// GenericType of the inner object
        /// </summary>
        public Type ContentType
        {
            get
            {
                return _object?.GetType();
            }
        }
        /// <summary>
        /// Get the inner object
        /// </summary>
        public T TypedImplementation
        {
            get
            {
                return _object;
            }
        }
        /// <summary>
        /// IUIIdenbtifier: Get the inner object
        /// </summary>
        public object Implementation()
        {
            return _object;
        }
        /// <summary>
        /// Get a new instance of the inner object using the provided parameters
        /// </summary>
        /// <param name="p">
        /// Parameters to initialize the object
        /// </param>
        /// <returns>
        /// New instance of the object
        /// </returns>
        public T TypedParamImplementation(params object[] p)
        {
            if (p == null || p.Length == 0)
            {
                return _object;
            }
            List<Type> types = new List<Type>();
            foreach (object o in p)
            {
                types.Add(o.GetType());
            }
            var constructor = _object.GetType().GetConstructor(types.ToArray());
            if (constructor == null)
            {
                throw new InvalidOperationException("No constructor found");
            }
            return (T)constructor.Invoke(p);
        }
        /// <summary>
        /// IUIIdentifier: Get a new instance of the inner object using the provided parameters
        /// </summary>
        /// <param name="p">
        /// Parameters to initialize the object
        /// </param>
        /// <returns>
        /// New instance of the object
        /// </returns>
        public object ParamImplementation(params object[] p)
        {
            if (p == null || p.Length == 0)
            {
                return _object;
            }
            List<Type> types = new List<Type>();
            foreach (object o in p)
            {
                types.Add(o.GetType());
            }
            var constructor = _object.GetType().GetConstructor(types.ToArray());
            if (constructor == null)
            {
                throw new InvalidOperationException("No constructor found");
            }
            return constructor.Invoke(p);
        }
        public int CompareTo(ObjectWrapper<T> other)
        {
            if ((other == null) ||
                (other._object == null) ||
                (other._object.GetType() != _object.GetType()))
            {
                return 1;
            }
            if (_object is IComparable)
            {
                dynamic thisObject = _object;
                dynamic otherObject = other._object;

                return thisObject.CompareTo(otherObject);
            }
            return FriendlyName.CompareTo(other.FriendlyName);
        }
        public bool Equals(ObjectWrapper<T> other)
        {
            if ((other == null) ||
                (other._object == null) ||
                (other._object.GetType() != _object.GetType()))
            {
                return false;
            }
            T thisObject = _object;
            T otherObject = other._object;

            return thisObject.Equals(otherObject);
        }
        public bool Equals(T other)
        {
            if ((other == null) ||
                (other.GetType() != _object.GetType()))
            {
                return false;
            }
            return _object.Equals(other);
        }
        public override bool Equals(object other)
        {
            if (other is ObjectWrapper<T>)
            {
                return Equals(other as ObjectWrapper<T>);
            }
            if ((other == null) ||
                (other.GetType() != _object.GetType()))
            {
                return false;
            }
            T thisObject = _object;
            dynamic otherObject = other;

            return thisObject.Equals(otherObject);
        }
        public override int GetHashCode()
        {
            return _object.GetHashCode();
        }
        public override string ToString()
        {
            return FriendlyName;
        }
    }
}
