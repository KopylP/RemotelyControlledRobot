using System;

namespace RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;

[AttributeUsage(validOn: AttributeTargets.Class, Inherited = false)]

public sealed class ControllerEnableAttribute(string configurationKey) : Attribute
{
	public string ConfigurationKey { get; } = configurationKey;
}

