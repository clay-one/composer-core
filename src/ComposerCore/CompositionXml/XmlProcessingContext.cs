using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Definitions;


namespace ComposerCore.CompositionXml
{
	internal class XmlProcessingContext
	{
		private readonly List<string> _runningLocation;

		public XmlProcessingContext(IComponentContext componentContext)
		{
			ComponentContext = componentContext;
			TypeCache = new TypeCache();
			_runningLocation = new List<string>();
			Errors = new List<XmlProcessingError>();
		}

		public XmlProcessingContext(XmlProcessingContext xmlProcessingContext)
		{
			ComponentContext = xmlProcessingContext.ComponentContext;
			TypeCache = xmlProcessingContext.TypeCache;
			_runningLocation = new List<string>();
			Errors = new List<XmlProcessingError>();
		}

		public IComponentContext ComponentContext { get; }

		public TypeCache TypeCache { get; }

		public List<XmlProcessingError> Errors { get; }

		public string RunningLocationText => string.Join(" > ", _runningLocation.ToArray());

	    public void EnterRunningLocation(string locationText)
		{
			if (locationText == null)
				throw new ArgumentNullException(nameof(locationText));

			_runningLocation.Add(locationText);
		}

		public void LeaveRunningLocation()
		{
			if (_runningLocation.Count < 1)
				throw new InvalidOperationException("There is no running location to leave!");

			_runningLocation.RemoveAt(_runningLocation.Count - 1);
		}

		public void ReportError(string errorText)
		{
			XmlProcessingError error;

			error.Message = errorText;
			error.RunningLocation = RunningLocationText;

			Errors.Add(error);
		}

		public void ThrowIfErrors()
		{
			if (Errors.Count == 0)
				return;

			var message = Errors.Aggregate("Errors encountered while processing Composition XML file.\r\n\r\n",
			                                (current, error) =>
			                                current +
			                                ("Error text: " + error.Message + "\r\nLocation: " + error.RunningLocation + "\r\n\r\n"));

			throw new CompositionXmlValidationException(message);
		}

		#region Nested type: XmlProcessingError

		public struct XmlProcessingError
		{
			public string Message;
			public string RunningLocation;
		}

		#endregion
	}
}