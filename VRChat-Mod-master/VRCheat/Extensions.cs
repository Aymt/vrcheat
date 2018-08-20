using System;
using System.Reflection;
using VRC.Core;

namespace VRCheat
{
	
	public static class Extensions
	{
		
		public static ApiAvatar GetApiAvatar(this VRCPlayer player)
		{
			return (ApiAvatar)typeof(VRCPlayer).GetField("apiAvatar", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(player);
		}

		
		static Type smethod_0(RuntimeTypeHandle runtimeTypeHandle_0)
		{
			return Type.GetTypeFromHandle(runtimeTypeHandle_0);
		}

		
		static FieldInfo smethod_1(Type type_0, string string_0, BindingFlags bindingFlags_0)
		{
			return type_0.GetField(string_0, bindingFlags_0);
		}

		
		static object smethod_2(FieldInfo fieldInfo_0, object object_0)
		{
			return fieldInfo_0.GetValue(object_0);
		}
	}
}
