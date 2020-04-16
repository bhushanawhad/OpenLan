using System.Diagnostics;
using System.Globalization;

namespace CustomerPortal.Plugins.Diagnostics
{
	/// <summary>
	/// Code tracing methods.
	/// </summary>
	/// <remarks>
	/// The framework uses several built-in <see cref="TraceSource"/> objects for tracing. These sources can be identified by name and reconfigured.
	/// <example>
	/// Controlling the trace output from the configuration.
	/// <code>
	/// <![CDATA[
	/// <configuration>
	///  <system.diagnostics>
	///   <sharedListeners>
	///    <add name="Console" type="System.Diagnostics.ConsoleTraceListener"/>
	///   </sharedListeners>
	///    <add name="Workflow"
	///     value="All" [Off | Critical | Error | Warning | Information | Verbose | All]
	///    />
	///   </switches>
	///   <sources>
	///    <source name="Workflow">
	///     <listeners>
	///      <add name="Console"/>
	///     </listeners>
	///    </source>
	///   </sources>
	///   <trace autoflush="true"/>
	///  </system.diagnostics>
	/// </configuration>
	/// ]]>
	/// </code>
	/// </example>
	/// </remarks>
	internal static class Tracing
	{
		static Tracing()
		{
			Workflow = new TraceSource("Workflow", SourceLevels.All);
		}

		public static TraceSource Workflow { get; private set; }

		public static void WorkflowInformation(string className, string memberName, string format, params object[] args)
		{
			TraceEvent(Workflow, TraceEventType.Information, className, memberName, format, args);
		}

		public static void WorkflowError(string className, string memberName, string format, params object[] args)
		{
			TraceEvent(Workflow, TraceEventType.Error, className, memberName, format, args);
		}

		public static void WorkflowEvent(TraceEventType eventType, string className, string memberName, string format, params object[] args)
		{
			TraceEvent(Workflow, eventType, className, memberName, format, args);
		}

		private static void TraceEvent(
			TraceSource source,
			TraceEventType eventType,
			string className,
			string memberName,
			string format,
			params object[] args)
		{
			if (format == null)
			{
				format = "<null>";
			}

			try
			{
				// escape the curly brackets if no arguments are included

				if (args == null || args.Length == 0)
				{
					format = format.Replace("{", "{{").Replace("}", "}}");
				}

				source.TraceEvent(
					eventType,
					0,
					string.Format(CultureInfo.InvariantCulture, "{0}: {1}: {2}",
						className,
						memberName,
						string.Format(CultureInfo.InvariantCulture, format, args ?? new object[] {})));
			}
			catch
			{
				// tracing errors should not be fatal, handle error locally
			}
		}
	}
}
