﻿using System;
using System.Globalization;
using System.Security.Principal;
using System.Web.Mvc;
using FailTracker.Core.Data;
using FailTracker.Core.Domain;
using System.Linq;

namespace FailTracker.Web.Infrastructure.ValueProviders
{
	public class CurrentUserValueProvider : IValueProvider
	{
		private readonly IIdentity _currentUser;
		private readonly IRepository<User> _users;

		public CurrentUserValueProvider(IIdentity currentUser, IRepository<User> users)
		{
			_currentUser = currentUser;
			_users = users;
		}

		private static bool BindingCurrentUser(string prefix)
		{
			return prefix.Equals("CurrentUser", StringComparison.OrdinalIgnoreCase);
		}

		public bool ContainsPrefix(string prefix)
		{
			if (BindingCurrentUser(prefix))
			{
				return true;
			}

			return false;
		}

		public ValueProviderResult GetValue(string key)
		{
			if (!BindingCurrentUser(key))
			{
				return null;
			}

			var user = _users.Query().Single(u => u.EmailAddress == _currentUser.Name);

			return new ValueProviderResult(user, user.ToString(), CultureInfo.CurrentCulture);
		}
	}
}