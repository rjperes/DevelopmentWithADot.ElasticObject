using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevelopmentWithADot.ElasticObject
{
	[Serializable]
	public sealed class ElasticObjectPropertyDescriptor : PropertyDescriptor
	{
		public ElasticObjectPropertyDescriptor(String name) : base(name, null)
		{
		}

		public override Boolean CanResetValue(Object component)
		{
			return (false);
		}

		public override Type ComponentType
		{
			get { return(typeof(ElasticObject)); }
		}

		public override Object GetValue(Object component)
		{
			return ((component as IDictionary<String, Object>)[this.Name]);
		}

		public override Boolean IsReadOnly
		{
			get { return(false); }
		}

		public override Type PropertyType
		{
			get { return (typeof(ElasticObject)); }
		}

		public override void ResetValue(Object component)
		{
		}

		public override void SetValue(Object component, Object value)
		{
			(component as IDictionary<String, Object>)[this.Name] = value;
		}

		public override Boolean ShouldSerializeValue(Object component)
		{
			return (true);
		}
	}
}
