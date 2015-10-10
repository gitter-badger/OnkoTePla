using System;
using System.Collections;
using System.Collections.Generic;

namespace bytePassion.Lib.Utils
{
	public static class Guard
	{
						
		public static void ArgumentNotNull (object argumentValue)
		{
			if (argumentValue == null)
			{				
				Type valueType = argumentValue.GetType();									
				throw new ArgumentNullException(valueType.Name);
			}
		}
			
							
		public static void KeyNotFoundInDictionary (IDictionary dictionary, object key)
		{
			ArgumentNotNull(key);
			ArgumentNotNull(dictionary);

			if (!dictionary.Contains(key))
			{
				Type valueType = dictionary.GetType();
				throw new KeyNotFoundException($"{key} is not found in {valueType.Name}");
			}
		}
		

		public static void KeyAlreadyExistsInDictionary (IDictionary dictionary, object key)
		{
			ArgumentNotNull(key);
			ArgumentNotNull(dictionary);

			if (dictionary.Contains(key))
			{
				Type valueType = dictionary.GetType();
				throw new ArgumentException($"{key} is already defined in {valueType.Name}");				
			}
		}

		
		public static void KeyAlreadyExistsInDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
		{
			ArgumentNotNull(key);
			ArgumentNotNull(dictionary);

			if (dictionary.ContainsKey(key) == true)
			{
				Type valueType = dictionary.GetType();
				throw new ArgumentException($"{key} is already defined in {valueType.Name}");
			}
		}
		

		public static T SafeCast<T>(object instance) where T : class
		{
			var instanceAsT = instance as T;
			ArgumentNotNull(instanceAsT);
			return instanceAsT;
		}		
	}
}
