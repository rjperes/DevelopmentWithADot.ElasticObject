using System;

namespace DevelopmentWithADot.ElasticObject.Tests
{
	class Program
	{
		static void TestConvert()
		{
			dynamic obj = new ElasticObject(1);
			Int32 i = obj;
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
			obj.A = new ElasticObject();
			obj.A.B = "1";
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

		static void Main(String[] args)
		{
			TestBinaryOperations();
			TestUnaryOperations();
			TestConvert();
			TestImmediateMembers();
			TestDeepMembers();
			TestAnonymous();
		}
	}
}
