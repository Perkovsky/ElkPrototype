using MG.MainLogger.Formatter;
using Xunit;

namespace MG.MainLogger.Tests.Formatter
{
	[Trait("MG.MainLogger", "NameFormatter")]
	public class NameFormatterTests
	{
		[Theory]
		[InlineData("ThisIsTest", "this-is-test")]
		[InlineData("ThisIsTest1020", "this-is-test1020")]
		[InlineData("10ThisIsTest", "10-this-is-test")]
		[InlineData("This10IsTest", "this10-is-test")]
		[InlineData("10ThisIsTest20", "10-this-is-test20")]
		[InlineData("10This20IsTest", "10-this20-is-test")]
		[InlineData("10This20Is30Test40", "10-this20-is30-test40")]
		public void SanitizeName_KebabCase_ReturnsString(string name, string expectedResult)
		{
			// Arrange
			var formatter = NameFormatter.Create("-");

			// Act
			var result = formatter.SanitizeName(name);

			// Assert
			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData("ThisIsTest", "this_is_test")]
		[InlineData("ThisIsTest1020", "this_is_test1020")]
		[InlineData("10ThisIsTest", "10_this_is_test")]
		[InlineData("This10IsTest", "this10_is_test")]
		[InlineData("10ThisIsTest20", "10_this_is_test20")]
		[InlineData("10This20IsTest", "10_this20_is_test")]
		[InlineData("10This20Is30Test40", "10_this20_is30_test40")]
		public void SanitizeName_SnakeCase_ReturnsString(string name, string expectedResult)
		{
			// Arrange
			var formatter = NameFormatter.Create("_");

			// Act
			var result = formatter.SanitizeName(name);

			// Assert
			Assert.Equal(expectedResult, result);
		}
	}
}
