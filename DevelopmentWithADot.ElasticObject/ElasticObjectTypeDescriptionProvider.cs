using System;
using System.ComponentModel;

namespace DevelopmentWithADot.ElasticObject
{
	[Serializable]
	public sealed class ElasticObjectTypeDescriptionProvider : TypeDescriptionProvider
	{
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, Object instance)
		{
			return (new ElasticObjectTypeDescriptor(instance));
		}
	}
}
