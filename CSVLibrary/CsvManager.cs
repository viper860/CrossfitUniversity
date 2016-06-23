using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Library
{

public class CsvManager<T> where T: new()
{
    public char DefaultSeparator { get; set; }
    public List<CsvFieldTarget> CsvFieldsToMap { get; set; }
    public string FilePath { get; set; }
    public CsvReader Reader { get; set; }
    public bool IsFirstLineColumnName { get; private set; }

    public CsvManager(string filePath, bool isFirstLineColumnName, char separator)
    {
        CsvFieldsToMap = new List<CsvFieldTarget>();
        //Setting default separator 
        DefaultSeparator = separator;
        IsFirstLineColumnName = isFirstLineColumnName;
        FilePath = filePath;
    }


    public CsvManager(string filePath, bool isFirstLineColumnName):this(filePath, isFirstLineColumnName,','){}

    #region Public methods

    public void SetField(Expression<Func<T, string>> expression, int position)
    {
        GetAndSetFieldTarget(expression, position);
    }

    public void SetField(Expression<Func<T,ValueType>> expression, int position)
    {
        GetAndSetFieldTarget(expression, position);
    }

    public void SetField(Expression<Func<T, DateTime>> expression, int position)
    {
        GetAndSetFieldTarget(expression, position);
    }

    public List<T> GetObjectList()
    {
        Reader = new CsvReader(FilePath, CsvFieldsToMap, DefaultSeparator, IsFirstLineColumnName);
        var csvRows = GetRowsFromFile();
        var resultList = new List<T>(csvRows.Count);
        foreach (var csvRow in csvRows)
        {
            var destinationObject = new T();
            var createdObj = SetPropertyViaReflection(destinationObject, csvRow);
            resultList.Add(createdObj);
        }
        return resultList;
    }

    #endregion

        

    #region Private methods

    private void GetAndSetFieldTarget(Expression expression, int position)
    {
        var property = GetMemberInfo(expression);
        CsvFieldsToMap.Add(new CsvFieldTarget()
        {
            FieldName = property.Name,
            Position = position
        });
    }


    private List<CsvRow> GetRowsFromFile()
    {
        var csvRows = Reader.ReadCsvRows();
        return csvRows;

    }

    private T SetPropertyViaReflection(T destinationObj, CsvRow csvRow)
    {
        Type type = destinationObj.GetType();
            foreach (var csvFieldResult in csvRow.CsvFieldsResult)
            {
                PropertyInfo prop = type.GetProperty(csvFieldResult.FieldName);

                var propertyType = prop.PropertyType;
                //var underlyingType = Nullable.GetUnderlyingType(type);
                //if (underlyingType != null)
                //{
                //    //an underlying nullable type, so the type is nullable
                //    //apply logic for null or empty test
                //    if (String.IsNullOrEmpty(csvFieldResult.FieldValue.ToString()))
                //    {
                //        csvFieldResult.FieldValue = null;
                //    }
                //}
                if ((propertyType.Name == "Int32" || propertyType.Name == "Double") && (String.IsNullOrEmpty(csvFieldResult.FieldValue.ToString())))
                {
                    csvFieldResult.FieldValue = 0;
                } 
                //var convertedValue;
                if (propertyType.Name == "DateTime")
                {
                    string predate = csvFieldResult.FieldValue.ToString();
                    csvFieldResult.FieldValue = DateTime.ParseExact(csvFieldResult.FieldValue.ToString(), "d/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString();
                }
                //else
                //{
                   var convertedValue = Convert.ChangeType(csvFieldResult.FieldValue, propertyType);
                //}
            prop.SetValue(destinationObj, convertedValue, null);
        }
        return destinationObj;
    }

    private static MemberInfo GetMemberInfo(Expression method)
    {
        var lambda = method as LambdaExpression;
        if (lambda == null)
            throw new InvalidCastException("Invalid lambda expression");

        MemberExpression memberExpression = null;

        switch (lambda.Body.NodeType)
        {
            case ExpressionType.Convert:
                memberExpression =
                    ((UnaryExpression) lambda.Body).Operand as MemberExpression;
                break;
            case ExpressionType.MemberAccess:
                memberExpression = lambda.Body as MemberExpression;
                break;
        }

        if (memberExpression == null)
            throw new ArgumentException("Invalid expression");

        return memberExpression.Member;
    }


    #endregion
}
}

