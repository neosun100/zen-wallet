﻿using System;
using MsgPack;

namespace NBitcoin.Protocol
{
	/// <summary>
	/// Ask for known peer addresses in the network
	/// </summary>
	public class GetAddrPayload
	{
		public override string ToString()
		{
			return "GetAddr";
		}
	}
}
