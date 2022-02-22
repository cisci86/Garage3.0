using System;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
	/// <summary>
	/// Custom floating point ModelBinder as the team of Microsoft is not capable of fixing their <see href="https://github.com/dotnet/aspnetcore/issues/6566">issue</see> with other cultures than en-US.
	/// </summary>
	public class CustomFloatingPointModelBinder : IModelBinder
	{
		private readonly NumberStyles supportedNumberStyles;
		private readonly ILogger logger;
		private readonly CultureInfo cultureInfo;

		/// <summary>
		/// Initializes a new instance of <see cref="CustomFloatingPointModelBinder"/>.
		/// </summary>
		/// <param name="supportedStyles">The <see cref="NumberStyles"/>.</param>
		/// <param name="cultureInfo">The <see cref="CultureInfo"/>.</param>
		/// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
		public CustomFloatingPointModelBinder(NumberStyles supportedStyles, CultureInfo cultureInfo, ILoggerFactory loggerFactory)
		{
			if (loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory));

			this.cultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));

			supportedNumberStyles = supportedStyles;
			logger = loggerFactory?.CreateLogger<CustomFloatingPointModelBinder>();
		}

		/// <inheritdoc />
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
				throw new ArgumentNullException(nameof(bindingContext));

			logger.AttemptingToBindModel(bindingContext);
			string modelName = bindingContext.ModelName;
			var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
			if (valueProviderResult == ValueProviderResult.None)
			{
				logger.FoundNoValueInRequest(bindingContext);

				// no entry
				logger.DoneAttemptingToBindModel(bindingContext);
				return Task.CompletedTask;
			}

			var modelState = bindingContext.ModelState;
			modelState.SetModelValue(modelName, valueProviderResult);

			var metadata = bindingContext.ModelMetadata;
			var type = metadata.UnderlyingOrModelType;
			try
			{
				string value = valueProviderResult.FirstValue;
				var culture = cultureInfo ?? valueProviderResult.Culture;

				object model;
				if (string.IsNullOrWhiteSpace(value))
				{
					// Parse() method trims the value (with common NumberStyles) then throws if the result is empty.
					model = null;
				}
				else if (type == typeof(float))
				{
					model = float.Parse(value, supportedNumberStyles, culture);
				}
				else if (type == typeof(double))
				{
					model = double.Parse(value, supportedNumberStyles, culture);
				}
				else if (type == typeof(decimal))
				{
					model = decimal.Parse(value, supportedNumberStyles, culture);
				}
				else
				{
					// unreachable
					throw new NotSupportedException();
				}

				// When converting value, a null model may indicate a failed conversion for an otherwise required
				// model (can't set a ValueType to null). This detects if a null model value is acceptable given the
				// current bindingContext. If not, an error is logged.
				if (model == null && !metadata.IsReferenceOrNullableType)
				{
					modelState.TryAddModelError(
						modelName,
						metadata
							.ModelBindingMessageProvider
							.ValueMustNotBeNullAccessor(valueProviderResult.ToString())
					);
				}
				else
				{
					bindingContext.Result = ModelBindingResult.Success(model);
				}
			}
			catch (Exception exception)
			{
				bool isFormatException = exception is FormatException;
				if (!isFormatException && exception.InnerException != null)
				{
					// Unlike TypeConverters, floating point types do not seem to wrap FormatExceptions. Preserve
					// this code in case a cursory review of the CoreFx code missed something.
					exception = ExceptionDispatchInfo.Capture(exception.InnerException).SourceException;
				}

				modelState.TryAddModelError(modelName, exception, metadata);
				// Conversion failed.
			}

			logger.DoneAttemptingToBindModel(bindingContext);
			return Task.CompletedTask;
		}
	}
}