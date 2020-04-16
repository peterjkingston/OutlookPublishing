using System.Collections.Generic;

namespace OlDocPublish.RulesMock
{
	public interface IRuleReader
	{
		List<IRuleCriteria> GetCriteria(string criteria_path);
	}
}