namespace appCoffeeMachine;
//Класс создания табуляции для вывода информации в консоль
public static class Tabulation
{
	public static String whiteSpace(String str, int count)
	{
		return addTabulation(str, count, ' ');
	}
	public static String underLine(String str, int count)
	{
		return addTabulation(str, count, '_');
	}
	public static String addTabulation(String str, int count, char tab)
	{
		for (int i = 0 + str.Length; i < count; i++)
		{
			str += tab;
		}
		return str;
	}
}