using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace DevelopmentWithADot.ElasticObject
{
	[Serializable]
	[TypeConverter(typeof(ElasticObjectTypeConverter))]
	[TypeDescriptionProvider(typeof(ElasticObjectTypeDescriptionProvider))]
	public sealed class ElasticObject : DynamicObject, IDictionary<String, Object>, ICloneable, INotifyPropertyChanged
	{
		#region Private readonly fields
        private static readonly String [] SpecialKeys = new String[] { "$Path", "$Parent", "$Root", "$Value", "$Type" };
		private readonly IDictionary<String, Object> values = new Dictionary<String, Object>();
		private Object value;
		private ElasticObject parent;
		#endregion

		#region Public constructors
		public ElasticObject() : this(null, null)
		{
		}

		internal ElasticObject(ElasticObject parent, Object value)
		{
			this.parent = parent;
			this.value = (value is ElasticObject) ? ((ElasticObject)value).value : value;
		}

		public ElasticObject(Object value) : this(null, value)
		{
		}
		#endregion

		#region Public override methods
		public override String ToString()
		{
			if (this.value != null)
			{
				return (this.value.ToString());
			}
			else
			{
				var dict = this as IDictionary<String, Object>;
				return (String.Format("{{{0}}}", String.Join(", ", dict.Keys.Zip(dict.Values, (k, v) => String.Format("{0}={1}", k, v)))));
			}
		}

		public override Int32 GetHashCode()
		{
			if (this.value != null)
			{
				return (this.value.GetHashCode());
			}
			else
			{
				return (base.GetHashCode());
			}
		}

		public override Boolean Equals(Object obj)
		{
			if (Object.ReferenceEquals(this, obj) == true)
			{
				return (true);
			}

			var other = obj as ElasticObject;

			if (other == null)
			{
				return (false);
			}

			if (Object.Equals(other.value, this.value) == false)
			{
				return (false);
			}

			return (this.values.SequenceEqual(other.values));
		}

		public override IEnumerable<String> GetDynamicMemberNames()
		{
			return (this.values.Keys.Concat((this.value != null) ? TypeDescriptor.GetProperties(this.value).OfType<PropertyDescriptor>().Select(x => x.Name) : Enumerable.Empty<String>()));
		}

		public override Boolean TryBinaryOperation(BinaryOperationBinder binder, Object arg, out Object result)
		{
			if (binder.Operation == ExpressionType.Equal)
			{
				result = Object.Equals(this.value, arg);
				return (true);
			}
			else if (binder.Operation == ExpressionType.NotEqual)
			{
				result = !Object.Equals(this.value, arg);
				return (true);
			}

			return (base.TryBinaryOperation(binder, arg, out result));
		}

		public override Boolean TryUnaryOperation(UnaryOperationBinder binder, out Object result)
		{
			if (binder.Operation == ExpressionType.Increment)
			{
				if (this.value is Int16)
				{
					result = (Int16)value + 1;
					return (true);
				}
				else if (this.value is Int32)
				{
					result = (Int32)value + 1;
					return (true);
				}
				else if (this.value is Int64)
				{
					result = (Int64)value + 1;
					return (true);
				}
				else if (this.value is UInt16)
				{
					result = (UInt16)value + 1;
					return (true);
				}
				else if (this.value is UInt32)
				{
					result = (UInt32)value + 1;
					return (true);
				}
				else if (this.value is UInt64)
				{
					result = (UInt64)value + 1;
					return (true);
				}
				else if (this.value is Decimal)
				{
					result = (Decimal)value + 1;
					return (true);
				}
				else if (this.value is Single)
				{
					result = (Single)value + 1;
					return (true);
				}
				else if (this.value is Double)
				{
					result = (Double)value + 1;
					return (true);
				}
			}
			else if (binder.Operation == ExpressionType.Decrement)
			{
				if (this.value is Int16)
				{
					result = (Int16)value - 1;
					return (true);
				}
				else if (this.value is Int32)
				{
					result = (Int32)value - 1;
					return (true);
				}
				else if (this.value is Int64)
				{
					result = (Int64)value - 1;
					return (true);
				}
				else if (this.value is UInt16)
				{
					result = (UInt16)value - 1;
					return (true);
				}
				else if (this.value is UInt32)
				{
					result = (UInt32)value - 1;
					return (true);
				}
				else if (this.value is UInt64)
				{
					result = (UInt64)value - 1;
					return (true);
				}
				else if (this.value is Decimal)
				{
					result = (Decimal)value - 1;
					return (true);
				}
				else if (this.value is Single)
				{
					result = (Single)value - 1;
					return (true);
				}
				else if (this.value is Double)
				{
					result = (Double)value - 1;
					return (true);
				}
			}
			else if (binder.Operation == ExpressionType.Not)
			{
				if (this.value is Boolean)
				{
					result = !(Boolean)value;
					return (true);
				}
			}
			else if (binder.Operation == ExpressionType.OnesComplement)
			{
				if (this.value is Int16)
				{
					result = ~(Int16)value;
					return (true);
				}
				else if (this.value is Int32)
				{
					result = ~(Int32)value;
					return (true);
				}
				else if (this.value is Int64)
				{
					result = ~(Int64)value;
					return (true);
				}
				else if (this.value is UInt16)
				{
					result = ~(UInt16)value;
					return (true);
				}
				else if (this.value is UInt32)
				{
					result = ~(UInt32)value;
					return (true);
				}
				else if (this.value is UInt64)
				{
					result = ~(UInt64)value;
					return (true);
				}
			}

			return base.TryUnaryOperation(binder, out result);
		}

		public override Boolean TryInvokeMember(InvokeMemberBinder binder, Object[] args, out Object result)
		{
			var method = this.GetType().GetMethod(binder.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (method == null)
			{
				foreach (var type in this.GetType().GetInterfaces())
				{
					method = type.GetMethod(binder.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

					if (method != null)
					{
						break;
					}
				}
			}

			if (method != null)
			{
				result = method.Invoke(this, args);

				return (true);
			}
			else
			{
				return (base.TryInvokeMember(binder, args, out result));
			}
		}

		public override Boolean TryConvert(ConvertBinder binder, out Object result)
		{
			if (this.value != null)
			{
				if (binder.Type.IsInstanceOfType(this.value) == true)
				{
					result = this.value;
					return (true);
				}
				else if (binder.Type.IsEnum == true)
				{
					result = Enum.Parse(binder.Type, this.value.ToString());
					return (true);
				}
				else if ((typeof(IConvertible).IsAssignableFrom(binder.Type) == true) && (typeof(IConvertible).IsAssignableFrom(this.value.GetType()) == true))
				{
					result = Convert.ChangeType(this.value, binder.Type);
					return (true);
				}
				else if (binder.Type == typeof(String))
				{
					result = this.value.ToString();
					return (true);
				}
				else
				{
					var converter = TypeDescriptor.GetConverter(binder.Type);

					if (converter.CanConvertFrom(this.value.GetType()) == true)
					{
						result = converter.ConvertFrom(this.value);
						return (true);
					}
				}
			}
			else if (binder.Type.IsClass == true)
			{
				result = null;
				return (true);
			}

			result = null;
			return (false);
		}

		public override Boolean TrySetMember(SetMemberBinder binder, Object value)
		{
			(this as IDictionary<String, Object>)[binder.Name] = value;

			return (true);
		}

		public override Boolean TryGetMember(GetMemberBinder binder, out Object result)
		{
            if (this.value != null)
            {
                var prop = TypeDescriptor.GetProperties(this.value)[binder.Name];

                if (prop != null)
                {
                    result = prop.GetValue(this.value);
                    return (true);
                }
            }

            return (this.values.TryGetValue(binder.Name, out result));
		}

		public override Boolean TrySetIndex(SetIndexBinder binder, Object[] indexes, Object value)
		{
			if ((indexes.Count() != 1) || (indexes.First() == null))
			{
				return (false);
			}

			var key = indexes.First() as String;

			if (indexes.First() is Int32)
			{
				var index = (Int32)indexes.First();

				if (this.values.Count < index)
				{
					key = this.values.ElementAt(index).Key;
				}
			}
            else if (key == null)
            {
                return (false);
            }

			(this as IDictionary<String, Object>)[key] = value;

			return (true);
		}

		public override Boolean TryGetIndex(GetIndexBinder binder, Object[] indexes, out Object result)
		{
			if ((indexes.Count() != 1) || (indexes.First() == null))
			{
				result = null;
				return (false);
			}

			var key = indexes.First() as String;

			if (key != null)
			{
                if (this.value != null)
                {
                    var prop = TypeDescriptor.GetProperties(this.value)[key];

                    if (prop != null)
                    {
                        result = prop.GetValue(this.value);
                        return (true);
                    }
                }

                if (String.Equals("$parent", key, StringComparison.InvariantCultureIgnoreCase) == true)
				{
					result = this.parent;
					return (true);
				}
				else if (String.Equals("$value", key, StringComparison.InvariantCultureIgnoreCase) == true)
				{
					result = this.value;
					return (true);
				}
				else if (String.Equals("$type", key, StringComparison.InvariantCultureIgnoreCase) == true)
				{
					result = ((this.value != null) ? this.value.GetType() : null);
					return (true);
				}
                else if (String.Equals("$root", key, StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    var root = this;

                    while (root != null)
                    {
                        if (root.parent == null)
                        {
                            break;
                        }

                        root = root.parent;
                    }

                    result = root;
                    return (true);
                }
                else if (String.Equals("$path", key, StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    var list = new LinkedList<string>();

                    var p = this.parent;
                    var previous = (Object)this;

                    while (p != null)
                    {
                        var kv = p.values.SingleOrDefault(x => (Object)x.Value == (Object)previous);

                        list.AddFirst(kv.Key);

                        previous = ((ElasticObject)kv.Value).parent;
                        p = p.parent;
                    }

                    result = String.Join(".", list);
                    return (true);
                }
                else
                {
					return (this.values.TryGetValue(key, out result));
				}
			}
			else if (indexes.First() is Int32)
			{
				var index = (Int32)indexes.First();

				if (this.values.Count < index)
				{
					result = this.values.ElementAt(index).Value;
					return (true);
				}
			}

			result = null;
			return (false);
		}
		#endregion

		#region IDictionary<String,Object> Members
		void IDictionary<String,Object>.Add(String key, Object value)
		{
			(this as IDictionary<String,Object>)[key] = value;
		}

		Boolean IDictionary<String,Object>.ContainsKey(String key)
		{
			return (this.GetDynamicMemberNames().Contains(key));
		}

		ICollection<String> IDictionary<String,Object>.Keys
		{
			get
			{
				return (this.GetDynamicMemberNames().ToList());
			}
		}

		Boolean IDictionary<String,Object>.Remove(String key)
		{
			return (this.values.Remove(key));
		}

		Boolean IDictionary<String,Object>.TryGetValue(String key, out Object value)
		{
		    if (this.value != null)
		    {
		        var prop = TypeDescriptor.GetProperties(this.value)[key];

		        if (prop != null)
		        {
		            value = prop.GetValue(this.value);
		            return (true);
		        }
		    }

		    return (this.values.TryGetValue(key, out value));
		}

		ICollection<Object> IDictionary<String,Object>.Values
		{
			get
			{
				return (this.values.Values.Concat((this.value != null) ? TypeDescriptor.GetProperties(this.value).OfType<PropertyDescriptor>().Select(x => x.GetValue(this.value)) : Enumerable.Empty<Object>()).ToList());
			}
		}

		Object IDictionary<String,Object>.this[String key]
		{
			get
			{
			    if (this.value != null)
			    {
			        var prop = TypeDescriptor.GetProperties(this.value)[key];

			        if (prop != null)
			        {
			            return (prop.GetValue(this.value));
			        }
			    }

				return (this.values[key]);
			}
			set
			{
				if (value is ElasticObject)
				{
					this.values[key] = value;
				    ((ElasticObject) value).parent = this;
				}
				else if (value == null)
				{
					this.values[key] = null;
				}
				else
				{
					this.values[key] = new ElasticObject(this, value);
				}

				this.OnPropertyChanged(new PropertyChangedEventArgs(key));
			}
		}

		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			var handler = this.PropertyChanged;

			if (handler != null)
			{
				handler(this, e);
			}
		}
		#endregion

		#region ICollection<KeyValuePair<String,Object>> Members
		void ICollection<KeyValuePair<String, Object>>.Add(KeyValuePair<String, Object> item)
		{
			(this as IDictionary<String, Object>)[item.Key] = item.Value;
		}

		void ICollection<KeyValuePair<String, Object>>.Clear()
		{
			this.values.Clear();
		}

		Boolean ICollection<KeyValuePair<String, Object>>.Contains(KeyValuePair<String, Object> item)
		{
			return (this.values.Contains(item));
		}

		void ICollection<KeyValuePair<String, Object>>.CopyTo(KeyValuePair<String, Object>[] array, Int32 arrayIndex)
		{
		    this.values.CopyTo(array, arrayIndex);
		}

		Int32 ICollection<KeyValuePair<String, Object>>.Count
		{
			get
			{
				return (this.values.Count);
			}
		}

		Boolean ICollection<KeyValuePair<String,Object>>.IsReadOnly
		{
			get
			{
				return (this.values.IsReadOnly);
			}
		}

		Boolean ICollection<KeyValuePair<String,Object>>.Remove(KeyValuePair<String, Object> item)
		{
			return (this.values.Remove(item));
		}
		#endregion

		#region IEnumerable<KeyValuePair<String,Object>> Members
		IEnumerator<KeyValuePair<String, Object>> IEnumerable<KeyValuePair<String,Object>>.GetEnumerator()
		{
			return (this.values.GetEnumerator());
		}
		#endregion

		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((this as IDictionary<String, Object>).GetEnumerator());
		}
		#endregion

		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region ICloneable Members
		Object ICloneable.Clone()
		{
			var clone = new ElasticObject(null, this.value) as IDictionary<String, Object>;

			foreach (var key in this.values.Keys)
			{
			    clone[key] = (this.values[key] is ICloneable) ? (this.values[key] as ICloneable).Clone() : this.values[key];
			}

			return (clone);
		}
		#endregion
	}
}
