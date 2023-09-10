using appCoffeeMachine;
// ласс модульного тестировани€ класса табул€ции
namespace UnitTestAppCoffeeMachine
{
	public class TabulationTest
	{
		[Fact]
		public void whiteSpaceEmptyStringEqualResult()
		{
			string result=Tabulation.whiteSpace("", 5);
			Assert.Equal("     ", result);
		}

		[Fact]
		public void whiteSpaceEqualResult()
		{
			string result = Tabulation.whiteSpace("test", 5);
			Assert.Equal("test ", result);
		}

		[Fact]
		public void underLineEmptyStringEqualResult()
		{
			string result = Tabulation.underLine("", 6);
			Assert.Equal("______", result);
		}
		[Fact]
		public void underLineEqualResult()
		{
			string result = Tabulation.underLine("test", 6);
			Assert.Equal("test__", result);
		}
		[Fact]
		public void addTabulationEmptyStringEqualResult()
		{
			string result = Tabulation.addTabulation("", 7, '%');
			Assert.Equal("%%%%%%%", result);
		}
		[Fact]
		public void addTabulationEqualResult()
		{
			string result = Tabulation.addTabulation("test", 6, '%');
			Assert.Equal("test%%", result);
		}
	}
}