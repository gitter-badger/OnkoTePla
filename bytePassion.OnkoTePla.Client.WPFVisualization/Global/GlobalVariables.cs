using System;
using System.Collections.Generic;
using System.Windows;
using static bytePassion.Lib.Utils.Converter;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Global
{
	public static class GlobalVariables
	{
		private class VariableInfo
		{

			public VariableInfo(string identifier, Type variableType, object initialValue)
			{ 
				Identifier = identifier;
				InitialValue = initialValue;
				VariableType = variableType;
			}

			public string Identifier   { get; }
			public Type   VariableType { get; }
			public object InitialValue { get; }

		}

		public const string AppointmentGridSizeVariable  = "AppointmentGridSize";

		private static IEnumerable<VariableInfo> Names()
		{	
			// string -> identifier
			// Type   -> variableType
			// object -> initialValue

			return new List<VariableInfo>
			{
				new VariableInfo(AppointmentGridSizeVariable,  typeof(Size), new Size(400,400)),
			};
		}

		public static void RegisterAllGlobalVariables()
		{
			foreach (var variableInfo in Names())
			{
				GlobalAccess.ViewModelCommunication
							.RegisterGlobalViewModelVariable(variableInfo.Identifier,
														     ChangeTo(variableInfo.InitialValue, 
																	  variableInfo.VariableType));
			}
		}
	}
}
