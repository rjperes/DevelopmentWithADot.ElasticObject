using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevelopmentWithADot.ElasticObject.Tests
{
	class Program
	{
		static void TestConvert()
		{
			dynamic obj = new ElasticObject(1);
			Int32 i = obj;
			Single f = obj;
			String s = obj;
			DayOfWeek dow = obj;
			Boolean b = obj;
		}

		static void TestImmediateMembers()
		{
			dynamic obj = new ElasticObject();
			obj.A = "1";
		}

		static void TestDeepMembers()
		{
			dynamic obj = new ElasticObject();
			obj.A = 1;
			obj.A.B = "1";
		}

		static void TestDynamicMemberAccess()
		{
			dynamic obj = new ElasticObject();
			obj.A = "A";

			var a = obj["A"];
		}

		static void TestAnonymous()
		{
			dynamic obj = new ElasticObject(new { A = 1 });
			Int32 i = obj.A;
		}

		static void TestUnaryOperations()
		{
			dynamic obj = new ElasticObject(1);
			Int32 n = ~obj;
			Int32 prei = ++obj;
			Int32 pred = --obj;
			Int32 posi = obj++;
			Int32 posd = obj--;
		}

		static void TestBinaryOperations()
		{
			dynamic obj = new ElasticObject(1);
			Boolean b1 = obj == 1;
			Boolean b2 = obj != null;
		}

		static void TestTypeDescriptor()
		{
			dynamic obj = new ElasticObject();
			obj.Name = "ricardo";
			var provider = TypeDescriptor.GetProvider(obj) as TypeDescriptionProvider;
			var descriptor = provider.GetTypeDescriptor(obj) as ICustomTypeDescriptor;
			var converter = descriptor.GetConverter() as TypeConverter;
			var attrs = TypeDescriptor.GetAttributes(obj);
			var props = TypeDescriptor.GetProperties(obj) as PropertyDescriptorCollection;
			var prop = props["Name"] as PropertyDescriptor;
			var value = prop.GetValue(obj);
		}

		static void TestNotifyPropertyChanged()
		{
			dynamic obj = new ElasticObject();
			(obj as INotifyPropertyChanged).PropertyChanged += (s, e) =>
				{
					e.ToString();
				};
			obj.Name = "ricardo";
		}

		static void TestSerialization()
		{
			var formatter = new BinaryFormatter();
			dynamic obj = new ElasticObject();
			obj.Name = "Ricardo";

			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, obj);

				stream.Position = 0;

				obj = formatter.Deserialize(stream) as ElasticObject;
			}
		}

		static void Main(String[] args)
		{
			TestSerialization();
			TestNotifyPropertyChanged();
			TestTypeDescriptor();
			TestBinaryOperations();
			TestUnaryOperations();
			TestConvert();
			TestImmediateMembers();
			TestDeepMembers();
			TestAnonymous();
			TestDynamicMemberAccess();
		}
	}
}
