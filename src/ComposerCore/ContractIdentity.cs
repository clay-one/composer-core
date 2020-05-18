using System;

namespace ComposerCore
{
	/// <summary>
	/// The primary key structure to identify component contracts, which contains a
	/// contract 'type' along with a name string.
	/// </summary>
	/// <remarks>
	/// The 'name' is null by default, but providing a 'type' for the contract is required.
	/// </remarks>
	public class ContractIdentity
	{
		public ContractIdentity(Type type, string name = null)
		{
            Type = type ?? throw new ArgumentNullException(nameof(type));
			Name = name;
		}

		public Type Type { get; }

		public string Name { get; }

		public bool Equals(Type type, string name)
		{
			return Type == type && Equals(Name, name);
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is ContractIdentity contractIdentity))
				return false;

			return Type == contractIdentity.Type && Equals(Name, contractIdentity.Name);
		}

		public override int GetHashCode()
		{
			return Type.GetHashCode() + 29*(Name != null ? Name.GetHashCode() : 0);
		}

		public static bool operator ==(ContractIdentity a, object b)
		{
			return Equals(a, b);
		}

		public static bool operator !=(ContractIdentity a, object b)
		{
			return !Equals(a, b);
		}
		
		public static implicit operator ContractIdentity(Type type)
		{
			return new ContractIdentity(type);
		}
	}
}