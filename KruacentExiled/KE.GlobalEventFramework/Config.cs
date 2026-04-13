using Exiled.API.Interfaces;
using Exiled.Events.Commands.PluginManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework
{
	public class Config : IConfig
	{

		public bool IsEnabled { get; set; } = true;
		public bool Debug { get; set; } = false;
		[Description("Show the log when the plugin is registering global event (require Debug to be true)")]
		public bool ShowRegisteringLog { get; set; } = false;
		[Description("The chance a global event is not shown and show [REDACTED] instead (0~100)")]
		public int ChanceRedacted { get; set; } = 10;
		[Description("Activate or not the multiple descriptions")]
		public bool ActivateAltDescription { get; set; } = true;

	}
}
