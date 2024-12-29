using Exiled.API.Interfaces;
using Exiled.Events.Commands.PluginManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentE.GlobalEventFramework
{
	internal class Config : IConfig
	{

		public bool IsEnabled { get; set; } = true;
		public bool Debug { get; set; } = true;
		[Description("Show the log when the plugin is registering global event (require Debug to be true)")]
		public bool ShowRegisteringLog { get; set; } = false;

	}
}
