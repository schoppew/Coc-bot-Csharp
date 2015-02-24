/*
 * Modern UI (Metro) Charts for Windows 8, WPF, Silverlight
 * 
 * The charts have been developed by Torsten Mandelkow.
 * http://modernuicharts.codeplex.com/
 */

namespace CoC.Bot.Chart
{
	using System;
	using System.Reflection;
	using System.Linq;

    public static class Extensions
    {
        public static PropertyInfo[] GetAllProperties(this Type type)
        {
			return type.GetProperties();
        }
    }
}
