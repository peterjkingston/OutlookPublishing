namespace OlDocPublish.RulesMock
{
	public interface IRuleCriteria
	{
		RulePropertyCondition Condition { get; }
		RuleProperty Property { get; }
		RuleAction[] ResultingAction { get; }
		string[] Validation { get; }

		void DoAction(MailItem_ID mailID);
		bool Match(MailItem_ID mailID);
	}
}