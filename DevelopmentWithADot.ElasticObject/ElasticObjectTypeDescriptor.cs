using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DevelopmentWithADot.ElasticObject
{
	[Serializable]
	public sealed class ElasticObjectTypeDescriptor : CustomTypeDescriptor
	{
		private readonly ElasticObject instance;

		public ElasticObjectTypeDescriptor(Object instance)
		{
			this.instance = instance as ElasticObject;
		}

		public override PropertyDescriptorCollection GetProperties()
		{
			if (this.instance != null)
			{
				return new PropertyDescriptorCollection((this.instance as IDictionary<String, Object>).Keys.Select(x => new ElasticObjectPropertyDescriptor(x)).ToArray());
			}
			else
			{
				return (base.GetProperties());
			}
		}

		public override TypeConverter GetConverter()
		{
			return (new ElasticObjectTypeConverter());
		}

		public override AttributeCollection GetAttributes()
		{
			return (new AttributeCollection(new SerializableAttribute()));
		}
	}
}
