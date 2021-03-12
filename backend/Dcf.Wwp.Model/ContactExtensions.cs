using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model
{
	public static class ContactExtensions
	{
		public static IContact FirstByType(this ICollection<IContact> contacts, string title)
		{
			if (contacts == null || contacts.Count < 1)
				return null;

			return contacts.FirstOrDefault(x => x.ContactTitleType != null && String.Equals(x.ContactTitleType.Name, title));
		}
	}
}