using DBDataGenerator.DataModels.DataGenerateConfigModels;
using DBDataGenerator.DataModels.Enums;
using DBDataGenerator.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.Services
{
    /// <summary>
    /// 数据生成配置服务
    /// </summary>
    public class DataGenerateConfigService
    {
        public DataGenerateConfigService()
        {
        }


        /// <summary>
        /// 获取或初始化列数据生成配置
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="tableName">数据表名称</param>
        /// <param name="columnSchemas">列信息</param>
        /// <returns></returns>
        public List<ColumnGenerateDataConfig> GetColumnGenerateDataConfigs(string databaseName,
            string tableName,
            List<ColumnSchema> columnSchemas)
        {
            List<ColumnGenerateDataConfig> columnGenerateDataConfigs = new List<ColumnGenerateDataConfig>();

            if (databaseName == null)
            {
                throw new ArgumentNullException("数据库名称不能为空");
            }

            if (tableName == null)
            {
                throw new ArgumentNullException("数据表名称不能为空");
            }

            if (columnSchemas == null || columnSchemas.Count <= 0)
            {
                return columnGenerateDataConfigs;
            }

            // 生成配置
            foreach (ColumnSchema columnSchema in columnSchemas)
            {
                ColumnGenerateDataConfig newConfig = InitColumnGenerateDataConfig(databaseName, tableName, columnSchema);
                columnGenerateDataConfigs.Add(newConfig);
            }

            // 如果有默认值为自增类型的列，则将其数据生成类型设置为自增ID
            InitAutoIncrementKeyIfExist(columnGenerateDataConfigs, columnSchemas);

            return columnGenerateDataConfigs;
        }



        /// <summary>
        /// 更新列数据生成配置
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="tableName">数据表名称</param>
        /// <param name="newConfig">数据表某一列的新配置</param>
        /// <param name="columnGenerateDataConfigs">数据库表所有列的生成配置</param>
        /// <returns></returns>
        public List<ColumnGenerateDataConfig> UpdateColumnGenerateDataConfig(
            string databaseName,
            string tableName,
            ColumnGenerateDataConfig newConfig,
            List<ColumnGenerateDataConfig> columnGenerateDataConfigs)
        {
            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(tableName)
                || newConfig == null || columnGenerateDataConfigs == null)
            {
                return columnGenerateDataConfigs;
            }

            // 寻找旧的配置
            ColumnGenerateDataConfig? oldConfig = columnGenerateDataConfigs.FirstOrDefault(x => x.ColumnName == newConfig.ColumnName);

            // 更新旧配置
            if(oldConfig != null)
            {
                oldConfig.DataGenerateType = newConfig.DataGenerateType;
                oldConfig.DataGenerateConfig = newConfig.DataGenerateConfig;
            }

            return columnGenerateDataConfigs;
        }
        #region 私有方法


        /// <summary>
        /// 初始化列配置信息
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <param name="columnSchema"></param>
        /// <returns></returns>
        private ColumnGenerateDataConfig InitColumnGenerateDataConfig(string databaseName, string tableName, ColumnSchema columnSchema)
        {
            ColumnGenerateDataConfig columnGenerateDataConfig = new ColumnGenerateDataConfig();

            columnGenerateDataConfig.DatabaseName = databaseName;// 数据库名称
            columnGenerateDataConfig.TableName = tableName;// 数据表名称
            columnGenerateDataConfig.ColumnName = columnSchema.COLUMN_NAME;// 列名称

            // 解析数据大致类别（整数、实数、日期、文本）
            columnGenerateDataConfig.MysqlDataTypeCategoryEnum = GetMysqlDataTypeCategoryEnum(columnSchema.COLUMN_TYPE);

            // 根据数据分类获取默认的数据生成类型
            columnGenerateDataConfig.DataGenerateType = GetDefaultDataGenerateTypeEnum(columnGenerateDataConfig.MysqlDataTypeCategoryEnum);

            // 根据数据生成类型，获取默认生成配置
            columnGenerateDataConfig.DataGenerateConfig = GetDefaultDataGenerateConfig(columnGenerateDataConfig.DataGenerateType);
            columnGenerateDataConfig.DataGenerateConfig.DatabaseName = databaseName;
            columnGenerateDataConfig.DataGenerateConfig.TableName = tableName;
            columnGenerateDataConfig.DataGenerateConfig.ColumnName = columnSchema.COLUMN_NAME;

            return columnGenerateDataConfig;
        }


        /// <summary>
        /// 解析列字段的mysql类型，获取大概的类型分类（整数、实数、日期、文本）
        /// </summary>
        /// <param name="columnType">Mysql列字段类型</param>
        /// <returns>类型分类（整数、实数、日期、文本）</returns>
        private MysqlDataTypeCategoryEnum GetMysqlDataTypeCategoryEnum(string columnType)
        {
            if (string.IsNullOrEmpty(columnType) || string.IsNullOrWhiteSpace(columnType))
            {
                return MysqlDataTypeCategoryEnum.Text;
            }

            // 去掉mysql类型的括号，方便进行switch...case语句判断,如varchar(255)变为varchar
            int charIndex = columnType.IndexOf('(');
            if (charIndex != -1)
            {
                columnType = columnType.Substring(0, charIndex);
            }

            MysqlDataTypeCategoryEnum mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Text;

            switch (columnType)
            {
                // 整数类型判断
                case "tinyint": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Integer; break;
                case "smallint": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Integer; break;
                case "mediumint": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Integer; break;
                case "int": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Integer; break;
                case "bigint": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Integer; break;
                case "bit": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Integer; break;

                // 实数类型判断
                case "float": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Real; break;
                case "double": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Real; break;
                case "decimal": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Real; break;

                // 文本类型判断
                case "varchar": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Text; break;
                case "char": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Text; break;
                case "tinytext": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Text; break;
                case "text": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Text; break;
                case "mediumtext": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Text; break;
                case "longtext": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Text; break;
                case "json": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Text; break;

                // 时间类型判断
                case "date": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Datetime; break;
                case "time": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Datetime; break;
                case "year": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Datetime; break;
                case "datetime": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Datetime; break;
                case "timestamp": mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Datetime; break;

                default:
                    mysqlDataTypeCategoryEnum = MysqlDataTypeCategoryEnum.Text; break;
            };

            return mysqlDataTypeCategoryEnum;
        }


        /// <summary>
        /// 根据数据分类获取默认的数据生成类型
        /// </summary>
        /// <param name="mysqlDataTypeCategoryEnum"></param>
        /// <returns></returns>
        private DataGenerateTypeEnum GetDefaultDataGenerateTypeEnum(MysqlDataTypeCategoryEnum mysqlDataTypeCategoryEnum)
        {
            DataGenerateTypeEnum dataGenerateTypeEnum = DataGenerateTypeEnum.FixedString;

            switch (mysqlDataTypeCategoryEnum)
            {
                // 整数类型，默认生成随机整数
                case MysqlDataTypeCategoryEnum.Integer: dataGenerateTypeEnum = DataGenerateTypeEnum.RandomInt; break;
                // 实数类型，默认生成随机浮点数
                case MysqlDataTypeCategoryEnum.Real: dataGenerateTypeEnum = DataGenerateTypeEnum.RandomFloat; break;
                // 文本类型，默认生成固定文本
                case MysqlDataTypeCategoryEnum.Text: dataGenerateTypeEnum = DataGenerateTypeEnum.FixedString; break;
                // 时间类型，默认生成当前时间
                case MysqlDataTypeCategoryEnum.Datetime: dataGenerateTypeEnum = DataGenerateTypeEnum.Datetime; break;

                default:
                    dataGenerateTypeEnum = DataGenerateTypeEnum.FixedString; break;
            }

            return dataGenerateTypeEnum;
        }


        /// <summary>
        /// 根据数据生成类型，获取默认的生成配置
        /// </summary>
        /// <param name="dataGenerateTypeEnum"></param>
        /// <returns></returns>
        private IDataGenerateConfig GetDefaultDataGenerateConfig(DataGenerateTypeEnum dataGenerateTypeEnum)
        {
            IDataGenerateConfig dataGenerateConfig = null;

            switch (dataGenerateTypeEnum)
            {
                case DataGenerateTypeEnum.RandomInt: dataGenerateConfig = new RandomIntGenerateConfig(); break;
                case DataGenerateTypeEnum.RandomFloat: dataGenerateConfig = new RandomFloatGenerateConfig(); break;
                case DataGenerateTypeEnum.FixedString: dataGenerateConfig = new FixedStringGenerateConfig(); break;
                case DataGenerateTypeEnum.Datetime: dataGenerateConfig = new DatetimeGenerateConfig(); break;
                default: dataGenerateConfig = new FixedStringGenerateConfig(); break;
            }

            return dataGenerateConfig;
        }


        /// <summary>
        /// 检查是否有自增ID类型的列，将该字段初始化为自增ID类型
        /// </summary>
        /// <param name="columnGenerateDataConfigs">列生成配置信息</param>
        /// <param name="columnSchemas">列信息</param>
        private void InitAutoIncrementKeyIfExist(List<ColumnGenerateDataConfig> columnGenerateDataConfigs, List<ColumnSchema> columnSchemas)
        {
            foreach (ColumnSchema column in columnSchemas)
            {
                if (column.EXTRA != "auto_increment")
                {
                    continue;
                }

                ColumnGenerateDataConfig? generateDataConfig = columnGenerateDataConfigs.Where(x => x.ColumnName == column.COLUMN_NAME).FirstOrDefault();
                if (generateDataConfig != null)
                {
                    generateDataConfig.DataGenerateType = DataGenerateTypeEnum.AutoIncrementID;
                    generateDataConfig.DataGenerateConfig = null;// 自增主键的数据是自动生成的，所以不需要用户专门去设置生成的值
                }
            }
        }
        #endregion
    }
}
