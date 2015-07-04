using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using Xunit;

namespace bytePassion.Lib.Test
{
	public class DateTest
	{
		[Theory]
		[MemberData("TestDataGetWeekDayFromDateTest")]
		public void GetWeekDayFromDateTest (Date d, DayOfWeek expectedWeekDay)
		{
			var computedWeekday = Date.GetDayOfWeekFrom(d);

			Assert.Equal(expectedWeekDay, computedWeekday);
		}

		public static readonly IEnumerable<object[]> TestDataGetWeekDayFromDateTest = 
			new[]
			{
				new object[]{ new Date(3,7,2015), DayOfWeek.Friday    },
				new object[]{ new Date(4,7,2015), DayOfWeek.Saturday  },
				new object[]{ new Date(5,7,2015), DayOfWeek.Sunday    },
				new object[]{ new Date(6,7,2015), DayOfWeek.Monday    },
				new object[]{ new Date(7,7,2015), DayOfWeek.Tuesday   },
				new object[]{ new Date(8,7,2015), DayOfWeek.Wednesday },
				new object[]{ new Date(9,7,2015), DayOfWeek.Thursday  }				
			};
	}
}
