using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace DevelopmentWithADot.ElasticObject
{
	[Serializable]
	public sealed class ElasticObjectTypeConverter : TypeConverter
	{
		public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return (true);
		}

		public override Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(ElasticObject))
			{
				return (true);
			}
			else
			{
				return (base.CanConvertTo(context, destinationType));
			}
		}

		public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
		{
			if (value is ElasticObject)
			{
				return (value);
			}
			else
			{
				return (new ElasticObject(value));
			}
		}

		public override Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
		{
			if (destinationType == typeof(ElasticObject))
			{
				return (this.ConvertFrom(context, culture, value));
			}
			else
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, Object value, Attribute[] attributes)
		{
			var provider = TypeDescriptor.GetProvider(value) as TypeDescriptionProvider;
			var descriptor = provider.GetTypeDescriptor(value);
			return (descriptor.GetProperties(attributes));
		}

		public override Boolean GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return (true);
		}

		public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			dynamic obj = new ElasticObject();

			foreach (var key in propertyValues.Keys)
			{
				obj[key.ToString()] = propertyValues[key];
			}

			return (obj);
		}

		public override Boolean GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return (true);
		}

		public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return (false);
		}
	}
}
