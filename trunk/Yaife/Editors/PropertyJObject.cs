using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace Yaife
{
	public class PropertyJObject : JObject, ICustomTypeDescriptor
	{
		private JObject internalObject;

		public PropertyJObject(JObject obj)
		{
			internalObject = obj;
		}

		private static Type convertType(JTokenType t)
		{
			switch (t)
			{
				case JTokenType.Array:
					return typeof(Array);
				case JTokenType.Boolean:
					return typeof(bool);
				case JTokenType.Bytes:
					return typeof(byte[]);
				case JTokenType.Date:
					return typeof(DateTime);
				case JTokenType.Float:
					return typeof(float);
				case JTokenType.Guid:
					return typeof(Guid);
				case JTokenType.Integer:
					return typeof(int);
				case JTokenType.Object:
					return typeof(object);
				case JTokenType.String:
					return typeof(string);
				case JTokenType.TimeSpan:
					return typeof(TimeSpan);
				case JTokenType.Uri:
					return typeof(Uri);
				default:
					return null;
			}
		}

		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this, true);
		}

		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(this, true);
		}

		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return null;
		}

		public object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);

		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			var list = new List<PropertyDescriptor>();

			foreach (var pair in internalObject)
				list.Add(new DynamicPropertyDescriptor(this, pair.Key, convertType(pair.Value.Type), attributes));

			return new PropertyDescriptorCollection(list.ToArray());
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(new Attribute[0]);
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return internalObject;
		}

		#region Private class
		private class DynamicPropertyDescriptor : PropertyDescriptor
		{
			private readonly PropertyJObject propertyJObject;
			private readonly Type propertyType;

			public DynamicPropertyDescriptor(PropertyJObject dynaObject, string propertyName, Type propertyType, Attribute[] propertyAttributes)
				: base(propertyName, propertyAttributes)
			{
				this.propertyJObject = dynaObject;
				this.propertyType = propertyType;
			}

			public override bool CanResetValue(object component)
			{
				return false;
			}

			public override object GetValue(object component)
			{
				return propertyJObject.internalObject[Name];
			}

			public override void ResetValue(object component)
			{
			}

			public override void SetValue(object component, object value)
			{
				propertyJObject.internalObject[Name] = (JToken)(new JValue(value));
			}

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}

			public override Type ComponentType
			{
				get { return null; }
			}

			public override bool IsReadOnly
			{
				get { return false; }
			}

			public override Type PropertyType
			{
				get { return propertyType; }
			}
		}
		#endregion
	}
}
