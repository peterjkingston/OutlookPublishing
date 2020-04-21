using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlDocPublish.RulesMock
{
	[Flags]
	public enum RuleProperty
	{
		SenderAddress,
		Body,
		Subject,
		ToAddress,
		CCAddress
	}
}
