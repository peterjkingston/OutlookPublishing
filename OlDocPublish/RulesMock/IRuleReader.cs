using System.Collections.Generic;

namespace OutlookAddInController
{
	public interface IRuleReader
	{
		List<IRuleCriteria> GetCriteria(string criteria_path);
	}
}