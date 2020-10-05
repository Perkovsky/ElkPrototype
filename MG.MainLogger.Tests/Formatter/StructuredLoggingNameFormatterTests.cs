using MG.MainLogger.Formatter;
using Xunit;

namespace MG.MainLogger.Tests.Formatter
{
	[Trait("MG.MainLogger", "StructuredLoggingNameFormatter")]
	public class StructuredLoggingNameFormatterTests
	{
		[Theory]
		[InlineData("SystemLogStructuredLogging", "system-log")]
		[InlineData("SystemLog", "system-log")]
		[InlineData("TransactionLogStructuredLogging", "transaction-log")]
		[InlineData("TransactionLog", "transaction-log")]
		[InlineData("MyStructuredLoggingStructuredLogging", "my-structured-logging")]
		[InlineData("MyStructuredLoggingTestStructuredLogging", "my-structured-logging-test")]
		[InlineData("StructuredLoggingTestStructuredLogging", "structured-logging-test")]
		[InlineData("StructuredLoggingStructuredLogging", "structured-logging")]
		public void SanitizeName_KebabCase_ReturnsString(string name, string expectedResult)
		{
			// Arrange
			var formatter = StructuredLoggingNameFormatter.Create("-");

			// Act
			var result = formatter.SanitizeName(name);

			// Assert
			Assert.Equal(expectedResult, result);
		}
	}
}
