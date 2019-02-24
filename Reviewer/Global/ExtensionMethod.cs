using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reviewer.Global
{
	public static class ExtensionMethod
	{
		// struct, enum일 경우 가비지 생성 ( todo 제약을 좀 더 걸어야 하는데 일단 패스하고 작업 )
		static public bool CheckMatch<T>(this List<T> li1, List<T> li2)
		{
			if (li1 == null && li2 == null)
			{
				return true;
			}
			else if (li1 == null || li2 == null)
			{
				return false;
			}

			if (li1.Count != li2.Count)
			{
				return false;
			}
				
			for (int i = 0; i < li1.Count; i++)
			{
				if ( li1[i].Equals(li2[i]) == false )
				{
					return false;
				}					
			}

			return true;
		}
		
		public static bool HasDuplicatedValue<T>(this List<T> list)
		{
			return list.GroupBy(n => n).Any(c => c.Count() > 1);
		}
	}



	

}
