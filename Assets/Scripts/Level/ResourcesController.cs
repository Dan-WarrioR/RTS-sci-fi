public class ResourcesController
{
	private static ResourcesController _instance;

	public static ResourcesController Instance
	{
		get
		{
			if (_instance == null)
				_instance = new ResourcesController();

			return _instance;
		}

		private set
		{
			_instance = value;
		}
	}

	public int MaxResourcesAmount => 400;
	
	public int Resources
	{
		get
		{
			return _resourcesAmount;
		}
		set
		{
			if (value > 0 && value <= MaxResourcesAmount)
			{
				_resourcesAmount = value;
			}
		}
	}
	private int _resourcesAmount;



	public int MaxArmyCapacity => 200;
	
	public int ArmyCapacity 
	{
		get
		{
			return _armyCapacity;
		}
		set
		{
			if (value > 0 && value <= MaxArmyCapacity)
			{
				_armyCapacity = value;
			}
		}
	}

	private int _armyCapacity;


	public int ArmyUnitCount
	{
		get
		{
			return _armyUnitCount;
		}
		set
		{
			if (value >= 0 && value <= MaxArmyCapacity)
			{
				_armyUnitCount = value;
			}
		}
	}

	private int _armyUnitCount;
}
