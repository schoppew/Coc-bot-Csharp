namespace CoC.Bot.UI.Converters
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Windows.Data;

	public class BooleanToHideShowPathConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value == true)
			{
				return @"M13.068723,6.3391984C10.02902,7.8790036 7.4492722,9.9387431 5.6594467,11.588534 8.9291277,14.598154 14.83855,19.027594 21.327917,19.027594 27.817283,19.027594 33.736705,14.598154 36.996387,11.588534 35.21656,9.9387431 32.626813,7.8790036 29.597109,6.3391984 29.837086,7.0291111 29.967073,7.74902 29.967073,8.508924 29.967073,12.668398 26.097451,16.037971 21.327917,16.037971 16.558382,16.037971 12.698759,12.668398 12.698759,8.508924 12.698759,7.74902 12.828746,7.0291111 13.068723,6.3391984z M21.327917,0C33.106767,0 42.665833,11.588534 42.665833,11.588534 42.665833,11.588534 33.106767,23.177069 21.327917,23.177069 9.5490665,23.177069 0,11.588534 0,11.588534 0,11.588534 9.5490665,0 21.327917,0z";
			}
			return @"M31.687237,10.667C38.231297,14.096666 42.666001,19.463814 42.666001,19.463814 42.666001,19.463814 33.114207,31.052 21.333262,31.052 19.11721,31.052 16.981756,30.641903 14.974,29.97641L17.968835,26.516945C19.064911,26.760542 20.189886,26.903641 21.333262,26.903641 27.822621,26.903641 33.739193,22.471584 37.004723,19.463814 35.21846,17.82023 32.632517,15.76165 29.602583,14.222665 29.836977,14.907658 29.968575,15.631751 29.968575,16.382744 29.968575,20.541902 26.101458,23.913969 21.333262,23.913969 20.97387,23.913969 20.622277,23.889471 20.275885,23.851471L28.911198,13.874968z M21.333649,7.8749999C22.367567,7.8749999,23.375384,7.991208,24.369999,8.1538057L23.543387,9.1103305 23.544586,9.1103305 13.919428,20.229303C13.148615,19.102821 12.698008,17.790342 12.698008,16.382666 12.698008,15.627777 12.82831,14.901288 13.065114,14.213799 10.031464,15.749875 7.4478719,17.817741 5.6614729,19.463716 7.0313649,20.726396 8.8751452,22.238071 11.028781,23.570051L8.3228858,26.695002C3.2681233,23.407453 -2.3841858E-07,19.463716 0,19.463716 -2.3841858E-07,19.463716 9.5521562,7.8749999 21.333649,7.8749999z M34.252037,0L36.449997,1.9038253 6.0807195,36.990002 3.8829998,35.089188z";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class BooleanToHideShowImageSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value == true)
			{
				return @"/Assets/Images/HideThumb.png";
			}
			return @"/Assets/Images/ShowThumb.png";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class BooleanToHideShowStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value == true)
			{
				return Properties.Resources.Restore;
			}
			return Properties.Resources.Hide;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
