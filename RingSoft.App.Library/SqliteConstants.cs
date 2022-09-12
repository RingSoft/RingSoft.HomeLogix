using RingSoft.DbLookup.EfCore;

namespace RingSoft.App.Library
{
    public class SqliteConstants
    {
        public static string IntegerColumnType => DbConstants.IntegerColumnType;
        public static string StringColumnType => DbConstants.StringColumnType;
        public static string DecimalColumnType => DbConstants.DecimalColumnType;
        public static string DateColumnType => DbConstants.DateColumnType;
        public static string ByteColumnType => DbConstants.ByteColumnType;
        public static string BoolColumnType => DbConstants.BoolColumnType;
        public static string MemoColumnType => DbConstants.MemoColumnType;
    }
}
