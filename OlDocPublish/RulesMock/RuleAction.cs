﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlDocPublish.RulesMock
{
	[Flags]
	public enum RuleAction
	{
		ProcessPDF,
		MoveToFolder
	}
}
