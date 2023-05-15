using System.Reflection;

namespace Learning_ASP.NET.Services;

public static class CsvParser
{
    public static List<T> Parse<T>(string filePath)
    {
        var csvText = File.ReadAllLines(filePath);
        var data = csvText.Where(t => !t.StartsWith("# ")).ToArray();
        var type = typeof(T);
        var result = new List<T>();
        var fields = type.GetFields(BindingFlags.Instance
                                    | BindingFlags.NonPublic
                                    | BindingFlags.Public 
                                    | BindingFlags.Static);
        if (!CheckFieldsNames(fields, data[0].Split(';')))
        {
            throw new ArgumentException("Названия полей в файле указаны неверно.");
        }

        data = data.Skip(1).ToArray();
            
        for (var i = 0; i < data.Length; i++)
        {
            var line = data[i];
            var lineElements = line.Split(';');
            if (lineElements.Length != fields.Length)
            {
                throw new ArgumentException($"Недостаточно данных в строке {i}");
            }

            var o = CreateInstance(type, fields, lineElements);
            result.Add((T) o);
        }

        return result;
    }

    private static object CreateInstance(Type type, FieldInfo[] fields, string[] data)
    {
        var o = Activator.CreateInstance(type);
        for (var j = 0; j < fields.Length; j++)
        {
            if (fields[j].FieldType == typeof(string))
            {
                fields[j].SetValue(o, data[j]);
            }
            else if (fields[j].FieldType == typeof(int))
            {
                fields[j].SetValue(o, int.Parse(data[j]));
            }
        }

        return o!;
    }

    private static bool CheckFieldsNames(FieldInfo[] fields, string[] names)
    {
        if (fields.Length != names.Length)
        {
            return false;
        }

        for (var i = 0; i < fields.Length; i++)
        {
            if (names[i].Contains('_'))
            {
                var index = names[i].IndexOf('_');
                names[i] = names[i].Remove(index, 1).Replace(names[i][index], names[i][index].ToString().ToUpper()[0]);
            }

            var fieldNameAdapted = fields[i].Name.Split('<')[1].Split('>')[0].ToLower();
            var flag = names[i].Equals(fieldNameAdapted);
            if (!flag)
            {
                return false;
            }
        }

        return true;
   }
}