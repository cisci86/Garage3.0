using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Microsoft.Extensions.Logging
{
	/// <summary>
	/// Extensions for the <see cref="ILogger"/>.
	/// </summary>
	internal static class LoggerExtensions
	{
		// Found here:
		// https://github.com/dotnet/aspnetcore/blob/a4c45262fb8549bdb4f5e4f76b16f98a795211ae/src/Mvc/Mvc.Core/src/MvcCoreLoggerExtensions.cs

		public static void AttemptingToBindModel(this ILogger logger, ModelBindingContext bindingContext)
		{
			if (!logger.IsEnabled(LogLevel.Debug))
				return;

			var modelMetadata = bindingContext.ModelMetadata;
			switch (modelMetadata.MetadataKind)
			{
				case ModelMetadataKind.Parameter:
					logger.Log(LogLevel.Debug,
						new EventId(44, "AttemptingToBindParameterModel"),
						$"Attempting to bind parameter '{modelMetadata.ParameterName}' of type '{modelMetadata.ModelType}' using the name '{bindingContext.ModelName}' in request data ...");
					break;

				case ModelMetadataKind.Property:
					logger.Log(LogLevel.Debug,
						new EventId(13, "AttemptingToBindPropertyModel"),
						$"Attempting to bind property '{modelMetadata.ContainerType}.{modelMetadata.PropertyName}' of type '{modelMetadata.ModelType}' using the name '{bindingContext.ModelName}' in request data ...");
					break;

				case ModelMetadataKind.Type:
					logger.Log(LogLevel.Debug,
						new EventId(24, "AttemptingToBindModel"),
						$"Attempting to bind model of type '{bindingContext.ModelType}' using the name '{bindingContext.ModelName}' in request data ...");
					break;
			}
		}

		public static void FoundNoValueInRequest(this ILogger logger, ModelBindingContext bindingContext)
		{
			if (!logger.IsEnabled(LogLevel.Debug))
				return;

			var modelMetadata = bindingContext.ModelMetadata;
			switch (modelMetadata.MetadataKind)
			{
				case ModelMetadataKind.Parameter:
					logger.Log(LogLevel.Debug,
						new EventId(16, "FoundNoValueForParameterInRequest"),
						$"Could not find a value in the request with name '{bindingContext.ModelName}' for binding parameter '{modelMetadata.ParameterName}' of type '{bindingContext.ModelType}'.");
					break;

				case ModelMetadataKind.Property:
					logger.Log(LogLevel.Debug,
						new EventId(15, "FoundNoValueForPropertyInRequest"),
						$"Could not find a value in the request with name '{bindingContext.ModelName}' for binding property '{modelMetadata.ContainerType}.{modelMetadata.PropertyName}' of type '{bindingContext.ModelType}'.");
					break;

				case ModelMetadataKind.Type:
					logger.Log(LogLevel.Debug,
						new EventId(46, "FoundNoValueInRequest"),
						$"Could not find a value in the request with name '{bindingContext.ModelName}' of type '{bindingContext.ModelType}'.");
					break;
			}
		}

		public static void DoneAttemptingToBindModel(this ILogger logger, ModelBindingContext bindingContext)
		{
			if (!logger.IsEnabled(LogLevel.Debug))
				return;

			var modelMetadata = bindingContext.ModelMetadata;
			switch (modelMetadata.MetadataKind)
			{
				case ModelMetadataKind.Parameter:
					logger.Log(LogLevel.Debug,
						new EventId(45, "DoneAttemptingToBindParameterModel"),
						$"Done attempting to bind parameter '{modelMetadata.ParameterName}' of type '{modelMetadata.ModelType}'.");
					break;

				case ModelMetadataKind.Property:
					logger.Log(LogLevel.Debug,
						new EventId(14, "DoneAttemptingToBindPropertyModel"),
						$"Done attempting to bind property '{modelMetadata.ContainerType}.{modelMetadata.PropertyName}' of type '{modelMetadata.ModelType}'.");
					break;

				case ModelMetadataKind.Type:
					logger.Log(LogLevel.Debug,
						new EventId(25, "DoneAttemptingToBindModel"),
						$"Done attempting to bind model of type '{bindingContext.ModelType}' using the name '{bindingContext.ModelName}'.");
					break;
			}
		}
	}
}